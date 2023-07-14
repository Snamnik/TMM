namespace TMM.API.Filters
{
    public class LoggingFilter : IActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggingFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var userId = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            LogContext.PushProperty("UserId", userId);
            LogContext.PushProperty("RequestId", httpContext.TraceIdentifier);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Headers.Add("Request-Id", _httpContextAccessor.HttpContext.TraceIdentifier);
        }
    }
}
