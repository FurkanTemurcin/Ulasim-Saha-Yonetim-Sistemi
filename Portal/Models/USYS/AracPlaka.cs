using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Models.USYS
{
    public class AracPlaka
    {
        [Key]
        public int Id { get; set; } // Benzersiz kimlik numarası

        [Required]
        public string? PlakaNumarasi { get; set; } // Plaka numarası bilgisi

        public string? Marka { get; set; } // Marka bilgisi
        public DateTime? VizeTarihi { get; set; } // Vize tarihi
        public int? KoltukKapasitesi { get; set; } // Koltuk kapasitesi
        public string? Model { get; set; } // Model bilgisi
        public string? MotorNumarasi { get; set; } // Motor numarası
        public string? SasiNumarasi { get; set; } // Şasi numarası

        // ForeignKey kullanımı navigation property ile tutarlı olmalıdır.
        public int? PlakaTuruId { get; set; } // Foreign Key (nullable)

        [ForeignKey("PlakaTuruId")] // PlakaTuruId, LookupList ile ilişkilendirilecek FK'dir.
        public LookupList? PlakaTuru { get; set; } // Plaka türü navigasyon özelliği

        public int? GuzergahId { get; set; } // Foreign Key (nullable)

        [ForeignKey("GuzergahId")] // GuzergahId, Guzergah ile ilişkilendirilecek FK'dir.
        public Guzergah Guzergah { get; set; } // Güzergah navigasyon özelliği
        // Diğer ilişkili modeller
        public ICollection<AracDenetim>? AracDenetimleri { get; set; }
        public ICollection<AracSahibi>? AracSahipleri { get; set; }
        public ICollection<AracPlaka> AracPlakalari { get; set; }

    }
}