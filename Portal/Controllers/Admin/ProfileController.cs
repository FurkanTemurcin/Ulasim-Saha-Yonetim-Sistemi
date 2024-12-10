using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.Models;
using Portal.ViewModels;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Portal.Helpers;
using Portal.Models.Admin;
using Portal.ViewModels.Admin;

namespace Portal.Controllers.Admin
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ApplicationDbContext context, ILogger<ProfileController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Mevcut kullanıcı alınıyor...");
            var email = User.Identity.Name;

            // Kullanıcı, Sube ve ilişkili Mudurluk ve Birim bilgilerini içeren sorgu
            var user = await _context.Users
            .Include(u => u.KanGrubu)
                .Include(u => u.Sube)
                    .ThenInclude(s => s.Mudurluk)
                        .ThenInclude(m => m.Birim)
                .SingleOrDefaultAsync(u => u.Email == email);



            if (user == null)
            {
                _logger.LogWarning("Kullanıcı bulunamadı, giriş sayfasına yönlendiriliyor.");
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcıya bağlı şube, müdürlük ve birim bilgilerini alıyoruz
            var sube = user.Sube;
            var mudurluk = sube?.Mudurluk;
            var birim = mudurluk?.Birim;

            var model = new ProfileViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Company = user.Company,
                JobTitle = user.JobTitle,
                Country = user.Country,
                Address = user.Address,
                Phone = user.Phone,
                Twitter = user.Twitter,
                Facebook = user.Facebook,
                Puan = user.Puan,
                Instagram = user.Instagram,
                ProfileImage = user.ProfileImage,
                About = user.About,
                Password = user.Password,
                TcKimlik = user.TcKimlik,
                BirthDate = user.BirthDate,
                Cinsiyet = user.Cinsiyet,
                OgrenimDurumu = user.OgrenimDurumu,
                MedeniDurum = user.MedeniDurum,
                Ehliyet = user.Ehliyet,
                BirimAd = birim?.Ad,
                MudurlukAd = mudurluk?.Ad,
                SubeAd = sube?.Ad,
                KanGrubu = user.KanGrubu?.Ad, // Kan Grubu adını direkt alıyoruz

            };

            ViewBag.ChangePasswordModel = new ChangePasswordViewModel();
            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> Update(ProfileViewModel model)
        {
            _logger.LogInformation("Güncelleme işlemi başlatıldı... ModelState geçerli: {ModelStateValid}", ModelState.IsValid);

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Kullanıcı ID: {UserId}", model.Id);

                var user = await _context.Users.FindAsync(model.Id);
                if (user == null)
                {
                    _logger.LogWarning("Güncellenecek kullanıcı bulunamadı. Kullanıcı ID: {UserId}", model.Id);
                    return NotFound();
                }

                _logger.LogInformation("Kullanıcı bulundu: {UserId}", user.Id);

                user.FullName = model.FullName;
                user.Email = model.Email;
                user.Password = model.Password;
                user.About = model.About ?? string.Empty;
                user.Address = model.Address ?? string.Empty;
                user.Company = model.Company ?? string.Empty;
                user.JobTitle = model.JobTitle ?? string.Empty;
                user.Country = model.Country ?? string.Empty;
                user.Phone = model.Phone;
                user.Twitter = model.Twitter ?? string.Empty;
                user.Facebook = model.Facebook ?? string.Empty;
                user.Instagram = model.Instagram ?? string.Empty;
                user.Puan = model.Puan ?? string.Empty;
                user.TcKimlik = model.TcKimlik;
                user.BirthDate = model.BirthDate;
                user.Cinsiyet = model.Cinsiyet; // Yeni alan
                user.OgrenimDurumu = model.OgrenimDurumu; // Yeni alan




                _logger.LogInformation("Kullanıcı bilgileri güncellendi: {UserId}", user.Id);

                if (model.ProfileImageFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.ProfileImageFile.CopyToAsync(memoryStream);
                        user.ProfileImage = memoryStream.ToArray();
                    }
                    _logger.LogInformation("Profil resmi güncellendi: {UserId}", user.Id);
                }

                _context.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Veritabanına kaydedildi: {UserId}", user.Id);

                return RedirectToAction("Index");
            }

            _logger.LogWarning("ModelState geçersiz. Güncelleme işlemi başarısız. Hatalar: {ModelStateErrors}", string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ShowImage(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null && user.ProfileImage != null)
            {
                return File(user.ProfileImage, "image/jpeg"); // veya image/png
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userId, out var userIdAsInt))
                {
                    ModelState.AddModelError("", "Kullanıcı kimliği geçersiz.");
                    return View(model);
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userIdAsInt);

                if (user == null)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                    return View(model);
                }

                // Mevcut şifreyi hashleyip doğrulama işlemi
                if (!HashHelper.VerifyPassword(model.CurrentPassword, user.Password))
                {
                    ModelState.AddModelError("", "Mevcut şifre yanlış.");
                    return View(model);
                }

                // Yeni şifreyi hashleyip kullanıcıya atama işlemi
                user.Password = HashHelper.HashPassword(model.NewPassword);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                ViewBag.Message = "Şifre başarıyla değiştirildi.";
            }
            return View(model);
        }

    }


}