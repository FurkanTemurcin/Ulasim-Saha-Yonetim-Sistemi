using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Models;
using Portal.Models.Admin;
using Portal.ViewModels;
using Portal.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Portal.Controllers.Admin
{
    [Route("Role")]
    public class RoleController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoleController> _logger;

        public RoleController(ApplicationDbContext context, ILogger<RoleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Role
        // Roles listesi görüntüleme
        [HttpGet]
        public IActionResult Index()
        {
            var roles = _context.Roles.ToList();
            return View(roles);
        }

        // GET: Role/Create
        // Yeni role oluşturma sayfası
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        // Yeni role oluşturma işlemi
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Roles.Add(role);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating role.");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the role.");
                }
            }
            return View(role);
        }

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var role = _context.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }

            var permissions = GetPermissions();
            var groupedPermissions = permissions
                .GroupBy(p => p.Controller)
                .ToDictionary(g => g.Key, g => g.ToList());

            var assignedPermissions = _context.RolePermissions
                .Where(rp => rp.RoleId == id)
                .Select(rp => rp.PermissionId)
                .ToList();

            var model = new EditRoleViewModel
            {
                RoleId = id,
                RoleName = role.Name,
                Permissions = groupedPermissions,
                AssignedPermissions = assignedPermissions
            };

            return View(model);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditRoleViewModel model)
        {
            if (id != model.RoleId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                // Track changes
                var rolePermissions = _context.RolePermissions
                    .Where(rp => rp.RoleId == model.RoleId)
                    .ToList();

                _context.RolePermissions.RemoveRange(rolePermissions);

                // Mevcut menü kayıtlarını silme
                var existingMenus = _context.Menus.Where(m => m.RoleId == model.RoleId).ToList();
                _context.Menus.RemoveRange(existingMenus);

                foreach (var permissionId in model.AssignedPermissions)
                {
                    var permission = _context.Permissions.Find(permissionId);

                    // Eğer zaten izleniyorsa, entity'i yeniden eklemeden önce detach edin
                    var existingRolePermission = _context.RolePermissions
                        .Local
                        .FirstOrDefault(rp => rp.RoleId == model.RoleId && rp.PermissionId == permissionId);

                    if (existingRolePermission != null)
                    {
                        _context.Entry(existingRolePermission).State = EntityState.Detached;
                    }

                    _context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = model.RoleId,
                        PermissionId = permissionId
                    });

                    // Yeni menü kayıtlarını ekleme
                    _context.Menus.Add(new Menu
                    {
                        Name = $"{permission.Controller}/{permission.Action}",
                        Controller = permission.Controller,
                        Action = permission.Action,
                        RoleId = model.RoleId
                    });
                }

                try
                {
                    _context.SaveChanges();
                    _logger.LogInformation("Role permissions updated successfully for RoleId: {RoleId}", model.RoleId);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating role permissions for RoleId: {RoleId}", model.RoleId);
                    ModelState.AddModelError(string.Empty, "An error occurred while saving changes.");
                }
            }
            else
            {
                _logger.LogError("ModelState is invalid. Errors: {Errors}", string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            var permissions = GetPermissions();
            var groupedPermissions = permissions
                .GroupBy(p => p.Controller)
                .ToDictionary(g => g.Key, g => g.ToList());

            model.Permissions = groupedPermissions;

            return View(model);
        }


        private List<Permission> GetPermissions()
        {
            var permissions = new List<Permission>();

            // Mevcut assembly'deki controller'ları keşfet
            var controllers = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type) && !type.IsAbstract);

            foreach (var controller in controllers)
            {
                var controllerName = controller.Name.Replace("Controller", "");

                // Her controller içindeki public action metotlarını keşfet
                var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    .Where(m => !m.IsDefined(typeof(NonActionAttribute)));

                foreach (var action in actions)
                {
                    var permission = _context.Permissions
                        .FirstOrDefault(p => p.Controller == controllerName && p.Action == action.Name);

                    if (permission == null)
                    {
                        permission = new Permission
                        {
                            Controller = controllerName,
                            Action = action.Name
                        };
                        _context.Permissions.Add(permission);
                        _context.SaveChanges();
                    }

                    permissions.Add(permission);
                }
            }

            return permissions;
        }


    
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            // Silinecek role'u bul
            var role = _context.Roles.FirstOrDefault(r => r.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            var rolePermissions = _context.RolePermissions.Where(rp => rp.RoleId == id).ToList();
            if (rolePermissions.Any())
            {
                _context.RolePermissions.RemoveRange(rolePermissions);
            }

            _context.Roles.Remove(role);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
