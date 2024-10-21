namespace LibraryManagementSystemAPI.MiddleWares;

public class ProfilingMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<ProfilingMiddleware> _logger;

    public ProfilingMiddleware(RequestDelegate next, ILogger<ProfilingMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        _logger.LogInformation("Request: {path} with Method: {method} Started at {time}", context.Request.Path, context.Request.Method, DateTime.Now);
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {   
            _logger.LogCritical("Exception thrown with source: {name}, path {path}, MESSAGE {message}", ex.Source, context.Request.Path, ex.Message);
        }
        _logger.LogInformation("Request: {path} with Method: {method} Finished at {time}", context.Request.Path, context.Request.Method, DateTime.Now);
    }
}