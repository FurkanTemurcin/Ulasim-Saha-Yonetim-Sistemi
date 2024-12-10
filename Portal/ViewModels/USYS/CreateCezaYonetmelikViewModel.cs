using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels.USYS
{
    public class CreateCezaYonetmelikViewModel
    {
        [Key]
        public int Id { get; set; }

        // Formdaki diğer alanlar
        [Required]

        public int Madde { get; set; }
        public int? Fikra { get; set; }
        public string? Bent { get; set; }
        [Required]
        public string Yonetmelik { get; set; }
        [Required]

        public int CezaPuani { get; set; }
        public int? PlakaTuruId { get; set; }
        public string? AnahtarKelime { get; set; }

        // Plaka türü için LookupList'ten seçenekler
        public List<SelectListItem>? PlakaTurleri { get; set; }
    }
}
