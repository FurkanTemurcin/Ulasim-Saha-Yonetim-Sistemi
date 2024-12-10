using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Portal.Filters;

namespace Portal.Controllers.Admin
{
    public class BaseController : Controller
    {
        private PermissionAuthorizationFilter _permissionFilter;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _permissionFilter = HttpContext.RequestServices.GetRequiredService<PermissionAuthorizationFilter>();
            var authContext = new AuthorizationFilterContext(context, context.Filters);
            if (!_permissionFilter.IsAuthorized(authContext))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }
            base.OnActionExecuting(context);
        }


    }

}
