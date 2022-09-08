using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ReferenceProject
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private class ErrorResponse
        {
            public ErrorResponse(Exception ex)
            {
                Error = new ErrorDescription
                {
                    Message = ex.Message,
                    InnerException = ex.InnerException?.Message
                };
            }

            public class ErrorDescription
            {
                public string Message { get; set; }
                public string InnerException { get; set; }
            }

            public ErrorDescription Error { get; set; }
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is HttpResponseException exception)
            {
                actionExecutedContext.Response = exception.Response;
                return;
            }

            var responseContent = new ErrorResponse(actionExecutedContext.Exception);
            var status = HttpStatusCode.InternalServerError;
            if (actionExecutedContext.Exception is KeyNotFoundException)
            {
                status = HttpStatusCode.NotFound;
            }

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(status, responseContent);
        }
    }
}