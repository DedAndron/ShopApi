using Microsoft.AspNetCore.Mvc.Filters;

namespace ShopApp.Filters
{
    public class UserActionFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("До виконання метода");
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("Після виконання метода");
        }
    }
}
