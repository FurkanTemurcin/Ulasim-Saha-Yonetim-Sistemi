using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portal.Controllers.Admin;
using Portal.Models.Admin;
using Portal.Models.USYS;
using Portal.ViewModels.USYS;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Portal.Controllers.USYS
{
 public class GuzergahController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public GuzergahController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Güzergah Create formunu gösterir
        [HttpGet]
        public IActionResult Create()
        {
            // İlçe verilerini LookupList'ten al (LookUpId = 7 olanlar)
            var ilceList = _context.LookupLists
                                   .Where(x => x.LookUpId == 7)
                                   .Select(x => new SelectListItem
                                   {
                                       Value = x.Id.ToString(),
                                       Text = x.Name
                                   })
                                   .ToList();

            ViewBag.IlceList = new SelectList(ilceList, "Value", "Text");

            // return View();
                return PartialView("_CreateGuzergahPartial");

        }

  // POST: Güzergah Ekleme işlemi
[HttpPost]
public IActionResult Create(CreateGuzergahViewModel viewModel)
{
    if (ModelState.IsValid)
    {
        // ViewModel'den Guzergah modeline dönüşüm
        var guzergah = new Guzergah
        {
            Ad = viewModel.Ad,
            IlceId = viewModel.IlceId,
            GeometriId = viewModel.GeometriId
        };

        // Önce Güzergah kaydını ekleyelim
        _context.Guzergahlar.Add(guzergah);
        _context.SaveChanges(); // Güzergah kaydediliyor

        // Eklenen Güzergahın Id'sini alıyoruz
        var guzergahId = guzergah.Id;

        // UcretTakvimleri tablosuna kayıt ekleme
        var ucretTakvimi = new UcretTakvim
        {
            GuzergahId = guzergahId, // Yeni eklenen Güzergah'ın Id'sini kullanıyoruz
            Ucret = viewModel.Ucret // ViewModel'den gelen ücret bilgisi
        };

        // Ücret kaydını ekleyelim
        _context.UcretTakvimleri.Add(ucretTakvimi);
        _context.SaveChanges(); // Ücret kaydediliyor

        // Kayıttan sonra Index metoduna yönlendirme
        return RedirectToAction("Index");
    }

    // Hata durumunda dropdown'ları tekrar doldurmak gerekir
    var ilceList = _context.LookupLists
                           .Where(x => x.LookUpId == 7)
                           .Select(x => new SelectListItem
                           {
                               Value = x.Id.ToString(),
                               Text = x.Name
                           })
                           .ToList();

    ViewBag.IlceList = new SelectList(ilceList, "Value", "Text");

    return View(viewModel);
}


[HttpGet]
public IActionResult Index()
{
    // Güzergahlar ile LookupLists, UcretTakvimleri ve ZamanCizelgeleri tablolarını birleştiriyoruz
    var guzergahList = (from guzergah in _context.Guzergahlar
                        join lookup in _context.LookupLists on guzergah.IlceId equals lookup.Id
                        join ucretTakvimi in _context.UcretTakvimleri on guzergah.Id equals ucretTakvimi.GuzergahId into ucretGroup
                        from ucret in ucretGroup.DefaultIfEmpty() // Eğer ilgili ücret yoksa null döner
                        join zamanCizelgesi in _context.ZamanCizelgeleri on guzergah.Id equals zamanCizelgesi.GuzergahId into zamanGroup
                        from zaman in zamanGroup.DefaultIfEmpty() // Eğer zaman çizelgesi yoksa null döner
                        join gun in _context.LookupLists on zaman.GunId equals gun.Id into gunGroup
                        from gunName in gunGroup.DefaultIfEmpty() // GunId ile LookupLists'ten Name alıyoruz
                        join yer in _context.LookupLists on zaman.YerTuruId equals yer.Id into yerGroup
                        from yerName in yerGroup.DefaultIfEmpty() // YerTuruId ile LookupLists'ten Name alıyoruz
                        select new
                        {
                            guzergah.Id,
                            guzergah.Ad,
                            IlceId = guzergah.IlceId ?? 0, // Eğer IlceId null ise 0 olarak ayarlanır
                            GeometriId = guzergah.GeometriId ?? 0, // Eğer GeometriId null ise 0 olarak ayarlanır
                            IlceAdi = lookup.Name,
                            Ucret = (ucret != null) ? ucret.Ucret ?? 0 : 0, // Null kontrolü yapıyoruz ve null ise 0 atıyoruz
                            ZamanCizelgesi = (zaman != null) ? (zaman.BaslangicSaati + " - " + zaman.BitisSaati) : "Zaman yok", // Null kontrolü
                            Mesafe = (zaman != null) ? zaman.Mesafe : (double?)null, // Mesafe null olabilir
                            GunAdi = gunName != null ? gunName.Name : "Bilinmiyor", // Çalıştığı gün adı
                            YerAdi = yerName != null ? yerName.Name : "Bilinmiyor"  // Kalkış yeri adı
                        }).ToList();

    // Anonim türden GuzergahViewModel modeline dönüştürüyoruz
    var model = guzergahList.Select(g => new GuzergahViewModel
    {
        Id = g.Id,
        Ad = g.Ad,
        IlceId = g.IlceId,
        GeometriId = g.GeometriId,
        IlceAdi = g.IlceAdi,
        Ucret = g.Ucret,
        ZamanCizelgesi = g.ZamanCizelgesi,
        Mesafe = g.Mesafe,
        GunAdi = g.GunAdi, // Çalıştığı gün adı
        YerAdi = g.YerAdi  // Kalkış yeri adı
    }).ToList();

    return View(model);
}





        // POST: Güzergah Silme
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var guzergah = _context.Guzergahlar.Find(id);
            if (guzergah != null)
            {
                _context.Guzergahlar.Remove(guzergah);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
// GET: Zaman Cizelgesi Ekleme Sayfası
[HttpGet]
public IActionResult CreateZamanCizelge()
{
    // Güzergah Dropdown Listesi
    var guzergahList = _context.Guzergahlar
                          .Select(x => new SelectListItem
                          {
                              Value = x.Id.ToString(), // GüzergahId
                              Text = x.Ad              // Güzergah adı
                          })
                          .ToList();

    // Yer Türü Dropdown Listesi (LookUpId = 8)
    var yerTuruList = _context.LookupLists
                          .Where(x => x.LookUpId == 8) // YerTuruId için LookUpId = 8
                          .Select(x => new SelectListItem
                          {
                              Value = x.Id.ToString(),
                              Text = x.Name
                          })
                          .ToList();

    // Gün Dropdown Listesi (LookUpId = 9)
    var gunList = _context.LookupLists
                      .Where(x => x.LookUpId == 9) // GunId için LookUpId = 9
                      .Select(x => new SelectListItem
                      {
                          Value = x.Id.ToString(),
                          Text = x.Name
                      })
                      .ToList();

    // ViewBag ile verileri View'e gönderiyoruz
    ViewBag.GuzergahList = new SelectList(guzergahList, "Value", "Text");
    ViewBag.YerTuruList = new SelectList(yerTuruList, "Value", "Text");
    ViewBag.GunList = new SelectList(gunList, "Value", "Text");

    // Boş ViewModel ile formu başlatıyoruz
    return View(new CreateZamanCizelgeViewModel());
}

[HttpPost]
public IActionResult CreateZamanCizelge(CreateZamanCizelgeViewModel viewModel)
{
    // Giriş yapan kullanıcının Id'sini HttpContext üzerinden alıyoruz
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null)
    {
        // Kullanıcı oturum açmamışsa bir hata gösterebilirsiniz veya başka bir işlem yapabilirsiniz
        return Unauthorized();
    }

    int olusturanKullaniciId = int.Parse(userIdClaim.Value);

    if (ModelState.IsValid)
    {
        // Zaman Cizelgesi modeline dönüşüm
        var zamanCizelge = new ZamanCizelge
        {
            GuzergahId = viewModel.GuzergahId,
            YerTuruId = viewModel.YerTuruId,
            GunId = viewModel.GunId,
            BaslangicSaati = viewModel.BaslangicSaati,
            BitisSaati = viewModel.BitisSaati,
            Mesafe = viewModel.Mesafe,
            OlusturanKullaniciId = olusturanKullaniciId, // Burada küçük harfle olmalı
            OlusturulmaTarihi = DateTime.Now
        };

        // Zaman Cizelgesi kaydını veritabanına ekliyoruz
        _context.ZamanCizelgeleri.Add(zamanCizelge);
        _context.SaveChanges();

        // Kayıttan sonra Index metoduna yönlendirme
        return RedirectToAction("Index");
    }
    

    // Model geçersizse tekrar formu göster
    return View(viewModel);
}

public IActionResult Index2()
{
    var zamanCizelgeleri = _context.ZamanCizelgeleri
        .Select(z => new CreateZamanCizelgeViewModel
        {
            GuzergahId = z.GuzergahId,
            YerTuruId = z.YerTuruId,
            GunId = z.GunId,
            BaslangicSaati = z.BaslangicSaati,
            BitisSaati = z.BitisSaati,
            Mesafe = z.Mesafe,
            OlusturanKullaniciId = z.OlusturanKullaniciId,

            // Null kontrolü yapmak için önce sorguyu çalıştırıyoruz
            YerTuruName = _context.LookupLists.FirstOrDefault(l => l.Id == z.YerTuruId) != null 
                            ? _context.LookupLists.FirstOrDefault(l => l.Id == z.YerTuruId).Name
                            : "Bilinmiyor",
            GunName = _context.LookupLists.FirstOrDefault(l => l.Id == z.GunId) != null
                            ? _context.LookupLists.FirstOrDefault(l => l.Id == z.GunId).Name
                            : "Bilinmiyor"
        })
        .ToList();

    return View(zamanCizelgeleri);
}






    }
}