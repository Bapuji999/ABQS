namespace ABQStask.Helpers
{
    public class BearerTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public BearerTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string token = context.Session.GetString("Token");

            if (!string.IsNullOrEmpty(token))
                context.Request.Headers.Add("Authorization", token);

            await _next(context);
        }
    }
}
