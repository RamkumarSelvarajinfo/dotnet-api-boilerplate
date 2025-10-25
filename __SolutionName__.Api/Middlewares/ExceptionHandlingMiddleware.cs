using __SolutionName__.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace __SolutionName__.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException ex)
            {
                await HandleBusinessExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleGenericExceptionAsync(context, ex);
            }
        }

        private static Task HandleBusinessExceptionAsync(HttpContext context, BusinessException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ex.HttpStatusCode;

            var response = new
            {
                StatusCode = ex.HttpStatusCode,
                Errors = ex.message.errorMsgList,
                InfoMessage = ex.message.infoMessge
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An unexpected error occurred. Please try again later."
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
