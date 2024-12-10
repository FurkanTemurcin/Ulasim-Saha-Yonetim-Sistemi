using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Models.USYS
{
    public class LookupList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int LookUpId { get; set; }
        public LookUp LookUp { get; set; }  // LookUp ile ilişkilidir.
        [Required]
        public string Name { get; set; }
        public string? Icon { get; set; }
        public bool? IsActive { get; set; }
        public string? Attributes { get; set; }

        public ICollection<Foto>? Fotolar { get; set; } // Foto ile ilişki
        public ICollection<CezaYonetmelik>? CezaYonetmelikleri { get; set; } // Ceza yönetmelikleri ile ilişki

        public ICollection<AracPlaka> AracPlakalari { get; set; } // Plaka türü ile ilişkili araçlar


        // Farklı navigasyon özellikleri tanımlayın
        [InverseProperty("Ilce")]
        public ICollection<Guzergah>? IlceGuzergahlar { get; set; } // İlçe bilgisi olarak Guzergah ile ilişki

        [InverseProperty("Geometri")]
        public ICollection<Guzergah>? GeometriGuzergahlar { get; set; } // Geometri bilgisi olarak Guzergah ile ilişki

        public ICollection<Denetim> Denetimler { get; set; }

    }
}