 using System.Linq;
using Portal.Models.Admin; // Veritabanı bağlamınızın olduğu namespace

namespace Portal.Helpers
{
    public class UserRoleHelper
    {
       
 private readonly ApplicationDbContext _context;

        public UserRoleHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        // Kullanıcı rolünü kontrol eden metot
        public bool CheckUserRole(int userId, int requiredRoleId)
        {
            return _context.UserRoles.Any(ur => ur.UserId == userId && ur.RoleId == requiredRoleId);
        }
    }
    }

    //layoutta menuyu burole göre basıyoruz dinamik olanı klasor>controller>Actionu