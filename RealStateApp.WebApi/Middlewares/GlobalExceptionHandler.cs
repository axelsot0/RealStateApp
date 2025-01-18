using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Exceptions;
using System.Net;

namespace RealStateApp.WebApi.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            string exceptionTitle = "Internal Server Error";
            string detail = exception.Message;
            switch (exception)
            {
                case ApiException e:
                    switch (e.ErrorCode)
                    {
                        case (int)HttpStatusCode.NotFound:
                            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                            exceptionTitle = "Not Found";
                            break;
                        case (int)HttpStatusCode.BadRequest:
                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            exceptionTitle = "Bad Request";
                            break;
                        default:
                            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                    break;
                case KeyNotFoundException e:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    exceptionTitle = "Not Found";
                    break;
                case ValidationException e:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    exceptionTitle = "Bad Request";
                    detail = ((ValidationException)exception).Errors.Aggregate((a, b) => a + ", " + b);
                    break;
                default:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var problemDetails = new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Title = exceptionTitle,
                Detail = detail
            };

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
