using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels.Admin
{
    public class RegisterViewModel
    {
      [Required(ErrorMessage = "Ad soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Şifre en az {2} karakter olmalı.", MinimumLength = 6)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        [Display(Name = "Şifre Tekrar")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "TC Kimlik numarası zorunludur.")]
        [StringLength(11, ErrorMessage = "TC Kimlik numarası 11 karakter olmalı.")]
        [Display(Name = "TC Kimlik")]
        public string TcKimlik { get; set; }

        [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public string Puan = "0";
        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [StringLength(15)]

         [Display(Name = "Telefon")]
        
        public string Phone { get; set; }
    [Required(ErrorMessage = "Cinsiyet zorunludur.")]


    [Display(Name = "Cinsiyet")]
    public string? Cinsiyet { get; set; }

     [Required(ErrorMessage = "Öğrenmi Durumu zorunludur.")]

    [Display(Name = "Öğrenim Durumu")]
    public string? OgrenimDurumu { get; set; }
    }
}
