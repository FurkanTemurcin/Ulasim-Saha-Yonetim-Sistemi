using Microsoft.AspNetCore.Mvc.Rendering;
using Portal.Models.USYS;
using Portal.ViewModels.USYS;


namespace Portal.ViewModels.USYS
{
    public class RaporViewModel
    {
        public List<CezaSayisi> CezaSayilari { get; set; }
        public List<AracDenetimSayisi> AracDenetimSayilari { get; set; }
        public List<KullaniciDenetimSayisi> KullaniciDenetimSayilari { get; set; }
        public int IhlalTutanakSayisi { get; set; }
        public int IhtarTutanakSayisi { get; set; }
        public int DenetimSayisi { get; set; }
        public int ToplamAracSayisi { get; set; } // Toplam araç sayısını tutacak
        public string TimeRange { get; set; } // Zaman dilimi (tüm zamanlar, yıllık, aylık, haftalık, günlük)
        public string DataType { get; set; } // Denetim türü (ihlal, ihtar, denetim)

        // Grafikte kullanılacak veriler
        public string[] Labels { get; set; } // X ekseni için tarihler
        public int[] Values { get; set; } // Y ekseni için denetim sayıları
        public List<PlakaSahip> PlakaSahipler { get; set; }
        public List<PlakaCezaGenel> PlakaCezaListesi { get; set; }
        // AracPlaka tablosundan gelen veriler
        public int PlakaId { get; set; }
        public string PlakaNumarasi { get; set; }
        public List<SelectListItem> Plakalar { get; set; } = new List<SelectListItem>();
        // Formda seçilen plakayı tutacak alan
        public int SelectedPlakaId { get; set; }

        public int IhlalSayisi { get; set; }  // İhlal sayısı özelliği
        public int IhtarSayisi { get; set; }  // İhtar sayısı özelliği

    }

    public class CezaSayisi
    {
        public string YonetmelikAnahtarKelime { get; set; }
        public int CezaAdedi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }

    public class AracDenetimSayisi
    {
        public string PlakaNumarasi { get; set; }
        public int DenetimAdedi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }

    public class KullaniciDenetimSayisi
    {
        public string KullaniciAdi { get; set; }
        public int DenetimAdedi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }



}


