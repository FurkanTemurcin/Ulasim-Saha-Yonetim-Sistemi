using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Models.USYS;


namespace Portal.Models.USYS
{
    public class AracDenetimGecmisiFotoVideo
    {
        [Key]
        public int Id { get; set; } // Benzersiz kimlik numarası (Primary Key)

        // DenetimGecmisi ile ilişkilendirilecek. Denetim geçmişi bilgisi tutulacak.
        [ForeignKey("DenetimGecmisiId")]
        public int DenetimGecmisiId { get; set; } // MinutesHistoryId
        public DenetimGecmisi DenetimGecmisi { get; set; } // DenetimGecmisi tablosu ile ilişki

        // FotoId ile Foto tablosu ile ilişkilendirilecek
        [ForeignKey("FotoId")]
        public int? FotoId { get; set; } // PhotoId
        public Foto? Foto { get; set; } // Foto tablosu ile ilişki
                                        // VideoId ile Video tablosu ile ilişkilendirilecek
        [ForeignKey("VideoId")]
        public int? VideoId { get; set; } // VideoId
        public Video? Video { get; set; } // Video tablosu ile ilişki

    }
}