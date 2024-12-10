using System.ComponentModel.DataAnnotations;
using Portal.Models.USYS;

namespace Portal.ViewModels.USYS
{
    public class CreateGuzergahViewModel
    {
   
        [Required(ErrorMessage = "Güzergah adı zorunludur.")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "İlçe seçilmelidir.")]
        public int IlceId { get; set; } // İlçe seçimi için LookupList verisi

        public int? GeometriId { get; set; } // Geometri bilgisi opsiyonel


    public decimal Ucret { get; set; } // Ücret bilgisi
}
}