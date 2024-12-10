using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Models.USYS
{
    public class Guzergah
    {
        [Key]
        public int Id { get; set; } // Benzersiz kimlik numarası

        [Required]
        public string Ad { get; set; } // Güzergah adı (Name)

        [ForeignKey("Ilce")]
        [InverseProperty("IlceGuzergahlar")] // LookupList'te tanımlanan IlceGuzergahlar ile ilişki

        public int? IlceId { get; set; } // District ID
        public LookupList? Ilce { get; set; } // İlçe bilgisi, LookupList'ten gelecek

        // Geometri bilgisi, LookupList tablosu ile ilişkilendirilecek
        [ForeignKey("GeometriId")]
        [InverseProperty("GeometriGuzergahlar")] // LookupList'te tanımlanan GeometriGuzergahlar ile ilişki
        [NotMapped]
        public string? IlceAdi { get; set; } // İlçe adı, veritabanında saklanmaz



        public int? GeometriId { get; set; } // GeometryId
        public LookupList? Geometri { get; set; } // Geometri bilgisi, LookupList'ten gelecek
        public ICollection<ZamanCizelge>? ZamanCizelgeleri { get; set; } // Güzergah ile ilişkili zaman çizelgeleri
        public ICollection<UcretTakvim>? UcretTakvimleri { get; set; } // Navigation Property for UcretTakvim


    }
}