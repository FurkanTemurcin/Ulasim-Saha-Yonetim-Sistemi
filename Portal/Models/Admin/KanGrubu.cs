namespace Portal.Models.Admin
{
    public class KanGrubu
    {
 
    public int Id { get; set; }
    public string Ad { get; set; }

    // Navigation property (optional)
    public ICollection<User> Users { get; set; }


    }
}