
namespace StudentSystem.Middleware {
    /// <summary>
    /// Simple Api Key Middleware.
    /// Reference from https://itsjoshcampos.codes/net-web-api-api-key-authorization
    /// </summary>
    public class ApiKeyMiddleware {
        private readonly RequestDelegate _next;
        
        private const string APIKEY = "x-api-key";

        public ApiKeyMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            //Check if we have an API Key specified in the context headers.
            if(!context.Request.Headers.TryGetValue(APIKEY, out var suppliedApiKey)) {

                await SendUnauthorisedReponse(context, "No api key was provided.");
                return;
            }

            IConfiguration appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            string? apiKey = appSettings.GetValue<string>("ApiKey");
            if (apiKey != null && !apiKey.Equals(suppliedApiKey)) {
                await SendUnauthorisedReponse(context, "Unauthorised. Invalid API Key.");
                return;
            }
            await _next(context);
        }

        /// <summary>
        /// Utility function to return a 401 unauthorised message to an HttpContext.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        private async Task SendUnauthorisedReponse(HttpContext context, string message) {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync(message);
        }
    }

}
