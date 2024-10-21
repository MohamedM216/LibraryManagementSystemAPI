using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibraryManagementSystemAPI.Filters;

public class IdValidationFilterAttribute : ActionFilterAttribute
{

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var parameter = context.ActionArguments["id"] as int?;

        if (!parameter.HasValue || parameter <= 0) {
            context.Result = new BadRequestObjectResult("Invalid parameter. 'id' must be a positive integer.");
        }
    }
}