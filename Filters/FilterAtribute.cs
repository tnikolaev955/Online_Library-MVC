using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Online_Library.Models;
using System.Text.Json;

namespace Online_Library.Filters
{
    public class FilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.HttpContext.Session.GetString("loggedUser") == null)
            {
                context.Result = new RedirectResult("/Home/Login");
            }
        }
    }

    public class RoleFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            string value = context.HttpContext.Session.GetString("loggedUser");
            if (!string.IsNullOrEmpty(value))
            {
                User user = JsonSerializer.Deserialize<User>(value);
                if (user.Role != "Библиотекар")
                {
                    context.Result = new RedirectResult("/Home/Login");
                }
            }
        }
    }
}
