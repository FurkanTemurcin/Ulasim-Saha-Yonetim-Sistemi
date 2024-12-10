using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Portal.Models;
using System.Linq;
using Portal.Models.Admin;

namespace Portal.Filters
{
    public class PermissionAuthorizationFilter : IAuthorizationFilter
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionAuthorizationFilter(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!IsAuthorized(context))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }
        }

        public bool IsAuthorized(AuthorizationFilterContext context)
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return false;
            }

            var controller = context.RouteData.Values["controller"].ToString();
            var action = context.RouteData.Values["action"].ToString();

            var userRoles = _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToList();

            var hasPermission = _context.RolePermissions
                .Any(rp => userRoles.Select(ur => ur.Id).Contains(rp.RoleId) && rp.Permission.Controller == controller && rp.Permission.Action == action);

            return hasPermission;
        }
    }
}
