using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Portal.Models;
using Portal.Models.Admin;

namespace Portal.ViewModels.Admin
{
    public class UserAssignmentViewModel
    {
    public int UserId { get; set; }

        [Required(ErrorMessage = "Kullanıcı seçilmesi zorunludur.")]
        public int? SelectedUserId { get; set; }

        [Required(ErrorMessage = "Şube seçilmesi zorunludur.")]
        public int? SelectedSubeId { get; set; }

        public List<User> Users { get; set; } = new List<User>();
        public List<Sube> Subeler { get; set; } = new List<Sube>();
    }
}