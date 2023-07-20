using System.Net;
using System.Text.Json;
using FluentValidation;

namespace Chair.Middllewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                isSuccess = false,
                errors = new List<object>()
            };

            // Handle validation exceptions (if any)
            if (exception is ValidationException validationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                foreach (var error in validationException.Errors)
                {
                    response.errors.Add(new
                    {
                        propertyName = error.PropertyName,
                        message = error.ErrorMessage
                    });
                }

                // Customize the response to only return validation error messages
                var result = JsonSerializer.Serialize(response.errors);
                return context.Response.WriteAsync(result);
            }
            else // Handle other exceptions (including generic Exception)
            {
                // Log the exception (you can add your logging logic here)
                // ...

                // Customize the response message based on the exception
                response.errors.Add(new
                {
                    message = exception.Message
                });

                var result = JsonSerializer.Serialize(response.errors);
                return context.Response.WriteAsync(result);
            }
        }
    }
}
