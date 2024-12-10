using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portal.Models.USYS;


    namespace Portal.ViewModels.USYS
{
    public class AracDenetimViewModel
    {

        // Plaka seçimi için dropdown
        [Required(ErrorMessage = "Lütfen bir plaka seçin.")]
        public int AracPlakaId { get; set; } // Foreign Key - Araç plakası

        // Kontrol türü için birden fazla seçime uygun hale getirildi
        [Required(ErrorMessage = "Lütfen en az bir kontrol türü seçin.")]
        public List<int> SelectedKontrolTuruIds { get; set; } = new List<int>(); // Çoklu seçim

        // Oluşturulma tarihi
        // public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        // Dropdown seçenekleri için plaka listesi
        public List<SelectListItem> Plakalar { get; set; } = new List<SelectListItem>();

        // Dropdown seçenekleri için kontrol türü listesi
        public List<SelectListItem> KontrolTurleri { get; set; } = new List<SelectListItem>();

            public bool? CezaDurum { get; set; } // bit (true/false) olarak tutulacak


    }
}



