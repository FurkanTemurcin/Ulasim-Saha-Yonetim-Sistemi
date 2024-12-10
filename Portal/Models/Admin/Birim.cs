using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models.Admin
{
    public class Birim
    {  [Key]
         public int Id { get; set; }
    public string Ad { get; set; }

    public ICollection<Mudurluk> Mudurlukler { get; set; }  // Müdürlükler ile ilişki
        
    }
}