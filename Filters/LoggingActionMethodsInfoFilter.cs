using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibraryManagementSystemAPI.Filters;

public class LoggingActionMethodsInfoFilter : IAsyncActionFilter
{
    private readonly ILogger<LoggingActionMethodsInfoFilter> _logger;

    public LoggingActionMethodsInfoFilter(ILogger<LoggingActionMethodsInfoFilter> logger) {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid) {
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
        _logger.LogInformation("Executing Action: {x}, On Controller: {y}, with Arguments {z}", context.ActionDescriptor.DisplayName, context.Controller, JsonSerializer.Serialize(context.ActionArguments));
        await next();
        _logger.LogInformation("Action: {x} Finished execution On Controller: {y}, with Arguments {z}", context.ActionDescriptor.DisplayName, context.Controller, JsonSerializer.Serialize(context.ActionArguments));
    }
}