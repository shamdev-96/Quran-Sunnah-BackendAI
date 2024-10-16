
namespace Quran_Sunnah_BackendAI.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyName = "X-Api-Key";
        private IConfiguration _configuration;

        public ApiKeyMiddleware(RequestDelegate next , IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {

            if(!context.Request.Path.Value!.Contains("/version") && context.Request.Method != "OPTIONS" )
            {
                // Check if the API key exists in the request headers
                if (!context.Request.Headers.TryGetValue(ApiKeyName, out var apiKeyValue))
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("API Key is missing");
                    return;
                }

                // Validate the API key
                if (!apiKeyValue.Equals(_configuration["API_KEY"]))
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Invalid API Key");
                    return;
                }
            }
   
            // API key is valid, proceed with the request
            await _next(context);
        }
    }

    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
