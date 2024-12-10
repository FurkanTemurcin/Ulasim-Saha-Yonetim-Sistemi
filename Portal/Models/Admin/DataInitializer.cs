using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models.Admin
{
    public class DataInitializer
    {

    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Roles.Any(r => r.Name == "Admin"))
        {
            var adminRole = new Role { Name = "Admin" };
            context.Roles.Add(adminRole);
            context.SaveChanges();
        }

        if (!context.Roles.Any(r => r.Name == "User"))
        {
            var userRole = new Role { Name = "User" };
            context.Roles.Add(userRole);
            context.SaveChanges();
        }

        var controllers = new[] { "Home", "Usys", "Genckart", "Role", "UserRole" };
        var actions = new[] { "Index", "Edit", "Create", "Delete", "Details" };

        foreach (var controller in controllers)
        {
            foreach (var action in actions)
            {
                if (!context.Permissions.Any(p => p.Controller == controller && p.Action == action))
                {
                    context.Permissions.Add(new Permission
                    {
                        Controller = controller,
                        Action = action
                    });
                }

                if (!context.Menus.Any(m => m.Controller == controller && m.Action == action))
                {
                    context.Menus.Add(new Menu
                    {
                        Name = $"{controller} {action}",
                        Controller = controller,
                        Action = action
                    });
                }
            }
        }

        context.SaveChanges();

        var adminRoleId = context.Roles.Single(r => r.Name == "Admin").Id;
        var allPermissions = context.Permissions.ToList();

        foreach (var permission in allPermissions)
        {
            if (!context.RolePermissions.Any(rp => rp.RoleId == adminRoleId && rp.PermissionId == permission.Id))
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = adminRoleId,
                    PermissionId = permission.Id
                });
            }
        }

        context.SaveChanges();
    }
}

    
}