using System.ComponentModel.DataAnnotations;
using Portal.Models.USYS;

namespace Portal.ViewModels.USYS
{
    public class CreateAracPlakaViewModel
    {
          [Required(ErrorMessage = "Plaka numarası zorunludur.")]
        public string? PlakaNumarasi { get; set; }

        public string? Marka { get; set; }

        [DataType(DataType.Date)]
        public DateTime? VizeTarihi { get; set; }

        public int? KoltukKapasitesi { get; set; }

        public string? Model { get; set; }

        public string? MotorNumarasi { get; set; }

        public string? SasiNumarasi { get; set; }

        [Required(ErrorMessage = "Plaka türü seçilmelidir.")]
        public int? PlakaTuruId { get; set; }

        public int? GuzergahId { get; set; }
    }
}