using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopDomain.Models;

namespace Shop.Api.Filters;
public class LogActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue("user", out var value))
        {
            var user = value as User;
            if (user!=null && user.Login == "admin" && user.Id == 1)
            {
                return;
            }
            context.Result = new JsonResult(new
            {
                message = "No authorization"
            })
            {
                StatusCode = 401
            };
        }
    }
}