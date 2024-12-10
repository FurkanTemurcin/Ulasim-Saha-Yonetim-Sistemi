using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models.Admin
{
    public class Sube
    {
        [Key]
   public int Id { get; set; }
    public string Ad { get; set; }

    public int MudurlukId { get; set; }
    public Mudurluk Mudurluk { get; set; }  // Müdürlük ile ilişki

    public ICollection<User> Users { get; set; }  // Kullanıcılar ile ilişki
    } 
    }
