namespace opr_lib
{
    public static class Optimization
    {
        public static Guid GetResult(decimal?[,] matrix, List<Condition> conditions, bool onMax, Dictionary<int, Guid> elements)
        {
            var resultList = new List<int>();

            var result = BinaryOptimization(matrix, conditions);
            var newMatrix = RemoveRows(matrix, result.MinPointsNums.Distinct().ToList());

            var newDictionary = elements
                .Where(kvp => !result.MinPointsNums.Distinct().ToList().Contains(kvp.Key))
                .Select((kvp, index) => new { Index = index, kvp.Value })
                .ToDictionary(x => x.Index, x => x.Value);

            var normalizeMatrix = NormalizeMatrix(newMatrix);
            var adjustedMatrix = AdjustMatrix(normalizeMatrix, onMax, conditions);
            var rowSums = CalculateRowSums(adjustedMatrix);
            var rowProductSums = CalculateProductSums(adjustedMatrix, conditions);
            var rowProducts = CalculateRowProducts(adjustedMatrix);
            var rowProductsWithLabda = CalculateProductsWithLabda(adjustedMatrix, conditions);
            var minMax = GetMinMaxValues(adjustedMatrix, onMax);
            var r = CalculateSqrtExpressionValues(adjustedMatrix, onMax);
            var main = CustomMethod(adjustedMatrix, conditions);

            resultList.Add(onMax
                ? Array.IndexOf(rowSums.ToArray(), rowSums.Max())
                : Array.IndexOf(rowSums.ToArray(), rowSums.Min()));

            resultList.Add(onMax
                ? Array.IndexOf(rowProductSums.ToArray(), rowProductSums.Max())
                : Array.IndexOf(rowProductSums.ToArray(), rowProductSums.Min()));

            resultList.Add(onMax
                ? Array.IndexOf(rowProducts.ToArray(), rowProducts.Max())
                : Array.IndexOf(rowProducts.ToArray(), rowProducts.Min()));

            resultList.Add(onMax
                ? Array.IndexOf(rowProductsWithLabda.ToArray(), rowProductsWithLabda.Max())
                : Array.IndexOf(rowProductsWithLabda.ToArray(), rowProductsWithLabda.Min()));

            resultList.Add(onMax
                ? Array.IndexOf(minMax.ToArray(), minMax.Max())
                : Array.IndexOf(minMax.ToArray(), minMax.Min()));

            resultList.Add(onMax ? Array.IndexOf(r.ToArray(), r.Max()) : Array.IndexOf(r.ToArray(), r.Min()));

            resultList.Add(onMax
                ? main.Item1.MaxBy(x => x.Value).Key
                : main.Item1.MinBy(x => x.Value).Key);

            var res = FindMostCommonNumber(resultList);

            return newDictionary.First(x => x.Key == res).Value;
        }

        static int FindMostCommonNumber(List<int> numbers)
        {
            var groups = numbers.GroupBy(n => n);

            var mostCommonGroup = groups.OrderByDescending(g => g.Count()).First();

            return mostCommonGroup.Key;
        }

        static Result BinaryOptimization(decimal?[,] matrix, List<Condition> conditions)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            decimal?[,] newMatrix = new decimal?[rows, rows];
            decimal?[,] newFailMatrix = new decimal?[rows, rows];
            var maxPointsNums = new List<int>();
            var minPointsNums = new List<int>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (i == j)
                    {
                        newMatrix[i, j] = cols / 2;
                        newMatrix[i, j] = null;
                        newFailMatrix[i, j] = null;
                    }
                    else
                    {
                        decimal passed = 0;
                        decimal failed = 0;

                        for (int k = 0; k < cols; k++)
                        {
                            var value1 = matrix[i, k];
                            var value2 = matrix[j, k];
                            bool condition = conditions[k].Value;

                            if (value1 == value2)
                            {
                                passed += 0.5m;
                                failed += 0.5m;
                            }
                            else if ((condition && value1 > value2) || (!condition && value1 < value2))
                            {
                                passed++;
                            }
                            else
                            {
                                failed++;
                            }
                        }

                        newMatrix[i, j] = passed;
                        newFailMatrix[i, j] = failed;
                        if (failed == 0 || failed == 0.5m)
                            maxPointsNums.Add(i);
                        if (passed == 0 || passed == 0.5m)
                            minPointsNums.Add(i);
                    }
                }
            }

            return new Result { Matrix = newMatrix, FailMatrix = newFailMatrix, MaxPointsNums = maxPointsNums, MinPointsNums = minPointsNums };
        }

        static decimal?[,] RemoveRows(decimal?[,] matrix, List<int> rowsToRemove)
        {
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);

            decimal?[,] newMatrix = new decimal?[numRows - rowsToRemove.Count, numCols];

            int newRow = 0;
            for (int i = 0; i < numRows; i++)
            {
                if (!rowsToRemove.Contains(i + 1))
                {
                    for (int j = 0; j < numCols; j++)
                    {
                        newMatrix[newRow, j] = matrix[i, j];
                    }
                    newRow++;
                }
            }

            return newMatrix;
        }

        static decimal?[,] NormalizeMatrix(decimal?[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            decimal?[,] normalizedMatrix = new decimal?[rows, cols];

            for (int j = 0; j < cols; j++)
            {
                decimal? minVal = matrix[0, j];
                decimal? maxVal = matrix[0, j];
                for (int i = 1; i < rows; i++)
                {
                    if (matrix[i, j] < minVal)
                        minVal = matrix[i, j];
                    if (matrix[i, j] > maxVal)
                        maxVal = matrix[i, j];
                }

                for (int i = 0; i < rows; i++)
                {
                    decimal? normalizedValue;
                    if((maxVal - minVal) == 0)
                        normalizedValue = (matrix[i, j] - minVal) / 1;
                    else
                        normalizedValue = (matrix[i, j] - minVal) / (maxVal - minVal);
                    normalizedMatrix[i, j] = Math.Round(normalizedValue ?? 0m, 2);
                }
            }

            return normalizedMatrix;
        }

        public static decimal?[,] AdjustMatrix(decimal?[,] normalizeMatrix, bool onMax, List<Condition> conditions)
        {
            int numRows = normalizeMatrix.GetLength(0);
            int numCols = normalizeMatrix.GetLength(1);
            decimal?[,] adjustedMatrix = new decimal?[numRows, numCols];

            for (int col = 0; col < numCols; col++)
            {
                var condition = conditions.FirstOrDefault(c => c.Column == col + 1);
                bool useOriginalValue = condition != null && condition.Value == onMax;

                for (int row = 0; row < numRows; row++)
                {
                    decimal? currentValue = normalizeMatrix[row, col];
                    adjustedMatrix[row, col] = useOriginalValue ? currentValue : 1 - currentValue;
                }
            }

            return adjustedMatrix;
        }

        static List<decimal?> CalculateRowSums(decimal?[,] matrix)
        {
            List<decimal?> rowSums = new List<decimal?>();

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                decimal? sum = 0;
                for (int j = 0; j < cols; j++)
                {
                    sum += matrix[i, j];
                }
                rowSums.Add(sum);
            }

            return rowSums;
        }

        static List<decimal?> CalculateProductSums(decimal?[,] matrix, List<Condition> conditions)
        {
            List<decimal?> rowSums = new List<decimal?>();

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                decimal? sum = 0;
                for (int j = 0; j < cols; j++)
                {
                    sum += matrix[i, j] * conditions.First(c => c.Column == j + 1).Lambda;
                }
                rowSums.Add(Math.Round(sum ?? 0, 2));
            }

            return rowSums;
        }

        static List<decimal?> CalculateRowProducts(decimal?[,] matrix)
        {
            List<decimal?> rowSums = new List<decimal?>();

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                decimal? sum = 1;
                for (int j = 0; j < cols; j++)
                {
                    sum *= matrix[i, j] ?? 1;
                }
                rowSums.Add(Math.Round(sum ?? 0, 2));
            }

            return rowSums;
        }

        static List<decimal?> CalculateProductsWithLabda(decimal?[,] matrix, List<Condition> conditions)
        {
            List<decimal?> rowSums = new List<decimal?>();

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                decimal? sum = 1;
                for (int j = 0; j < cols; j++)
                {
                    var lambda = conditions.First(c => c.Column == j + 1).Lambda;
                    var value = matrix[i, j] ?? 1;
                    sum *= value * lambda;
                }
                rowSums.Add(sum);
            }

            return rowSums;
        }

        static List<decimal?> GetMinMaxValues(decimal?[,] matrix, bool onMax)
        {
            List<decimal?> minMaxValues = new List<decimal?>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                IEnumerable<decimal?> rowValues = Enumerable.Range(0, matrix.GetLength(1))
                    .Select(j => matrix[i, j]);

                decimal? value = onMax ? rowValues.Min() : rowValues.Max();
                minMaxValues.Add(value);
            }

            return minMaxValues;
        }

        static List<decimal?> CalculateSqrtExpressionValues(decimal?[,] matrix, bool onMax)
        {
            List<decimal?> result = new List<decimal?>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                decimal expressionValue = 0;

                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (onMax)
                    {
                        expressionValue += ((1 - matrix[i, j]) * (1 - matrix[i, j])) ?? 0;
                    }
                    else
                    {
                        expressionValue += ((0 - matrix[i, j]) * (0 - matrix[i, j])) ?? 0;
                    }
                }

                result.Add((decimal?)Math.Round(Math.Sqrt((double)expressionValue), 2));
            }

            return result;
        }

        static Tuple<Dictionary<int, decimal?>, decimal?> CustomMethod(decimal?[,] adjustedMatrix, List<Condition> conditions)
        {
            decimal? maxLambda = conditions.Max(c => c.Lambda);

            int columnToRemove = conditions.First(c => c.Lambda == maxLambda).Column;
            var newMatrix = RemoveColumn(adjustedMatrix, columnToRemove - 1);

            var maxValues = newMatrix.Cast<decimal?>().Where(val => val < 1).GroupBy(val => val)
                .Select(group => group.Max()).ToList();

            decimal? maxMaxValue = maxValues.Max();

            Dictionary<int, decimal?> resultDictionary = new Dictionary<int, decimal?>();
            for (int i = 0; i < newMatrix.GetLength(0); i++)
            {
                if (newMatrix.GetRow(i).All(val => val <= maxMaxValue))
                {
                    //int originalRow = conditions.First(c => c.Column == columnToRemove).Value ? i + 1 : i + 1;
                    resultDictionary.Add(i + 1, adjustedMatrix[i, columnToRemove - 1]);
                }
            }

            if (resultDictionary.Any()) return Tuple.Create(resultDictionary, maxMaxValue);
            {
                for (int i = 0; i < newMatrix.GetLength(0); i++)
                {
                    if (newMatrix.GetRow(i).All(val => val <= 1))
                        resultDictionary.Add(i + 1, adjustedMatrix[i, columnToRemove - 1]);
                }
            }

            return Tuple.Create(resultDictionary, maxMaxValue);
        }

        static decimal?[,] RemoveColumn(decimal?[,] matrix, int columnIndex)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            decimal?[,] newMatrix = new decimal?[rows, cols - 1];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0, newJ = 0; j < cols; j++)
                {
                    if (j != columnIndex)
                    {
                        newMatrix[i, newJ] = matrix[i, j];
                        newJ++;
                    }
                }
            }

            return newMatrix;
        }

        static void PrintMatrix(decimal?[,] matrix, decimal?[,] failMatrix, List<int> maxPointsNums, List<int> minPointsNums)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                var sum = 0m;
                for (int j = 0; j < cols; j++)
                {
                    sum += matrix[i, j] ?? 0m;
                    if (i == j)
                        Console.Write($"   *:*   |");
                    else
                    {
                        if (($"{matrix[i, j]}:{failMatrix[i, j]}").Length == 3)
                            Console.Write($"   {matrix[i, j]}:{failMatrix[i, j]}   |");
                        else if (($"{matrix[i, j]}:{failMatrix[i, j]}").Length == 7)
                            Console.Write($" {matrix[i, j]}:{failMatrix[i, j]} |");
                    }
                }
                Console.WriteLine($"S={sum}");
                Console.WriteLine($"------------------------------------------------------------------------------");
            }
            Console.WriteLine($"Точки согласия {string.Join(", ", minPointsNums.Distinct())}");
            Console.WriteLine($"Точки максимумa {string.Join(", ", maxPointsNums.Distinct())}");
        }

        static void BasePrintMatrix(decimal?[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if ($"{matrix[i, j]}".Length == 4)
                        Console.Write($" {matrix[i, j]} |");
                    if ($"{matrix[i, j]}".Length == 3)
                        Console.Write($"  {matrix[i, j]} |");
                    else if ($"{matrix[i, j]}".Length == 2)
                        Console.Write($"  {matrix[i, j]}  |");
                    else if ($"{matrix[i, j]}".Length == 1)
                        Console.Write($"   {matrix[i, j]}  |");
                }
                Console.WriteLine();
                Console.WriteLine($"---------------------------");
            }
        }
    }

    public class Condition
    {
        public int Column { get; }
        public bool Value { get; }
        public decimal Lambda { get; }

        public Condition(int column, bool value, decimal lambda)
        {
            Column = column;
            Value = value;
            Lambda = lambda;
        }
    }

    public class Result
    {
        public decimal?[,] Matrix { get; set; }
        public decimal?[,] FailMatrix { get; set; }
        public List<int> MaxPointsNums { get; set; } = new();
        public List<int> MinPointsNums { get; set; } = new();
    }

    public static class Extensions
    {
        public static IEnumerable<T> GetRow<T>(this T[,] matrix, int row)
        {
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                yield return matrix[row, i];
            }
        }
    }

}