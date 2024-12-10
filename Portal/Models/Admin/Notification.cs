using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Models.Admin
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }  // Bildirimi alan kullanıcı ID'si

        [ForeignKey("UserId")]
        public User User { get; set; }   // User navigasyon özelliği

        [Required]
        public string Message { get; set; }  // Bildirim mesajı

        public bool IsRead { get; set; } = false;  // Bildirimin okunma durumu

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Bildirimin oluşturulma tarihi
    }
}
