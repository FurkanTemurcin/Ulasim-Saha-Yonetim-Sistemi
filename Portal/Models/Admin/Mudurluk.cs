using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models.Admin
{
    public class Mudurluk
    {
      [Key]
          public int Id { get; set; }
    public string Ad { get; set; }

    public int BirimId { get; set; }
    public Birim Birim { get; set; }  // Birim ile ilişki

    public ICollection<Sube> Subeler { get; set; }  // Şubeler ile ilişki
    }
        
    }
