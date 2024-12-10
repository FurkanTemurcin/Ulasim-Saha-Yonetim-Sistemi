using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Models;
using System.Linq;
using System.Security.Claims;
using Portal.ViewModels;
using Portal.Helpers;
using Portal.Models.Admin;
using Portal.ViewModels.Admin;


namespace Portal.Controllers.Admin
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<AccountController> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            _logger.LogInformation("Login page requested.");
            return View();
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            {
                if (ModelState.IsValid)
                {
                    var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);

                    if (user == null)
                    {
                        ModelState.AddModelError("Email", "Bu e-posta adresi ile kayıtlı bir kullanıcı bulunamadı.");
                        return View(model);
                    }

                    if (!HashHelper.VerifyPassword(model.Password, user.Password))
                    {
                        ModelState.AddModelError("Password", "Geçersiz şifre.");
                        return View(model);
                    }

                    var roles = _context.UserRoles
                                        .Where(ur => ur.UserId == user.Id)
                                        .Select(ur => ur.Role.Name)
                                        .ToList();

                    var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email)
        };

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Set session values
                    _httpContextAccessor.HttpContext.Session.SetInt32("UserId", user.Id);
                    _httpContextAccessor.HttpContext.Session.SetString("UserName", user.Email);
                    _httpContextAccessor.HttpContext.Session.SetString("UserFullName", user.FullName);

                    // JobTitle ve ProfileImage alanları için null kontrolü
                    _httpContextAccessor.HttpContext.Session.SetString("UserJobTitle", user.JobTitle ?? "Unknown");

                    if (user.ProfileImage != null && user.ProfileImage.Length > 0)
                    {
                        _httpContextAccessor.HttpContext.Session.SetString("UserProfileImage", Convert.ToBase64String(user.ProfileImage));
                    }
                    else
                    {
                        _httpContextAccessor.HttpContext.Session.SetString("UserProfileImage", string.Empty); // Veya "NoImageAvailable" gibi bir varsayılan değer
                    }

                    return RedirectToAction("Index", "Denetim");
                }

                return View(model);
            }

        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _httpContextAccessor.HttpContext.Session.Clear();  // Clear the session

            return RedirectToAction("Login", "Account");
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Aynı e-posta adresine sahip bir kullanıcı olup olmadığını kontrol et
                var existingUser = _context.Users.SingleOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor.");
                    return View(model);
                }

                // Şifreyi hashle
                var hashedPassword = HashHelper.HashPassword(model.Password);

                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = hashedPassword, // Hashlenmiş şifreyi kullan
                    TcKimlik = model.TcKimlik,
                    BirthDate = model.BirthDate,
                    Puan = model.Puan ?? "0",
                    Phone = model.Phone,
                    Cinsiyet = model.Cinsiyet, // Cinsiyet alanını kaydet
                    OgrenimDurumu = model.OgrenimDurumu, // Öğrenim durumu alanını kaydet



                };

                _context.Users.Add(user);
                _context.SaveChanges();

                // Kullanıcıya 'User' rolünü atayalım
                var role = _context.Roles.SingleOrDefault(r => r.Name == "User");
                if (role != null)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    };
                    _context.UserRoles.Add(userRole);
                }

                // Kullanıcıya Home/Index iznini atayalım
                var homeIndexPermission = _context.Permissions.FirstOrDefault(p => p.Controller == "Home" && p.Action == "Index");
                if (homeIndexPermission != null)
                {
                    var rolePermission = _context.RolePermissions
                        .SingleOrDefault(rp => rp.RoleId == role.Id && rp.PermissionId == homeIndexPermission.Id);

                    if (rolePermission == null)
                    {
                        rolePermission = new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = homeIndexPermission.Id
                        };
                        _context.RolePermissions.Add(rolePermission);
                    }
                }

                _context.SaveChanges();

                // Kayıttan sonra logine yönlendirme
                return RedirectToAction("Login");
            }

            return View(model);
        }

        // GET: ForgotPassword - Kullanıcıya TC Kimlik numarasını ve yeni şifreyi girmesi için form sağlar
[HttpGet]
public IActionResult ForgotPassword()
{
    return View(new ForgotPasswordViewModel());
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult ForgotPassword(ForgotPasswordViewModel model)
{
    if (!ModelState.IsValid)
    {
        return View(model);
    }

    // TC Kimlik ile kullanıcıyı bul
    var user = _context.Users.SingleOrDefault(u => u.TcKimlik == model.TcKimlik);
    if (user == null)
    {
        ModelState.AddModelError("TcKimlik", "Bu TC Kimlik numarasına ait bir kullanıcı bulunamadı.");
        return View(model);
    }

    // Yeni şifreyi hashle ve kaydet
    user.Password = HashHelper.HashPassword(model.NewPassword);
    _context.SaveChanges();

    // Başarı mesajı ve yönlendirme
    ViewBag.Message = "Şifreniz başarıyla güncellendi. Yeni şifrenizle giriş yapabilirsiniz.";
    return RedirectToAction("Login");
}

    }
}

