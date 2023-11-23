using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Chair.DAL.Enums;
using Chair.DAL.Extension.Models;
using Microsoft.EntityFrameworkCore;

namespace Chair.DAL.Extension
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ToFilterView<T>(
            this IQueryable<T> query, FilterModel filter)
        {
            return filter == null ? query : query.ToFilterView(filter, out _);
        }

        public static IQueryable<T> ToFilterView<T>(
            this IQueryable<T> query, FilterModel filter, out int count)
        {
            if (filter == null)
            {
                count = query.Count();
                return query;
            }
            query = Filter(query, filter.Filter);
            count = query.Count();
            query = Sort(query, filter.Sort);
            query = Limit(query, filter.Take, filter.Skip);

            return query;
        }

        private static IQueryable<T> Filter<T>(
            IQueryable<T> queryable, Filter filter)
        {
            if (filter?.Logic == null) return queryable;
            var filters = GetAllFilters(filter);
            var values = filters.Select(f => f.Value).ToArray();
            var where = Transform(filter, filters);
            queryable = queryable.Where(where, values);

            return queryable;
        }

        private static IQueryable<T> Sort<T>(IQueryable<T> queryable, IEnumerable<Sort> sort)
        {
            if (sort == null || !sort.Any())
                return queryable;
            var ordering = string.Join(",",
                sort.Select(s => $"{s.Field} {s.Direction}"));

            var filterQuery = queryable;

            foreach (var s in sort.Where(s=>s.Field.Contains(".")))
            {
                filterQuery = filterQuery.Where(CheckFullPathForNotNull(s.Field));
            }
            return filterQuery.OrderBy(ordering);
        }

        private static (IQueryable<T>, IQueryable<T>) InternalSort<T>(IQueryable<T> filterQueryable, IQueryable<T> nullQueryable, IEnumerable<Sort> sort)
        {
            if (!sort.Any()) return (filterQueryable, nullQueryable);

            var sField = sort.First().Field;

            if (!sField.Contains(".")) return InternalSort<T>(filterQueryable, nullQueryable, sort.Skip(1));
            var nullQuery = filterQueryable.Where(CheckFullPathForNull(sField));
            var filterQuery = filterQueryable.Where(CheckFullPathForNotNull(sField));
            return InternalSort<T>(filterQuery, nullQuery.Concat(nullQueryable), sort.Skip(1));
        }

        private static IOrderedQueryable<T> CustomOrder<T>(IQueryable<T> filterQuery, string sField, string sDirection, bool isFirst)
        {
            return !isFirst ? (filterQuery as IOrderedQueryable<T>).ThenBy($"{sField} {sDirection}") : filterQuery.OrderBy($"{sField} {sDirection}");
        }

        private static IQueryable<T> Limit<T>(
            IQueryable<T> queryable, int? take, int? skip)
        {
            var query = queryable;
            if (skip != null)
                query = query.Skip((int) skip);
            if (take != null)
                query = query.Take((int) take);
            return query;
        }

        private static readonly IDictionary<string, string>
            Operators = new Dictionary<string, string>
            {
                {"=", "="},
                {"!=", "!="},
                {"<", "<"},
                {"<=", "<="},
                {">", ">"},
                {">=", ">="},
                {"startswith", "StartsWith"},
                {"endswith", "EndsWith"},
                {"contains", "Contains"},
                {"doesnotcontain", "Contains"},
                {"contained", "Contained"},
                {"hasflag", "HasFlag"},
            };

        public static IList<Filter> GetAllFilters(Filter filter)
        {
            var filters = new List<Filter>();
            GetFilters(filter, filters);
            return filters;
        }

        private static void GetFilters(Filter filter, ICollection<Filter> filters)
        {
            if (filter.Filters != null && filter.Filters.Any())
            {
                foreach (var item in filter.Filters)
                {
                    GetFilters(item, filters);
                }
            }
            else
            {
                filters.Add(filter);
            }
        }

        public static string Transform(Filter filter, IList<Filter> filters)
        {
            if (filter.Filters != null && filter.Filters.Any())
            {
                return "(" + string.Join(" " + filter.Logic + " ",
                    filter.Filters.Select(f => Transform(f, filters)).ToArray()) + ")";
            }

            var index = filters.IndexOf(filter);
            var comparison = Operators[filter.Operator];
            if (filter.Operator == "doesnotcontain")
            {
                return string.Format("({0} != null && !{0}.ToString().{1}(@{2}))",
                    filter.Field, comparison, index);
            }

            string format;
            var isPropObject = filter.Field.Contains(".");
            var checkObjNullStr = isPropObject ? CheckFullPathForNull(filter.Field) + " &&" : "";
            
            switch (comparison)
            {
                default:
                    format = "{0} {1} @{2}";
                    break;
                case "StartsWith":
                case "EndsWith":
                case "Contains":
                    format = checkObjNullStr + ((filter.MatchCaseSettings == MatchCaseEnum.IgnoreCase) 
                        ? "({0} != null && {0}.ToLower().{1}(@{2}.ToString().ToLower()))" 
                        : "({0} != null && {0}.{1}(@{2}))");
                    break;
                case "Contained":
                    format = CheckFullPathForNull(filter.Field) + " && (string.Join(\",\", @{2}).ToLower().IndexOf({0}.ToString().ToLower()) != -1)";
                    break;
                case "HasFlag":
                    format = "({0} & @{2}) = @{2}";
                    break;
            }

            return string.Format(format, filter.Field, comparison, index);
        }
        
        public static IQueryable<T> SearchByKeywordsWithFilter<T>(this IQueryable<T> queryable, FilterWithKeywordSearchModel filter, out int count)
        {
            if (filter.KeywordSearch == null ||
                filter.KeywordSearch.Search == null ||
                !filter.KeywordSearch.Search.All(kw => kw.IsValid))
            {
                return queryable.ToFilterView(filter, out count);
            }

            return queryable.GetMatches(filter.KeywordSearch.Search, filter.KeywordSearch.Logic.ToLowerInvariant()).ToFilterView(filter, out count);
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
            {
                return source.Where(predicate); 
            }
            else
            {
                return source;
            }
        }
        
        public static IQueryable<T> If<T>(this IQueryable<T> query, bool condition,
            params Func<IQueryable<T>, IQueryable<T>>[] transforms)
        {
            return condition 
                ? transforms.Aggregate(query, (current, transform) => transform.Invoke(current)) 
                : query;
        }

        private static string CheckFullPathForNull(string field)
        {
            var result = "";
            var last = "";
            var lines = field.Split('.');
            for (int i = 0; i < lines.Length; i++)
            {
                var line = last + lines[i];
                result += $"{last + lines[i]} != null && ";
                last = line + '.';
            }
            return result.TrimEnd(" &&".ToCharArray());
        }

        private static string CheckFullPathForNotNull(string field)
        {
            var result = "";
            var last = "";
            var lines = field.Split('.');
            for (int i = 0; i < lines.Length; i++)
            {
                var line = last + lines[i];
                result += $"{last + lines[i]} != null && ";
                last = line + '.';
            }
            return result.TrimEnd(" &&".ToCharArray());
        }

        private static IQueryable<T> GetMatches<T>(this IQueryable<T> source,
            IEnumerable<KeywordSearchModel> searchModels,
            string logic)
        {
            var longestWord = searchModels
                .OrderByDescending(kw => kw.LongestWord.Length)
                .FirstOrDefault();

            Func<T, bool> searchLogic;

            switch (logic)
            {
                case "or":
                    {
                        searchLogic = BuildOrPredicate<T>(searchModels);
                        break;
                    }
                case "and":
                default:
                    {
                        // var predicates = new List<Predicate<T>>();
                        // foreach (var model in searchModels)
                        // {
                        //     predicates.Add(BuildPredicateForKeywordSearch<T>(model));
                        // }
                        //
                        // source = source.GetByLongestWord(longestWord).ToList().AsQueryable();
                        // searchLogic = (match) => predicates.All(predicate => predicate(match));
                        searchLogic = BuildAndPredicate<T>(searchModels);
                        break;
                    }
            }

            return source.Where(searchLogic).AsQueryable();
        }

        /// <summary>
        /// Build predicate using lookahead Regex expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private static Predicate<T> BuildPredicateForKeywordSearch<T>(KeywordSearchModel searchModel)
        {
            var regexString = new Regex(SearchByKeywordsRegexBuilder.Build(searchModel.Words), RegexOptions.IgnoreCase);

            var paramObject = Expression.Parameter(typeof(T), "p");
            var body = SplitProperty(paramObject, searchModel.FieldName);

            body = Expression.Call(body, "ToLowerInvariant", Type.EmptyTypes);
            var constRegex = Expression.Constant(regexString);
            var methodInfo = typeof(Regex).GetMethod("IsMatch", new Type[] { typeof(string) });
            var paramsEx = new Expression[] { body };

            var lambdaBody = Expression.Call(constRegex, methodInfo, paramsEx);
            Expression<Func<T, bool>> queryExpr = Expression.Lambda<Func<T, bool>>(lambdaBody, paramObject);

            return new Predicate<T>(queryExpr.Compile());
        }

        /// <summary>
        /// Get all entries by the longest word in the query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private static IQueryable<T> GetByLongestWord<T>(this IQueryable<T> source, KeywordSearchModel searchModel)
        {
            var param = Expression.Parameter(typeof(T), "e");
            var body = SplitProperty(param, searchModel.FieldName);

            body = Expression.Call(body, "ToLowerInvariant", Type.EmptyTypes);
            body = Expression.Call(typeof(DbFunctionsExtensions), "Like", Type.EmptyTypes,
                Expression.Constant(EF.Functions), body, Expression.Constant($"%{searchModel.LongestWord}%".ToLower()));

            var lambda = Expression.Lambda(body, param);
            var queryExpr = Expression.Call(typeof(Queryable), "Where", new[] { typeof(T) }, source.Expression, lambda);

            return source.Provider.CreateQuery<T>(queryExpr);
        }

        /// <summary>
        /// Get property by its path
        /// </summary>
        /// <param name="param"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static Expression SplitProperty(ParameterExpression param, string fieldName)
        {
            var emptyString = Expression.Constant("");
            var body = (Expression)param;
            try
            {
                foreach (var propName in fieldName.Split('.'))
                {
                    body = Expression.PropertyOrField(body, propName);
                }
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid field name \"{fieldName}\"");
            }

            return Expression.Coalesce(body, emptyString);
        }

        /// <summary>
        /// Build predicate with "OR" logic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private static ExpressionStarter<T> BuildOrPredicate<T>(IEnumerable<KeywordSearchModel> searchModel)
        {
            var predicate = PredicateBuilder.New<T>(true);

            foreach (var searchItem in searchModel)
            {
                var searchParam = Expression.Constant(searchItem.SearchText.ToLower());
                var param = Expression.Parameter(typeof(T), "e");
                var body = SplitProperty(param, searchItem.FieldName);

                body = Expression.Call(body, "ToLower", Type.EmptyTypes);
                body = Expression.Call(body, "Contains", Type.EmptyTypes, searchParam);

                Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(body, param);
                
                predicate = predicate.Or(lambda);
            }

            return predicate;
        }
        
        private static ExpressionStarter<T> BuildAndPredicate<T>(IEnumerable<KeywordSearchModel> searchModel)
        {
            var predicate = PredicateBuilder.New<T>(true);

            foreach (var searchItem in searchModel)
            {
                var searchParam = Expression.Constant(searchItem.SearchText.ToLowerInvariant());
                var param = Expression.Parameter(typeof(T), "e");
                var body = SplitProperty(param, searchItem.FieldName);

                body = Expression.Call(body, "ToLowerInvariant", Type.EmptyTypes);
                body = Expression.Call(body, "Contains", Type.EmptyTypes, searchParam);

                Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(body, param);
                
                predicate = predicate.And(lambda);
            }

            return predicate;
        }
    }
}