using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models.Admin
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        // Koleksiyonları zorunlu olmaktan çıkaralım.
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
