using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Portal.Models.USYS;


namespace Portal.Models.Admin
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? About { get; set; } 
        public string? Address { get; set; } 
        public string? Company { get; set; } 
        public string? JobTitle { get; set; } 
        public string? Country { get; set; } 
        [Required]
        [StringLength(11)]

        public string Phone { get; set; } = string.Empty;
        public string? Twitter { get; set; }
        public string? Facebook { get; set; } 
        public string? Instagram { get; set; } 
        public string? Puan { get; set; } 
        public byte[]? ProfileImage { get; set; } 
        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik numarası 11 haneli olmalıdır.")]
        public string TcKimlik { get; set; } = string.Empty;


        [Required]
        public DateTime BirthDate { get; set; } 
        public DateTime? LastQuizDate { get; set; } 
        // Foreign Key for KanGrubus
   
    public int? KanGrubuId { get; set; }
    public KanGrubu KanGrubu { get; set; } // Navigation property


        public string? Cinsiyet { get; set; }
        public string? Ehliyet { get; set; }
        public string? OgrenimDurumu { get; set; }
        public string? MedeniDurum { get; set; }

        // Birim, Müdürlük ve Şube ilişkileri (Nullable Olarak Ayarlandı)
        // public int? BirimId { get; set; }
        // public Birim? Birim { get; set; }

        // public int? MudurlukId { get; set; }
        // public Mudurluk? Mudurluk { get; set; }

        public int? SubeId { get; set; }
        public Sube? Sube { get; set; }


        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Foto>? Fotolar { get; set; } // Kullanıcıya ait fotoğraflar koleksiyonu
        public ICollection<AracDenetim>? AracDenetimleri { get; set; } // Kullanıcıya ait fotoğraflar koleksiyonu
        //    public ICollection<AracDenetimGecmis>? AracDenetimGecmisleri { get; set; } // Kullanıcıya ait denetim geçmişleri

        public ICollection<DenetimGecmisi>? DenetimGecmisleri { get; set; } // Kullanıcıya ait fotoğraflar koleksiyonu
        public ICollection<PlakaSahip>? PlakaSahipleri { get; set; } // Kullanıcıya ait fotoğraflar koleksiyonu

public ICollection<CezaYonetmelik>? CezaYonetmelikleri { get; set; } // Kullanıcıya ait ceza yönetmelikleri koleksiyonu
    public ICollection<Denetim> Denetimler { get; set; } // Kullanıcının oluşturduğu denetimler

                public ICollection<ZamanCizelge>? ZamanCizelgeleri { get; set; } // Kullanıcının oluşturduğu zaman çizelgeleri

                //Apide sifremi unuttum için asagıdaki 2 kolonu ekledik 

                
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiration { get; set; }



       
    }
}
