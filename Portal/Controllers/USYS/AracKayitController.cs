using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Portal.Models;
using System.Linq;
using Portal.ViewModels;
using Portal.Controllers.Admin;
using Portal.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Portal.ViewModels.USYS;
using Portal.Models.USYS;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Portal.Controllers.USYS
{
    [Route("AracKayit")]
    public class AracKayitController : BaseController
    {


        private readonly ApplicationDbContext _context;

        public AracKayitController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: AracPlaka Create formunu gösterir
        [HttpGet("Create")]
        public IActionResult Create()
        {
            // LookupList tablosundan PlakaTuru (LookUpId=2) verilerini almak
            var plakaTuruList = _context.LookupLists
                                        .Where(x => x.LookUpId == 2) // Plaka türü için LookUpId = 2
                                        .Select(x => new SelectListItem
                                        {
                                            Value = x.Id.ToString(),
                                            Text = x.Name
                                        })
                                        .ToList();

            // Guzergahlar tablosundan Güzergah verilerini almak
            var guzergahList = _context.Guzergahlar
                                       .Select(x => new SelectListItem
                                       {
                                           Value = x.Id.ToString(),
                                           Text = x.Ad
                                       })
                                       .ToList();

            ViewBag.PlakaTuruList = new SelectList(plakaTuruList, "Value", "Text");
            ViewBag.GuzergahList = new SelectList(guzergahList, "Value", "Text");

            // return View();
            return PartialView("_CreateAracPartial");

        }

        // POST: Formdan gelen verileri alıp kaydeder
        [HttpPost("Create")]
        public IActionResult Create(CreateAracPlakaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // ViewModel'den AracPlaka modeline dönüşüm
                var aracPlaka = new AracPlaka
                {
                    PlakaNumarasi = viewModel.PlakaNumarasi,
                    Marka = viewModel.Marka,
                    VizeTarihi = viewModel.VizeTarihi,
                    KoltukKapasitesi = viewModel.KoltukKapasitesi,
                    Model = viewModel.Model,
                    MotorNumarasi = viewModel.MotorNumarasi,
                    SasiNumarasi = viewModel.SasiNumarasi,
                    PlakaTuruId = viewModel.PlakaTuruId,
                    GuzergahId = viewModel.GuzergahId
                };

                _context.AracPlakalari.Add(aracPlaka);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Hata durumunda dropdown'ları tekrar doldurmak gerekir
            var plakaTuruList = _context.LookupLists
                                        .Where(x => x.LookUpId == 2)
                                        .Select(x => new SelectListItem
                                        {
                                            Value = x.Id.ToString(),
                                            Text = x.Name
                                        })
                                        .ToList();

            var guzergahList = _context.Guzergahlar
                                       .Select(x => new SelectListItem
                                       {
                                           Value = x.Id.ToString(),
                                           Text = x.Ad
                                       })
                                       .ToList();

            ViewBag.PlakaTuruList = new SelectList(plakaTuruList, "Value", "Text");
            ViewBag.GuzergahList = new SelectList(guzergahList, "Value", "Text");

            return RedirectToAction("Index");
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var aracPlakaList = _context.AracPlakalari
                                        .Select(ap => new
                                        {
                                            ap.Id,
                                            ap.PlakaNumarasi,
                                            ap.Marka,
                                            ap.VizeTarihi,
                                            ap.KoltukKapasitesi,
                                            ap.Model,
                                            ap.MotorNumarasi,
                                            ap.SasiNumarasi,
                                            PlakaTuru = _context.LookupLists.FirstOrDefault(l => l.Id == ap.PlakaTuruId).Name, // LookupLists'teki plaka türü adı
                                            Guzergah = _context.Guzergahlar.FirstOrDefault(g => g.Id == ap.GuzergahId).Ad // Guzergah tablosundaki güzergah adı
                                        })
                                        .ToList();

            return View(aracPlakaList);
        }






    }
}
