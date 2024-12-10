using System.ComponentModel.DataAnnotations;

namespace Portal.Models.USYS
{
    public class PlakaCezaGenel
    {
        [Key] // Benzersiz sütunu birincil anahtar olarak işaretliyoruz
        public long UniqueId { get; set; } // Yeni benzersiz sütun
        public string PlakaNumarasi { get; set; }
        public string AnahtarKelime { get; set; }
        public int DenetimTuruId { get; set; }
        public int PlakaTuruId { get; set; }
        public DateTime OlusturmaTarihi { get; set; }

    }

}
