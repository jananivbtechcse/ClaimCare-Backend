using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using ClaimCare.Responses;
using ClaimCare.Exceptions;

namespace ClaimCare.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception,
                "An error occurred while processing the request.");

            //creating a error response object 
            var errorResponse = new ErrorResponse
            {
                Message = exception.Message
            };

            switch (exception)
            {
                case NotFoundException:
                    errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Title = "Resource Not Found";
                    break;

                case BadRequestException:
                    errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Title = "Bad Request";
                    break;

                case UnauthorizedAccessException:
                    errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Title = "Unauthorized";
                    break;

                default:
                    errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Title = "Internal Server Error";
                    errorResponse.Message = "An unexpected error occurred.";
                    break;
            }

            httpContext.Response.StatusCode = errorResponse.StatusCode;

            // we are writing the repsone in json format here 
            await httpContext.Response.WriteAsJsonAsync(
                errorResponse,
                cancellationToken);

            return true; // it is beacuse the exception handled successfully 
        }
    }
}
//example outcome 
// {
//   "statusCode": 404,
//   "title": "Resource Not Found",
//   "message": "Claim not found",
//   "timestamp": "..."
// }