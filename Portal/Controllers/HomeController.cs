using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Portal.Models;
using System.Linq;
using Portal.ViewModels;
using Portal.Controllers.Admin;
using Portal.Models.Admin;


namespace Portal.Controllers
{
    [Route("[controller]")]
    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userRoles = _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToList();

            var hasAccess = _context.RolePermissions
                .Any(rp => userRoles.Select(ur => ur.Id).Contains(rp.RoleId) && rp.Permission.Controller == "Home" && rp.Permission.Action == "Index");

            if (!hasAccess)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var permissions = _context.RolePermissions
                .Where(rp => userRoles.Select(ur => ur.Id).Contains(rp.RoleId))
                .Select(rp => rp.Permission)
                .ToList();

            var viewModel = new HomeIndexViewModel
            {
                Permissions = permissions
            };

            return View(viewModel);
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

         
    }
}
