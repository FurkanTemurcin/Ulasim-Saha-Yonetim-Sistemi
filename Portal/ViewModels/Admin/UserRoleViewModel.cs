using System.Collections.Generic;
using Portal.Models;
using Portal.Models.Admin;

namespace Portal.ViewModels.Admin
{
    public class UserRoleViewModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();
        public List<int> AssignedRoles { get; set; } = new List<int>();
        public List<string> RoleNames { get; set; } = new List<string>(); // Rol isimleri için
    }
}
