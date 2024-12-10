using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Models.USYS
{
    public class UcretTakvim
    {
          [Key]
        public int Id { get; set; } // Benzersiz kimlik numarası (Primary Key)

        // Güzergah ile ilişkilendirilmiş olan RouteId alanı
        [ForeignKey("GuzergahId")]
        public int GuzergahId { get; set; } // Güzergah kimlik numarası
        public Guzergah Guzergah { get; set; } // Güzergah ile ilişki (Navigation Property)

        // Ücret bilgisi (Payment)
        [Required]
        public decimal? Ucret { get; set; } // Ücret bilgisi, virgüllü sayılar için 'decimal' veri tipi kullanılır
        
    }
}