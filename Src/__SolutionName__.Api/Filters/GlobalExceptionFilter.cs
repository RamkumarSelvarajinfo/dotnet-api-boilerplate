using __SolutionName__.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace __SolutionName__.Api.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionFilter(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BusinessException ex)
            {
                var response = new
                {
                    StatusCode = ex.HttpStatusCode,
                    Errors = ex.message?.errorMsgList,
                    InfoMessage = ex.message?.infoMessge
                };

                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)ex.HttpStatusCode
                };
            }
            else
            {
                // For non-Business exceptions (500s):
                object response;

                if (_env.IsDevelopment())
                {
                    // Show detailed error info in Development
                    response = new
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Message = context.Exception.Message,
                        ExceptionType = context.Exception.GetType().Name,
                        StackTrace = context.Exception.StackTrace,
                        InnerException = context.Exception.InnerException?.Message
                    };
                }
                else
                {
                    // Hide details in Production
                    response = new
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Message = "An unexpected error occurred. Please try again later."
                    };
                }

                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            context.ExceptionHandled = true;
        }
    }
}