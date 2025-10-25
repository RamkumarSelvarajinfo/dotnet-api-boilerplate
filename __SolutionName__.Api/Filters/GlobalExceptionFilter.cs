using __SolutionName__.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace __SolutionName__.Api.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BusinessException ex)
            {
                var response = new
                {
                    StatusCode = ex.HttpStatusCode,
                    Errors = ex.message.errorMsgList,
                    InfoMessage = ex.message.infoMessge
                };

                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)ex.HttpStatusCode
                };
            }
            else
            {
                var response = new
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                };

                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            context.ExceptionHandled = true;
        }
    }
}
