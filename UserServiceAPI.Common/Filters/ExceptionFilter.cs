using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data.Common;
using System.Net;
using UserServiceAPI.Common.Models;

namespace UserServiceAPI.Common.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
        
            Error error = new()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = context.Exception.Message
            };
    
            if (context.Exception is KeyNotFoundException)
            {
                error.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else if (context.Exception is DbException || context.Exception is DBErrorException)
            {
                error.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            context.Result = new JsonResult(error) { StatusCode = error.StatusCode };
        }
    }
}
