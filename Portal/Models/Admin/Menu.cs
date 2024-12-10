namespace Portal.Models.Admin
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int RoleId { get; set; }  // Hangi role ait olduğunu belirtmek için
    }
}
