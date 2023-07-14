namespace TMM.API.Filters
{
    public class ExceptionHandler : IExceptionFilter
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IStringLocalizer<Resource> _stringLocalizer;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;


        public ExceptionHandler(IWebHostEnvironment environment, ILogger<ExceptionHandler> logger, IStringLocalizer<Resource> stringLocalizer)
        {
            _environment = environment;
            _stringLocalizer = stringLocalizer;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            ProblemDetails problemDetails;

            if (context.Exception is TMMException exception)
            {
                _logger.LogError(TMMEventId.BadRequest, exception, exception.Message);

                var humanizedMessage = HumanizeExceptionMessage(exception.GetType().Name);
                problemDetails = new ProblemDetails
                {
                    Type = humanizedMessage,
                    Detail = GetExceptionMessage(humanizedMessage, exception.Arguments),
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest
                };
            }
            else
            {
                _logger.LogError(TMMEventId.InternalServerError, context.Exception, context.Exception.Message);

                var message = "internal-server-error";
                problemDetails = new ProblemDetails
                {
                    Type = message,
                    Detail = GetExceptionMessage(message),
                    Instance= context.HttpContext.Request.Path,
                    Status = StatusCodes.Status500InternalServerError
                };
            }

            problemDetails.Extensions.Add("request-id", context.HttpContext.TraceIdentifier);

            if (_environment.IsDevelopment())
            {
                AddExceptionDetail(problemDetails.Extensions, context.Exception);
            }

            // Problem Details for HTTP APIs- as a way to carry machine-readable details of errors.
            // https://datatracker.ietf.org/doc/html/rfc7807
            
            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };

            context.ExceptionHandled = true;
        }

        private void AddExceptionDetail(IDictionary<string, object> dictionary, Exception exception)
        {
            dictionary.Add("stackTrace", exception.StackTrace);

            if (exception.InnerException != null)
            {
                Dictionary<string, object> innerData = new Dictionary<string, object>();
                dictionary.Add("inner", innerData);
                AddExceptionDetail(innerData, exception.InnerException);
            }
        }

        private string GetExceptionMessage(string exceptionMessage, params object[] args) =>
            _stringLocalizer.GetString(exceptionMessage, args);


        private string HumanizeExceptionMessage(string message)
        {
            //TODO: It should be improved because of multiple memory allocation.
            message = message.Humanize(LetterCasing.LowerCase);
            return message.Remove(message.LastIndexOf(' ')).Replace(' ', '-');
        }

    }
}
