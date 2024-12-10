using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Models.Admin;

namespace Portal.Models.USYS
{
    public class ZamanCizelge
    {
        [Key]
        public int Id { get; set; } // Benzersiz kimlik numarası (Primary Key)

        // Route ile ilişki (Güzergah tablosuyla bağlantı)
        [ForeignKey("GuzergahId")]
        public int GuzergahId { get; set; } // RouteId alanı
        public Guzergah Guzergah { get; set; } // Navigation Property (İlgili güzergah bilgisi)

        // PlaceType alanı LookUpList'ten referans alacak
        [ForeignKey("YerTuruId")]
        public int YerTuruId { get; set; } // PlaceType alanı
        public LookupList YerTuru { get; set; } // Navigation Property (Yer türü bilgisi)

        // Gün bilgisi LookUpList'ten referans alacak
        [ForeignKey("GunId")]
        public int? GunId { get; set; } // Days alanı
        public LookupList? Gun { get; set; } // Navigation Property (Gün bilgisi)

        public TimeSpan? BaslangicSaati { get; set; } // StartedTime alanı
        public TimeSpan? BitisSaati { get; set; } // EndedTime alanı
        public double? Mesafe { get; set; } // Distance alanı (Mesafe bilgisi)

        [ForeignKey("OlusturanKullaniciId")]
        public int OlusturanKullaniciId { get; set; } // CreatedBy alanı
        public User OlusturanKullanici { get; set; } // Navigation Property (Oluşturan kullanıcı bilgisi)

        public DateTime OlusturulmaTarihi { get; set; } // CreatedOn alanı

    }
}