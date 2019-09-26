using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using OpenWebTech.Infrastructure.ActionResults;
using OpenWebTech.Infrastructure.Exceptions;
using System.Net;

namespace OpenWebTech.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment env;
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        public HttpGlobalExceptionFilter(IHostingEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            if (context.Exception.GetType() == typeof(BusinnessDomainException))
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "Please refer to the errors property for additional details."
                };

                problemDetails.Errors.Add("DomainValidations", new string[] { context.Exception.Message.ToString() });

                context.Result = new InternalServerErrorResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            else if (context.Exception.GetType() == typeof(NotFoundBusinessEntityException))
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status404NotFound,
                    Detail = "Please refer to the errors property for additional details."
                };

                problemDetails.Errors.Add("DomainValidations", new string[] { context.Exception.Message.ToString() });

                context.Result = new NotFoundObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                var json = new JsonErrorResponse
                {
                    Contacts = new[] { "An error ocurred." }
                };

                if (env.IsDevelopment())
                {
                    json.DeveloperContact = context.Exception;
                }

                context.Result = new InternalServerErrorResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
        }

        private class JsonErrorResponse
        {
            public string[] Contacts { get; set; }

            public object DeveloperContact { get; set; }
        }
    }
}
