using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Portal.Controllers.Admin;
using Portal.Models;
using Portal.Models.Admin;
using Portal.Models.USYS;
using Portal.ViewModels.USYS;
using System.Linq;

namespace Portal.Controllers.USYS
{
    [Route("Rapor")]
    public class RaporController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public RaporController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("rapor")] // Bu satırla rota belirtiliyor
        public IActionResult Index()
        {
         
            var model = new RaporViewModel
            {
                IhlalTutanakSayisi = _context.Denetimler.Count(d => d.DenetimTuruId == 401),
                IhtarTutanakSayisi = _context.Denetimler.Count(d => d.DenetimTuruId == 402),
                DenetimSayisi = _context.AracDenetimleri.Count(),
                ToplamAracSayisi = _context.AracPlakalari.Count(),

                CezaSayilari = _context.DenetimCezalari
                    .Include(dc => dc.CezaYonetmelik)
                    .GroupBy(dc => dc.CezaYonetmelik.AnahtarKelime)
                    .Select(g => new CezaSayisi
                    {
                        YonetmelikAnahtarKelime = g.Key.ToString(),
                        CezaAdedi = g.Count()
                    })
                    .OrderByDescending(x => x.CezaAdedi)
                    .ToList(),

                AracDenetimSayilari = _context.AracDenetimleri
                    .Include(ad => ad.AracPlaka)
                    .GroupBy(ad => ad.AracPlaka.PlakaNumarasi)
                    .Select(g => new AracDenetimSayisi
                    {
                        PlakaNumarasi = g.Key,
                        DenetimAdedi = g.Count()
                    })
                    .OrderByDescending(x => x.DenetimAdedi)
                    .ToList(),

                KullaniciDenetimSayilari = _context.AracDenetimleri
                    .Include(ad => ad.Kullanici)
                    .GroupBy(ad => ad.Kullanici.FullName)
                    .Select(g => new KullaniciDenetimSayisi
                    {
                        KullaniciAdi = g.Key,
                        DenetimAdedi = g.Count()
                    })
                    .OrderByDescending(x => x.DenetimAdedi)
                    .ToList(),

                PlakaSahipler = _context.PlakaSahipleri.ToList(),
                PlakaCezaListesi = _context.PlakaCezaGenel.ToList()
            };

            return View(model);
        }
        [HttpGet("rapor-json")]
        public JsonResult GetFilteredData(DateTime? startDatePersonel, DateTime? endDatePersonel)
        {
            var query = _context.AracDenetimleri.AsQueryable();

            if (startDatePersonel.HasValue)
                query = query.Where(ad => ad.OlusturmaTarihi >= startDatePersonel.Value);

            if (endDatePersonel.HasValue)
            {
                var endDateAdjusted = endDatePersonel.Value.AddDays(1);
                query = query.Where(ad => ad.OlusturmaTarihi < endDateAdjusted);
            }

            var data = query
                .Include(ad => ad.Kullanici)
                .GroupBy(ad => ad.Kullanici.FullName)
                .Select(g => new KullaniciDenetimSayisi
                {
                    KullaniciAdi = g.Key,
                    DenetimAdedi = g.Count()
                })
                .OrderByDescending(x => x.DenetimAdedi)
                .ToList();

            return Json(new { KullaniciDenetimSayilari = data });
        }
        [HttpGet("rapor-arac")]
        public JsonResult GetFilteredDataArac(DateTime? startDateArac, DateTime? endDateArac)
        {
            var query = _context.AracDenetimleri.AsQueryable();

            if (startDateArac.HasValue)
            {
                query = query.Where(ad => ad.OlusturmaTarihi >= startDateArac.Value);
            }
            if (endDateArac.HasValue)
            {
                var endDateReal = endDateArac.Value.AddDays(1);
                query = query.Where(ad => ad.OlusturmaTarihi < endDateReal);
            }

            var data = query
                .Include(ad => ad.AracPlaka)
                .GroupBy(ad => ad.AracPlaka.PlakaNumarasi)
                .Select(g => new AracDenetimSayisi
                {
                    PlakaNumarasi = g.Key,
                    DenetimAdedi = g.Count()
                })
                .OrderByDescending(x => x.DenetimAdedi)
                .ToList();
            return Json(new{ AracDenetimSayilari = data });
        }
        [HttpGet("rapor-yonetmelik")]
        public JsonResult GetFilteredDataYonetmelik(DateTime? startDateYonetmelik, DateTime? endDateYonetmelik)
        {
            var denetimQuery = _context.Denetimler.AsQueryable();

            if (startDateYonetmelik.HasValue)
            {
                denetimQuery = denetimQuery.Where(d => d.OlusturmaTarihi >= startDateYonetmelik.Value);
            }
            if (endDateYonetmelik.HasValue)
            {
                var endDateReal = endDateYonetmelik.Value.AddDays(1);
                denetimQuery = denetimQuery.Where(d => d.OlusturmaTarihi < endDateReal);
            }

            var denetimIds = denetimQuery.Select(d => d.Id).ToList();

            var cezaData = _context.DenetimCezalari
                .Where(dc => denetimIds.Contains(dc.DenetimId))
                .Include(dc => dc.CezaYonetmelik)
                .GroupBy(dc => dc.CezaYonetmelik.AnahtarKelime)
                .Select(g => new CezaSayisi
                {
                    YonetmelikAnahtarKelime = g.Key,
                    CezaAdedi = g.Count()
                })
                .OrderByDescending(x => x.CezaAdedi)
                .ToList();

            return Json(new { YonetmelikCezaSayilari = cezaData });
        }


        [HttpGet("GetPlates")]
        public IActionResult GetPlates(string term)
        {
            var plakalar = _context.AracPlakalari
                .Where(p => p.PlakaNumarasi.Contains(term))
                .Select(p => new { value = p.Id, text = p.PlakaNumarasi })
                .ToList();

            return Json(plakalar);
        }
        [HttpGet]
        public IActionResult GetTutanakDataByPlaka(int? plakaId, DateTime? startDate, DateTime? endDate)
        {
            // Tarih aralığını kontrol et ve bitiş tarihine 1 gün ekle
            var inclusiveEndDate = endDate?.AddDays(1);

            // Denetimler tablosundan ihlal ve ihtar sayıları
            var sorgu = _context.Denetimler.AsQueryable();

            if (plakaId.HasValue)
            {
                sorgu = sorgu.Where(d => d.AracPlakaId == plakaId);
            }

            if (startDate.HasValue && inclusiveEndDate.HasValue)
            {
                sorgu = sorgu.Where(d => d.OlusturmaTarihi >= startDate && d.OlusturmaTarihi < inclusiveEndDate);
            }

            var sonuçlar = sorgu
                .GroupBy(d => d.DenetimTuruId)
                .Select(g => new
                {
                    DenetimTuruId = g.Key,
                    Sayisi = g.Count()
                })
                .ToList();

            var ihlalSayisi = sonuçlar.FirstOrDefault(x => x.DenetimTuruId == 401)?.Sayisi ?? 0;
            var ihtarSayisi = sonuçlar.FirstOrDefault(x => x.DenetimTuruId == 402)?.Sayisi ?? 0;

            // AracDenetimleri tablosundan denetim sayısı
            var denetimSorgu = _context.AracDenetimleri.AsQueryable();

            if (plakaId.HasValue)
            {
                denetimSorgu = denetimSorgu.Where(d => d.AracPlakaId == plakaId);
            }

            if (startDate.HasValue && inclusiveEndDate.HasValue)
            {
                denetimSorgu = denetimSorgu.Where(d => d.OlusturmaTarihi >= startDate && d.OlusturmaTarihi < inclusiveEndDate);
            }

            var denetimSayisi = denetimSorgu.Count();

            // JSON verisini oluştur
            var data = new
            {
                Ihlal = ihlalSayisi,
                Ihtar = ihtarSayisi,
                Denetim = denetimSayisi
            };

            return Json(data);
        }




        [HttpGet("getTutanakVerileri")]
        public async Task<IActionResult> GetTutanakVerileri(DateTime? startDate, DateTime? endDate, string dataType = "ihlal")
        {
            var now = DateTime.Now;

            if (dataType == "ihlal")
            {
                IQueryable<Denetim> query = _context.Denetimler.Where(t => t.DenetimTuruId == 401);

                // Tarih aralığına göre filtreleme
                if (startDate.HasValue)
                    query = query.Where(t => t.OlusturmaTarihi >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(t => t.OlusturmaTarihi <= endDate.Value.AddDays(1)); // Bitiş tarihine 1 gün ekle

                var tutanakVerileri = await query
                    .GroupBy(t => t.OlusturmaTarihi.Date)
                    .Select(g => new { Tarih = g.Key, Sayi = g.Count() })
                    .ToListAsync();

                var labels = tutanakVerileri.Select(v => v.Tarih.ToString("yyyy-MM-dd")).ToArray();
                var values = tutanakVerileri.Select(v => v.Sayi).ToArray();

                return Ok(new { labels, values });
            }
            else if (dataType == "denetim")
            {
                IQueryable<AracDenetim> query = _context.AracDenetimleri;

                if (startDate.HasValue)
                    query = query.Where(t => t.OlusturmaTarihi >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(t => t.OlusturmaTarihi <= endDate.Value.AddDays(1)); // Bitiş tarihine 1 gün ekle

                var denetimVerileri = await query
                    .GroupBy(t => t.OlusturmaTarihi.Date)
                    .Select(g => new { Tarih = g.Key, Sayi = g.Count() })
                    .ToListAsync();

                var labels = denetimVerileri.Select(v => v.Tarih.ToString("yyyy-MM-dd")).ToArray();
                var values = denetimVerileri.Select(v => v.Sayi).ToArray();

                return Ok(new { labels, values });
            }

            return BadRequest("Geçersiz dataType parametresi.");
        }


        // PlakaCeza verilerini almak için yeni bir metot
        [HttpGet("getPlakaCezaVerileri")]
        public IActionResult GetPlakaCezaVerileri()
        {
            var plakaCezaVerileri = _context.PlakaCezaGenel.ToList();
            return Ok(plakaCezaVerileri);
        }
    }
}




