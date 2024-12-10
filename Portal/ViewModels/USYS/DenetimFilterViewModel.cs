using Portal.Models.USYS;

namespace Portal.ViewModels.USYS
{
    public class DenetimFilterViewModel
    {
        public string? Plaka { get; set; } // Plaka filtreleme
    public string? KontrolEdenMemur { get; set; } // Kullanıcı filtreleme
    public DateTime? BaslangicTarihi { get; set; } // Başlangıç tarihi filtreleme
    public DateTime? BitisTarihi { get; set; } // Bitiş tarihi filtreleme
    public List<Denetim>? Denetimler { get; set; } // Filtrelenmiş denetim sonuçlarını tutacak liste
}
}