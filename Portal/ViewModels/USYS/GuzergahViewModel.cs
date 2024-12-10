using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portal.ViewModels.USYS
{
    public class GuzergahViewModel
    {
    public int Id { get; set; }
    public string Ad { get; set; }
    public int IlceId { get; set; }
    public string IlceAdi { get; set; }
    public int GeometriId { get; set; }
    public decimal Ucret { get; set; } // Ücret bilgisi
    public string ZamanCizelgesi { get; set; } // Zaman çizelgesi bilgisi
    public double? Mesafe { get; set; } // Mesafe bilgisi
    public string GunAdi { get; set; } // Çalıştığı gün adı
    public string YerAdi { get; set; } // Kalkış yeri adı
}
}
