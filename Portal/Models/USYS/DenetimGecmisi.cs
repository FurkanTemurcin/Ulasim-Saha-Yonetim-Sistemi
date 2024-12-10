using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Models.Admin;

namespace Portal.Models.USYS
{
    public class DenetimGecmisi
    {
        [Key]
        public int Id { get; set; } // Benzersiz kimlik numarası

        // MinutesId ile ilişkilendirilecek. Denetim tablosundaki Id ile bağlantılı olacak.
        [Required]
        public int DenetimId { get; set; } // MinutesId (Foreign Key)

        [ForeignKey("DenetimId")] // DenetimId, Denetim tablosu ile ilişkilendirilecek FK'dir.
        public Denetim Denetim { get; set; } // Denetim tablosu ile ilişki (navigasyon özelliği)

        // Yorum (Comment) alanı
        public string? Yorum { get; set; } // Comment

        // StatusId ile ilişkilendirilecek. Status bilgisi LookupList tablosundan gelecek
        [ForeignKey("CezaDurumId")]
        public int CezaDurumId { get; set; } // StatusId
        public LookupList CezaDurum { get; set; } // Durum bilgisi, LookupList'ten gelecek

        // Geçmişin oluşturulma tarihi
        [Required]
        public DateTime OlusturmaTarihi { get; set; } // CreatedOn

        // Geçmişi oluşturan kullanıcı, User tablosu ile ilişkilendirilecek
        [ForeignKey("OlusturanKullaniciId")]
        public int OlusturanKullaniciId { get; set; } // CreatedBy
        public User OlusturanKullanici { get; set; } // Oluşturan kullanıcı bilgisi, User tablosundan gelecek

        // Geçmiş kaydının durum tarihi
        public DateTime? DurumTarihi { get; set; } // StatusDate

        // Uyarı süresi (WarningDuration)
        public int? UyariSuresi { get; set; } // WarningDuration
        public ICollection<AracDenetimGecmisiFotoVideo>? AracDenetimGecmisiFotoVideolari { get; set; } // Fotoğraflar koleksiyonu
        public string? Coordinates { get; set; } // Plaka numarası bilgisi
        public string? LocationName { get; set; }


    }
}