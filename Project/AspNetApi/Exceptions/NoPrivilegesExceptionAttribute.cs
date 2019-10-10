using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AspNetApi.Exceptions
{
    public class NoPrivilegesExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            string exceptionMessage = context.Exception.Message;

            ContentResult contentResult = new ContentResult();
            contentResult.StatusCode = (int)HttpStatusCode.Unauthorized;
            contentResult.Content = exceptionMessage;

            context.Result = contentResult;
            context.ExceptionHandled = true;
        }
    }
}
