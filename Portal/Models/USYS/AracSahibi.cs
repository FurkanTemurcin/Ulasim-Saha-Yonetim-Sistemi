using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Models.USYS
{
    public class AracSahibi
    {
        [Key]
        public int Id { get; set; } // Benzersiz kimlik numarası

        [Required]
        public int AracPlakaId { get; set; } // Plaka ile ilişki (Foreign Key)

        [ForeignKey("AracPlakaId")] // AracPlakaId, AracPlaka ile ilişkilendirilecek FK'dir.
        public AracPlaka AracPlaka { get; set; } // Plaka navigasyon özelliği

        [Required]
        public int PlakaSahipId { get; set; } // Araç Sahibi ile ilişki (Foreign Key)

        [ForeignKey("PlakaSahipId")] // PlakaSahipId, PlakaSahip ile ilişkilendirilecek FK'dir.
        public PlakaSahip PlakaSahip { get; set; } // Araç sahibi navigasyon özelliği
    }
}

