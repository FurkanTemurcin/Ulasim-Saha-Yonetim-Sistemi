using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Models.USYS
{
    public class DenetimCeza
    {
        [Key]
        public int Id { get; set; } // Benzersiz kimlik numarası (Primary Key)

        // Minutes ile ilişki
        [ForeignKey("DenetimId")]
        public int DenetimId { get; set; } // MinutesId alanı
        public Denetim Denetim { get; set; } // Navigation Property (İlgili denetim bilgisi)

        // PunishmentRegulations ile ilişki
        [ForeignKey("CezaYonetmelikId")]
        public int CezaYonetmelikId { get; set; } // PunishmentRegulationsId alanı
        public CezaYonetmelik CezaYonetmelik { get; set; } // Navigation Property (İlgili ceza maddesi bilgisi)
    }

}
