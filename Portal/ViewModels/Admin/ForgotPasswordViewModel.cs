using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.ViewModels.Admin
{
     public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "TC Kimlik numarası gereklidir.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik numarası 11 haneli olmalıdır.")]
        public string TcKimlik { get; set; }

        [Required(ErrorMessage = "Yeni şifre gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Şifre tekrar gereklidir.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Şifre ve Şifre Tekrar alanları eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}