using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Portal.Models;
using System.Linq;
using Portal.Models.Admin;

namespace Portal.Controllers.Admin
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MenuController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var roles = _context.UserRoles
                                .Where(ur => ur.UserId == userId)
                                .Select(ur => ur.RoleId)
                                .ToList();

            var permissions = _context.RolePermissions
                                      .Where(rp => roles.Contains(rp.RoleId))
                                      .Select(rp => rp.Permission)
                                      .ToList();

            var menus = _context.Menus
                                .Where(m => permissions.Any(p => p.Controller == m.Controller && p.Action == m.Action))
                                .ToList();

            return View(menus);
        }
    }
}
