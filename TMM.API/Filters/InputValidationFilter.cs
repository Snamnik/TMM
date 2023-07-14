namespace TMM.API.Filters
{
    public class InputValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                        .SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage)
                        .ToList();

                var problemDetails = new ProblemDetails
                {
                    Type = "input-validation-failed",
                    Detail = string.Join(' ', errors),
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status422UnprocessableEntity,
                };

                problemDetails.Extensions.Add("request-id", context.HttpContext.TraceIdentifier);


                context.Result = new JsonResult(problemDetails)
                {
                    StatusCode = 422
                };
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
