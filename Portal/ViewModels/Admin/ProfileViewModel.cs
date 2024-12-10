using Microsoft.AspNetCore.Http;
using Portal.Models.Admin;
using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels.Admin
{
    public class ProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tam Adı alanı gereklidir.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email alanı gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçersiz email formatı.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı gereklidir.")]
        public string Password { get; set; }

        public string? About { get; set; }
        public string? Address { get; set; }
        public string? Company { get; set; }
        public string? JobTitle { get; set; }
        public string? Country { get; set; }

        [Required]
        [StringLength(11)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "TC Kimlik numarası zorunludur.")]
        [StringLength(11, ErrorMessage = "TC Kimlik numarası 11 karakter olmalı.")]
        public string TcKimlik { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public string? Twitter { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Puan { get; set; }
        public byte[]? ProfileImage { get; set; }
        public IFormFile? ProfileImageFile { get; set; } // Eklenen dosya

        // Yeni Eklenen Alanlar
        [Display(Name = "Cinsiyet")]
        public string? Cinsiyet { get; set; }

        [Display(Name = "Öğrenim Durumu")]
        public string? OgrenimDurumu { get; set; }

        [Display(Name = "Medeni Durum")]
        public string? MedeniDurum { get; set; }

        [Display(Name = "Ehliyet")]
        public string? Ehliyet { get; set; }

        [Display(Name = "Birim")]
        public int? BirimId { get; set; }

        [Display(Name = "Birim Adı")]
        public string? BirimAd { get; set; }

        [Display(Name = "Müdürlük")]
        public int? MudurlukId { get; set; }

        [Display(Name = "Müdürlük Adı")]
        public string? MudurlukAd { get; set; }

        [Display(Name = "Şube")]
        public int? SubeId { get; set; }

        [Display(Name = "Şube Adı")]
        public string? SubeAd { get; set; }    
        public string? KanGrubu { get; set; } // Kan grubu adı burada string olacak



        
    }
}
