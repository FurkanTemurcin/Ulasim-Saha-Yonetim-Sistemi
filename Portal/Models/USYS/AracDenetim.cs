using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Models.Admin;

namespace Portal.Models.USYS
{
    public class AracDenetim
    {
        [Key]
        public int Id { get; set; }
        [Required]


        public int AracPlakaId { get; set; } // Foreign Key - AracPlaka tablosundan alınacak (Türkçeleştirilmiş)
        [Required]

        public int KullaniciId { get; set; } // Foreign Key - User tablosundan alınacak (Kullanıcı bilgisi)

        [Required]

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now; // Oluşturulma tarihi

        // Navigation Properties
        [ForeignKey("AracPlakaId")]
        public AracPlaka AracPlaka { get; set; } // İlişkili araç bilgisi

        [ForeignKey("KullaniciId")]
        public User Kullanici { get; set; } // İlişkili kullanıcı bilgisi

        public ICollection<AracDenetimGecmis> AracDenetimGecmisleri { get; set; } // Araç denetim ile ilişkili denetim geçmişi bilgisi
            public bool? CezaDurum { get; set; } // bit (true/false) olarak tutulacak





    }
}