using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InfinityElectronics.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("X-API-KEY", out var headerKey))
        {
            context.Result = new UnauthorizedObjectResult(new { message = "API key is required" });
            return;
        }
        
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = config.GetValue<string>("ApiKey");
        
        if (!headerKey.Equals(apiKey))
            context.Result = new UnauthorizedObjectResult(new { message = "Invalid API key" });
    }
}