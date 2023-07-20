﻿using FluentValidation;
using MediatR;

namespace Chair.BLL.Commons.MidiatRPipeline
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators.Select(async validator =>
                {
                    var val = await validator.ValidateAsync(context);
                    return val;
                })
                .SelectMany(result =>
                {
                    var res = result.GetAwaiter().GetResult();
                    return res.Errors;

                })
                .Where(failure => failure != null)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);

            return await next.Invoke(); // next middleware in line
        }
    }
}
