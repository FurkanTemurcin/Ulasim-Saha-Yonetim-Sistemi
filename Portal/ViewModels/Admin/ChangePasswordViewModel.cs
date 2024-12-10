using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels.Admin
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mevcut şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre zorunludur.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Yeni şifre en az 6 karakter olmalıdır.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Şifre doğrulama zorunludur.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Yeni şifre ve doğrulama şifresi eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}
