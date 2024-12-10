using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portal.ViewModels.USYS
{
    public class EditCezaYonetmelikViewModel
    {
          public int Id { get; set; } // Ceza Yönetmelik ID

        [Required(ErrorMessage = "Madde alanı zorunludur.")]
        public int Madde { get; set; }

        public int? Fikra { get; set; }
        public string Bent { get; set; }

        [Required(ErrorMessage = "Yönetmelik alanı zorunludur.")]
        public string Yonetmelik { get; set; }

        [Required(ErrorMessage = "Ceza puanı zorunludur.")]
        public int CezaPuani { get; set; }

        // Seçilen Plaka Türü ID
        [Required(ErrorMessage = "Plaka Türü alanı zorunludur.")]
        public int? SelectedPlakaTuruId { get; set; }

        // Anahtar kelime alanı
        public string AnahtarKelime { get; set; }

        // Dropdown için Plaka Türleri listesi
        public IEnumerable<SelectListItem>? PlakaTurleri { get; set; }
    }
}