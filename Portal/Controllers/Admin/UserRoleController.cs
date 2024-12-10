using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Models;
using Portal.Models.Admin;
using Portal.ViewModels;
using Portal.ViewModels.Admin;
using System;
using System.Linq;

namespace Portal.Controllers.Admin
{
    [Route("UserRole")]
    public class UserRoleController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserRoleController> _logger;

        public UserRoleController(ApplicationDbContext context, ILogger<UserRoleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = _context.Roles.ToList();
            var assignedRoles = _context.UserRoles
                .Where(ur => ur.UserId == id)
                .Select(ur => ur.RoleId)
                .ToList();

            var model = new UserRoleViewModel
            {
                UserId = id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = roles,
                AssignedRoles = assignedRoles
            };

            return View(model);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, UserRoleViewModel model)
        {
            if (id != model.UserId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var userRoles = _context.UserRoles
                    .Where(ur => ur.UserId == model.UserId)
                    .ToList();

                _context.UserRoles.RemoveRange(userRoles);

                foreach (var roleId in model.AssignedRoles)
                {
                    _context.UserRoles.Add(new UserRole
                    {
                        UserId = model.UserId,
                        RoleId = roleId
                    });
                }

                try
                {
                    _context.SaveChanges();
                    _logger.LogInformation("User roles updated successfully for UserId: {UserId}", model.UserId);
                    TempData["SuccessMessage"] = "User roles updated successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating user roles for UserId: {UserId}", model.UserId);
                    ModelState.AddModelError(string.Empty, "An error occurred while saving changes.");
                }
            }
            else
            {
                _logger.LogError("ModelState is invalid. Errors: {Errors}", string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            model.Roles = _context.Roles.ToList();
            return View(model);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            var users = _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToList();

            var model = users.Select(u => new UserRoleViewModel
            {
                UserId = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                RoleNames = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            }).ToList();

            return View(model);
        }
    }
}
