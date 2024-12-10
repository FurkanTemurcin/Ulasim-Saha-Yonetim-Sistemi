using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels.Admin
{
    public class AddUserViewModel
    {
        [Required(ErrorMessage = "Lütfen tam ad soyad giriniz.")]
        [Display(Name = "Tam Ad")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Lütfen e-posta adresi giriniz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lütfen şifre giriniz.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Lütfen şifreyi tekrar giriniz.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifreyi Onayla")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }

        public string? About { get; set; }
        public string? Address { get; set; }
        public string? Company { get; set; }
        public string? JobTitle { get; set; }
        public string? Country { get; set; }

        [Required(ErrorMessage = "Lütfen telefon numarası giriniz.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        public string? Twitter { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Puan { get; set; }
         public string? MedeniDurum { get; set; }
          public string? Cinsiyet { get; set; }
           public string? OgrenimDurumu { get; set; }

        public byte[]? ProfileImage { get; set; }

        [Required(ErrorMessage = "Lütfen TC Kimlik numarası giriniz.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik numarası 11 haneli olmalıdır.")]
        [Display(Name = "TC Kimlik No")]
        public string TcKimlik { get; set; }

        [Required(ErrorMessage = "Lütfen doğum tarihi giriniz.")]
        [Display(Name = "Doğum Tarihi")]
        public DateTime BirthDate { get; set; }
            public int? KanGrubuId { get; set; } // Dropdown'dan seçilecek Kan Grubu Id'si

    }
}
