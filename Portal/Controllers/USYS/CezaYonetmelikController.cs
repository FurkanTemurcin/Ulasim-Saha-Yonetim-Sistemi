using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portal.Models.Admin;
using Portal.Models.USYS;
using Portal.ViewModels.USYS;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Portal.Controllers.USYS
{
    public class CezaYonetmelikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CezaYonetmelikController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // LookupList'ten LookUpId'si 2 olan Plaka Türleri'ni getiriyoruz
            var plakaTurleri = await _context.LookupLists
                .Where(l => l.LookUpId == 2)
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),  // ID değeri
                    Text = l.Name  // Kullanıcıya gösterilecek ad
                })
                .ToListAsync();

            // ViewBag ile Plaka Türleri'ni View'a taşıyoruz
            ViewBag.PlakaTurleri = plakaTurleri;

            // return View();
            return PartialView("_CreateCezaYonetmelikPartial");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? PlakaTuruId, CreateCezaYonetmelikViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (PlakaTuruId == null)
                {
                    ModelState.AddModelError("", "Lütfen bir Plaka Türü seçiniz.");
                    return View(viewModel);
                }

                // Giriş yapan kullanıcının Id'sini HttpContext üzerinden alıyoruz
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    // Kullanıcı oturum açmamışsa bir hata gösterebilirsiniz veya başka bir işlem yapabilirsiniz
                    return Unauthorized();
                }

                int olusturanKullaniciId = int.Parse(userIdClaim.Value);

                // Kayıt işlemi burada yapılır
                var cezaYonetmelik = new CezaYonetmelik
                {
                    Madde = viewModel.Madde,
                    Fikra = viewModel.Fikra,
                    Bent = viewModel.Bent,
                    Yonetmelik = viewModel.Yonetmelik,
                    CezaPuani = viewModel.CezaPuani,
                    PlakaTuruId = PlakaTuruId,  // Seçilen Plaka Türü ID'sini kaydediyoruz
                    AnahtarKelime = viewModel.AnahtarKelime,
                    OlusturanKullaniciId = olusturanKullaniciId,  // Giriş yapan kullanıcının Id'sini atıyoruz
                    OlusturmaTarihi = DateTime.Now
                };

                // Veritabanına kaydet
                _context.CezaYonetmelikleri.Add(cezaYonetmelik);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }

            // Eğer form geçersizse tekrar Plaka Türleri'ni dolduruyoruz ve formu geri döndürüyoruz
            ViewBag.PlakaTurleri = await _context.LookupLists
                .Where(l => l.LookUpId == 2)
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                })
                .ToListAsync();

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // CezaYonetmelik tablosunu LookupList ile birlikte çekiyoruz
            var cezaYonetmelikleri = await _context.CezaYonetmelikleri
                .Include(c => c.PlakaTuru) // LookupList ile ilişkili PlakaTuru'nu da getiriyoruz
                .Include(c => c.OlusturanKullanici) // Kullanıcı bilgilerini de getiriyoruz
                .ToListAsync();

            return View(cezaYonetmelikleri);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cezaYonetmelik = await _context.CezaYonetmelikleri
                .Include(c => c.PlakaTuru)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cezaYonetmelik == null)
            {
                return NotFound();
            }

            // CezaYonetmelik verilerini ViewModel'e aktarıyoruz
            var viewModel = new EditCezaYonetmelikViewModel
            {
                Id = cezaYonetmelik.Id,
                Madde = cezaYonetmelik.Madde,
                Fikra = cezaYonetmelik.Fikra,
                Bent = cezaYonetmelik.Bent,
                Yonetmelik = cezaYonetmelik.Yonetmelik,
                CezaPuani = cezaYonetmelik.CezaPuani,
                SelectedPlakaTuruId = cezaYonetmelik.PlakaTuruId,
                AnahtarKelime = cezaYonetmelik.AnahtarKelime
            };

            // LookupList'ten Plaka Türleri'ni getiriyoruz
            viewModel.PlakaTurleri = await _context.LookupLists
                .Where(l => l.LookUpId == 2)
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                })
                .ToListAsync();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCezaYonetmelikViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var cezaYonetmelik = await _context.CezaYonetmelikleri.FindAsync(id);
                if (cezaYonetmelik == null)
                {
                    return NotFound();
                }

                // CezaYonetmelik güncelleniyor
                cezaYonetmelik.Madde = viewModel.Madde;
                cezaYonetmelik.Fikra = viewModel.Fikra;
                cezaYonetmelik.Bent = viewModel.Bent;
                cezaYonetmelik.Yonetmelik = viewModel.Yonetmelik;
                cezaYonetmelik.CezaPuani = viewModel.CezaPuani;
                cezaYonetmelik.PlakaTuruId = viewModel.SelectedPlakaTuruId;
                cezaYonetmelik.AnahtarKelime = viewModel.AnahtarKelime;
                cezaYonetmelik.GuncellemeTarihi = DateTime.Now;

                _context.CezaYonetmelikleri.Update(cezaYonetmelik);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            // Hatalı form yeniden dolduruluyor
            viewModel.PlakaTurleri = await _context.LookupLists
                .Where(l => l.LookUpId == 2)
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                })
                .ToListAsync();

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cezaYonetmelik = await _context.CezaYonetmelikleri.FindAsync(id);
            if (cezaYonetmelik == null)
            {
                return NotFound();
            }

            _context.CezaYonetmelikleri.Remove(cezaYonetmelik);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




    }
}
