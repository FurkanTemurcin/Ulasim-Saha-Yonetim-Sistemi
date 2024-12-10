using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Models.Admin;

namespace Portal.Models.USYS
{
    public class Denetim
    {
        [Key]
        public int Id { get; set; } // Benzersiz kimlik numarası

        // LicensePlateId ile ilişkilendirilecek. Plaka numarasını tutan tablodan gelecek.
        [Required]
        public int AracPlakaId { get; set; }
        public AracPlaka? AracPlaka { get; set; } // Plaka bilgisi, LicensePlate tablosundan gelecek

        // Durum (Status) LookupList ile ilişkilendirilecek
        [ForeignKey("CezaDurumId")]
        public int CezaDurumId { get; set; } // Status
        public LookupList CezaDurum { get; set; } // Durum bilgisi, LookupList'ten gelecek

        // MinutesType, denetim türünü belirtir. LookupList ile ilişkilendirilecek
        [ForeignKey("DenetimTuruId")]
        public int DenetimTuruId { get; set; } // MinutesType
        public LookupList DenetimTuru { get; set; } // Denetim türü, LookupList'ten gelecek

        // Argument alanı, olay veya denetim ile ilgili bir açıklama ya da ek bilgi içerebilir. LookupList ile ilişkilendirilecek
        [ForeignKey("CezaDelilTipId")]
        public int? CezaDelilTipId { get; set; } // Argument
        public LookupList CezaDelilTip { get; set; } // Açıklama bilgisi, LookupList'ten gelecek

        [ForeignKey("CezaDelilTip2Id")]
        public int? CezaDelilTip2Id { get; set; } // Argument
        public LookupList CezaDelilTip2 { get; set; } // Açıklama bilgisi, LookupList'ten gelecek

        // Denetimin oluşturulma tarihi
        [Required]
        public DateTime OlusturmaTarihi { get; set; } // CreatedOn

        // Denetimi oluşturan kullanıcı, User tablosu ile ilişkilendirilecek
        [ForeignKey("OlusturanKullaniciId")]
        public int OlusturanKullaniciId { get; set; } // CreatedBy

        public User OlusturanKullanici { get; set; } // Oluşturan kullanıcının bilgisi, User tablosundan gelecek

        public ICollection<DenetimGecmisi> DenetimGecmisleri { get; set; } = new List<DenetimGecmisi>();
        public ICollection<DenetimCeza>? DenetimCezalari { get; set; } // Denetim ile ilişkili cezalar

    }

}
