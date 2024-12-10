using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Portal.Models;
using Portal.Models.Admin;

namespace Portal.ViewModels.Admin
{
    public class EditRoleViewModel
    {
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; }

        public Dictionary<string, List<Permission>> Permissions { get; set; } = new Dictionary<string, List<Permission>>();
        public List<int> AssignedPermissions { get; set; } = new List<int>();
    }
}
