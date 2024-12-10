using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models.Admin
{
    public class Permission
    {
        // [Key]
        public int Id { get; set; }

        // [Required]
        public string Controller { get; set; }

        // [Required]
        public string Action { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
