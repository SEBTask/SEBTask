using API.Models;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            this.SetStatusCode(context, exception);

            var result = JsonConvert.SerializeObject(
                new Error
                {
                    Message = this.GetExceptionMessage(exception),
                    TraceIdentifier = context.TraceIdentifier
                }
            );

            return context.Response.WriteAsync(result);
        }

        private void SetStatusCode(HttpContext context, Exception exception)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case EntityNotFoundException enfe:
                    {
                        statusCode = (int)HttpStatusCode.NotFound;
                        break;
                    }

                case IntegrationFailedException ife:
                    {
                        statusCode = (int)HttpStatusCode.ServiceUnavailable;
                        break;
                    }
            }

            context.Response.StatusCode = statusCode;
        }

        private string GetExceptionMessage(Exception exception)
        {
            switch (exception)
            {
                case EntityNotFoundException enfe:
                case IntegrationFailedException ife:
                    {
                        return exception.Message;
                    }

                default:
                    {
                        return "Internal server error";
                    }
            }
        }
    }
}
