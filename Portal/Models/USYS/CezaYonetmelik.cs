using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Models.Admin;

namespace Portal.Models.USYS
{
    public class CezaYonetmelik
    {
        [Key]
        public int Id { get; set; } // Benzersiz kimlik numarası (Primary Key)

        // Madde bilgisi (Matter)
        [Required]
        public int Madde { get; set; } = 0;

        // Fıkra bilgisi (Clause)

        public int? Fikra { get; set; }

        // Bent bilgisi (Weir)
        public string? Bent { get; set; }

        // Yönetmelik bilgisi (Regulations)

        public string Yonetmelik { get; set; }

        // Ceza puanı (PenaltyScore)
        public int CezaPuani { get; set; }

        // Plaka Türü (PlateType), LookUpList ile ilişkili olacak.
        [ForeignKey("PlakaTuruId")]
        public int? PlakaTuruId { get; set; }
        public LookupList? PlakaTuru { get; set; } // Plaka Türü ile ilişki

        // Anahtar kelime (KeyWord)

        public string? AnahtarKelime { get; set; }

        // Oluşturan kullanıcı (CreatedBy), User tablosu ile ilişkili olacak.
        [ForeignKey("OlusturanKullaniciId")]
        public int OlusturanKullaniciId { get; set; }
        public User OlusturanKullanici { get; set; } // Kullanıcı ile ilişki

        // Oluşturulma tarihi (CreatedOn)
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        // Güncellenme tarihi (UpdatedOn)
        public DateTime GuncellemeTarihi { get; set; }

        public ICollection<DenetimCeza>? DenetimCezalari { get; set; } // Ceza yönetmeliği ile ilişkili denetim cezaları


    }
}