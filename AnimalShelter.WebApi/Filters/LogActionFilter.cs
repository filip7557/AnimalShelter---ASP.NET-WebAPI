using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class LogActionFilterAttribute : TypeFilterAttribute
{
    public LogActionFilterAttribute() : base(typeof(LogActionFilter))
    {
    }
}

public class LogActionFilter : IActionFilter
{
    private readonly ILogger<LogActionFilter> _logger;

    public LogActionFilter(ILogger<LogActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("Executing action: {ActionName} at {Date}", context.ActionDescriptor.DisplayName, DateTime.Now.ToShortDateString());
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("Executed action: {ActionName} at {Date}", context.ActionDescriptor.DisplayName, DateTime.Now.ToShortDateString());
    }
}
