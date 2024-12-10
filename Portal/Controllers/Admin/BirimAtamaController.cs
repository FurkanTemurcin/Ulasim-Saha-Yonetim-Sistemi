using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal.Models;
using Portal.Models.Admin;
using Portal.ViewModels;
using Portal.ViewModels.Admin;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Controllers.Admin
{
    public class BirimAtamaController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public BirimAtamaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Assign()
        {
            var userRoleIds = await _context.Roles
                .Where(r => r.Name == "Personel" || r.Name == "Admin" || r.Name=="Görevli" || r.Name=="User")
                .Select(r => r.Id)
                .ToListAsync();

            var users = await _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && userRoleIds.Contains(ur.RoleId)))
                .ToListAsync();

            var viewModel = new UserAssignmentViewModel
            {
                Users = users,
                Subeler = await _context.Subeler.ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(UserAssignmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(viewModel.SelectedUserId);
                if (user == null)
                {
                    return NotFound();
                }

                // Kullanıcıya sadece SubeId atanır
                user.SubeId = viewModel.SelectedSubeId;

                _context.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            // Yeniden aynı rol filtresini uygularız
            var userRoleIds = await _context.Roles
                .Where(r => r.Name == "Personel" || r.Name == "Admin" || r.Name == "Görevli" || r.Name=="Gorevli")
                .Select(r => r.Id)
                .ToListAsync();

            viewModel.Users = await _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && userRoleIds.Contains(ur.RoleId)))
                .ToListAsync();

            viewModel.Subeler = await _context.Subeler.ToListAsync();

            return View(viewModel);
        }
    }
}
