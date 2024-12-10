namespace Portal.ViewModels.USYS
{
    public class CreateZamanCizelgeViewModel
    {

    public int GuzergahId { get; set; }
    public int YerTuruId { get; set; }
    public int? GunId { get; set; }
    public TimeSpan? BaslangicSaati { get; set; }
    public TimeSpan? BitisSaati { get; set; }
    public double? Mesafe { get; set; }
    public int OlusturanKullaniciId { get; set; }
    public string? YerTuruName { get; set; }
    public string? GunName { get; set; }

    }
}