using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portal.Models;
using Portal.Models.USYS;
using Portal.ViewModels.USYS;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Portal.Models.Admin;
using Portal.Controllers.Admin;
using Microsoft.EntityFrameworkCore;


namespace Portal.Controllers.USYS
{
    [Route("Denetim")]
    public class DenetimController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public DenetimController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Denetim oluşturma formu
        [HttpGet("DenetimOlustur")]
        public IActionResult DenetimOlustur()
        {
            // Veritabanından plakaları alıyoruz
            var model = new AracDenetimViewModel
            {
                Plakalar = _context.AracPlakalari
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.PlakaNumarasi })
                    .ToList(),
                KontrolTurleri = _context.LookupLists
                    .Where(l => l.LookUpId == 6) // Kontrol türleri LookupId = 6
                    .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name })
                    .ToList()
            };
            //  return View(model);

            return PartialView("_DenetimOlusturPartial", model);
        }
        [HttpPost("DenetimOlustur")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DenetimOlustur(AracDenetimViewModel model)
        {
            // Login olan kullanıcıyı al
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(); // Eğer kullanıcı kimliği yoksa yetkisiz erişim döndür
            }

            int olusturanKullaniciId = int.Parse(userIdClaim.Value); // Kullanıcı kimliğini al

            if (ModelState.IsValid)
            {
                // Yeni AracDenetim kaydını oluştur
                var aracDenetim = new AracDenetim
                {
                    AracPlakaId = model.AracPlakaId,
                    KullaniciId = olusturanKullaniciId, // Login olan kullanıcı ID
                    OlusturmaTarihi = DateTime.Now, // Oluşturulma tarihi
                    CezaDurum = model.CezaDurum // Modelden gelen CezaDurum değeri

                };

                // AracDenetim'i kaydet
                _context.AracDenetimleri.Add(aracDenetim);
                await _context.SaveChangesAsync(); // Denetim kaydedilir, Id otomatik olarak oluşturulur.

                // Seçilen her bir kontrol türü için AracDenetimGecmis kaydı oluştur
                foreach (var kontrolTuruId in model.SelectedKontrolTuruIds)
                {
                    var aracDenetimGecmis = new AracDenetimGecmis
                    {
                        AracDenetimId = aracDenetim.Id, // Yeni oluşturulan AracDenetim kaydının ID'si
                        KontrolTuruId = kontrolTuruId, // Seçilen kontrol türü ID'si
                        OlusturmaTarihi = DateTime.Now // Oluşturulma tarihi
                    };
                    _context.AracDenetimGecmisleri.Add(aracDenetimGecmis); // Denetim geçmişi kaydını ekle
                }

                // Tüm AracDenetimGecmis kayıtlarını kaydet
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Denetim");
            }

            // Formu tekrar doldur
            model.Plakalar = _context.AracPlakalari
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.PlakaNumarasi })
                .ToList();
            model.KontrolTurleri = _context.LookupLists
                .Where(l => l.LookUpId == 6)
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name })
                .ToList();

            return View(model); // Hatalarla birlikte formu yeniden göster
        }

        [HttpGet("GetFilteredPlates")]
        public JsonResult GetFilteredPlates(string term)
        {
            // Eğer term boş veya null ise, boş bir liste döndür
            if (string.IsNullOrWhiteSpace(term))
            {
                return Json(new List<object>());
            }

            // Plakalar veritabanından filtreleniyor
            var plakalar = _context.AracPlakalari
                .Where(p => p.PlakaNumarasi.StartsWith(term)) // Kullanıcının girdiği terimle başlayan plakalar
                .Select(p => new { id = p.Id, text = p.PlakaNumarasi }) // JSON için ID ve plaka numarası
                .ToList();

            return Json(plakalar);
        }



        public async Task<IActionResult> Index()
        {
            // AracDenetim ve ilişkili AracPlaka bilgilerini çekiyoruz
            var aracDenetimListesi = await _context.AracDenetimleri
                .Include(ad => ad.AracPlaka)  // AracPlaka tablosu ile ilişkiyi getiriyoruz
                .Include(ad => ad.Kullanici)  // Kullanıcı bilgilerini getiriyoruz
                .Include(ad => ad.AracDenetimGecmisleri) // Denetim geçmişlerini getiriyoruz
                .ThenInclude(gecmis => gecmis.KontrolTuru) // Kontrol türünü getiriyoruz
        .OrderByDescending(ad => ad.OlusturmaTarihi) // Doğrudan sıralama

                .ToListAsync();

            return View(aracDenetimListesi);
        }

    }


}
