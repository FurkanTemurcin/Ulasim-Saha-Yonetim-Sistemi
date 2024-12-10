using System.ComponentModel.DataAnnotations;

namespace Portal.Models.USYS
{
    public class LookUp
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<LookupList> LookUpListesi { get; set; }

    }
}