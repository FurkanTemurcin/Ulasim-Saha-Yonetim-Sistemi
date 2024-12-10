using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Models.Admin;

namespace Portal.Models.USYS
{
    public class PlakaSahip
    {
        [Key]
        public int Id { get; set; } // Benzersiz kimlik numarası

        public int? KayitNumarasi { get; set; } // Kayıt numarası
        public string? KimlikNumarasi { get; set; } // Kimlik numarası
        public string? VergiNumarasi { get; set; } // Vergi numarası
        public string? Ad { get; set; } // İsim
        public string? Soyad { get; set; } // Soyisim
        public string? Unvan { get; set; } // Unvan
        public string? Adres { get; set; } // Adres

        public int? CinsiyetId { get; set; } // Foreign Key
        [ForeignKey("CinsiyetId")] // ForeignKey özniteliği CinsiyetId ile ilişkilendirildi

        public LookupList? Cinsiyet { get; set; } // Cinsiyet navigasyon özelliği

        public string? Telefon { get; set; } // Telefon
        public bool AktifMi { get; set; } // Aktif mi

        [ForeignKey("OlusturanKullaniciId")] // ForeignKey özniteliği OlusturanKullaniciId ile ilişkilendirildi

        public int? OlusturanKullaniciId { get; set; } // Foreign Key
        public User? OlusturanKullanici { get; set; } // Kullanıcı navigasyon özelliği

        public DateTime OlusturulmaTarihi { get; set; } // Oluşturulma tarihi

        // Diğer ilişkili modeller
        public ICollection<AracSahibi>? AracSahipleri { get; set; }

    }

}
