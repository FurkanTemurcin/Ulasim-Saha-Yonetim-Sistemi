using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Models.USYS
{
    public class AracDenetimGecmis
    {
        [Key]
        public int Id { get; set; } // Anahtar kolon

        [Required]
        public int AracDenetimId { get; set; } // Foreign Key - AracDenetim (VehicleControl) tablosundan alınacak

        [Required]
        public int KontrolTuruId { get; set; } // Foreign Key - LookupList tablosundan alınacak (Kontrol Türü)
        [Required]

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now; // Oluşturulma tarihi

        // Navigation Properties
        [ForeignKey("AracDenetimId")]
        public AracDenetim AracDenetim { get; set; } // İlişkili araç denetim bilgisi

        [ForeignKey("KontrolTuruId")]
        public LookupList KontrolTuru { get; set; } // İlişkili kontrol türü bilgisi


    }
}