using FluentValidation;
using System.Text.Json;

namespace __SolutionName__.Api.Filters
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            if (context.Request.ContentLength > 0 && context.Request.Method != HttpMethods.Get)
            {
                var endpoint = context.GetEndpoint();
                var validationAttribute = endpoint?.Metadata.GetMetadata<ValidationModelAttribute>();

                if (validationAttribute != null)
                {
                    context.Request.EnableBuffering();
                    var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                    context.Request.Body.Position = 0;

                    var modelType = validationAttribute.ModelType;
                    var model = JsonSerializer.Deserialize(body, modelType, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var validatorType = typeof(IValidator<>).MakeGenericType(modelType);
                    var validator = serviceProvider.GetService(validatorType) as IValidator;

                    if (validator != null)
                    {
                        var validationResult = await validator.ValidateAsync(new ValidationContext<object>(model));

                        if (!validationResult.IsValid)
                        {
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            context.Response.ContentType = "application/json";
                            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

                            var response = new
                            {
                                StatusCode = StatusCodes.Status400BadRequest,
                                Errors = errors
                            };

                            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
