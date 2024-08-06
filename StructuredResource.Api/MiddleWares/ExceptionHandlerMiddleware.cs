using System.Net;

namespace StructuredResource.Api.MiddleWares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
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
                var errorId = new Guid();

                //log the exeption either to the console or logfile
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                // returning a custom exception here 
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";


                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Ooops! Something went wrong! We are looking into resolving this!"
                };

                await httpContext.Response.WriteAsJsonAsync(error); 
            }
        }
    }
}
