using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GameWebApi3
{
    public class TooLowLevelExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            string exceptionMessage = context.Exception.Message;

            ContentResult contentResult = new ContentResult();
            contentResult.StatusCode = (int)HttpStatusCode.PreconditionFailed;
            contentResult.Content = exceptionMessage;

            context.Result = contentResult;
            context.ExceptionHandled = true;
        }
    }
}
