using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portal.Models.USYS;

namespace Portal.ViewModels.USYS
{
    public class CreateCezaViewModel

    {
        // AracPlaka tablosundan gelen veriler
        public int PlakaId { get; set; }
        public string PlakaNumarasi { get; set; }
        public List<SelectListItem> Plakalar { get; set; } = new List<SelectListItem>();
        // Formda seçilen plakayı tutacak alan
        public int SelectedPlakaId { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now.Date; // Varsayılan olarak bugünün tarihi
        public TimeSpan Saat { get; set; } = DateTime.Now.TimeOfDay; // Varsayılan olarak bugünün saati

        // Anahtar Kelime Seçimi için yeni alan
        // Anahtar Kelime Seçimi için yeni alan (çoklu seçim için)
        // public List<string> SelectedAnahtarKelimeler { get; set; }
        // public List<SelectListItem> AnahtarKelimeler { get; set; }

        // Anahtar kelimeler SelectList olarak gönderilecek
        public List<SelectListItem> AnahtarKelimeler { get; set; }

        // Kullanıcı tarafından seçilen anahtar kelimeler string olarak saklanacak
        public List<string> SelectedAnahtarKelimeler { get; set; }

        // Cezanın kesildiği yer alanı
        public string KesildigiYer { get; set; }

        // Tutanak Türü alanı
        public List<SelectListItem> TutanakTurleri { get; set; }
        public int SelectedTutanakTuruId { get; set; }

        // Bitiş günü alanı (nullable DateTime)
        public int? BitisGunu { get; set; }


        // // Delil Türleri alanı (birden fazla seçim için)
        // public List<SelectListItem>? DelilTurleri { get; set; }
        // public List<string>? SelectedDelilTurleri { get; set; } // Birden fazla seçilen delil türleri
        // Delil Türleri seçenekleri için

        // Delil Türleri alanı (birden fazla seçim için)
        public List<SelectListItem>? DelilTurleri { get; set; }
        public List<int>? SelectedDelilTurleri { get; set; } // Birden fazla seçilen delil türleri için List<int> kullanın
                                                             // Fotoğraf ve video alanları (birden fazla dosya yükleme destekli)
        public List<IFormFile> FotoDosyalar { get; set; }
        public List<IFormFile> VideoDosyalar { get; set; }
        public int? DenetimId { get; set; }  // Denetim ID'sini formda tutacağız

        public string? Coordinates { get; set; } // Plaka numarası bilgisi
        public string?  LocationName { get; set; }

        

    public string Plaka { get; set; }
    public List<string> KontrolTurleri { get; set; } // Birden fazla kontrol türü için liste










    }


}