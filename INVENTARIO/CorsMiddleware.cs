namespace INVENTARIO
{
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;

        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");
            
            if (httpContext.Request.Method == "OPTIONS")
            {
                // Handle OPTIONS requests
                httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
                httpContext.Response.StatusCode = StatusCodes.Status200OK;
                return Task.CompletedTask;
            }

            return _next(httpContext);
            
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CorsMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorsMiddleware>();
        }
    }
}
