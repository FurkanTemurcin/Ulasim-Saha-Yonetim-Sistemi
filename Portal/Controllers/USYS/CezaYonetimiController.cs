using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portal.Controllers.Admin;
using Portal.Models.Admin;
using Portal.Models.USYS;
using Portal.ViewModels.USYS;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Portal.Controllers.USYS
{
    public class CezaYonetimiController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public CezaYonetimiController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult GenerateViolationReport(int denetimId)
        {
            // Denetim kaydını ve ilişkili verileri al
            var denetim = _context.Denetimler
                .Where(d => d.Id == denetimId && d.DenetimTuru.Name == "İhlal Tutanağı")
                .Select(d => new
                {
                    d.Id,
                    Plaka = d.AracPlaka.PlakaNumarasi,
                    Olusturan = d.OlusturanKullanici.FullName,
                    d.OlusturmaTarihi,
                    Cezalar = d.DenetimCezalari.Select(c => new
                    {
                        c.Id,
                        CezaAdi = c.CezaYonetmelik.Yonetmelik
                    }).ToList()
                })
                .FirstOrDefault();

            if (denetim == null)
            {
                return NotFound("Geçerli bir ihlal tutanağı bulunamadı.");
            }

            // PDF belgesi oluştur
            var document = CreateViolationDocument(denetim);

            using (var memoryStream = new MemoryStream())
            {
                document.GeneratePdf(memoryStream);
                var base64String = Convert.ToBase64String(memoryStream.ToArray());
                var fileName = $"IhlalTutanagi_{denetim.Id}.pdf";

                return Json(new
                {
                    success = true,
                    pdfData = base64String,
                    downloadUrl = Url.Action("DownloadPdf", "Denetim", new { denetimId })
                });
            }
        }

        public IActionResult DownloadPdf(int denetimId)
        {
            // Denetim kaydını al ve PDF oluştur
            var denetim = _context.Denetimler
                .Where(d => d.Id == denetimId && d.DenetimTuru.Name == "İhlal Tutanağı")
                .Select(d => new
                {
                    d.Id,
                    Plaka = d.AracPlaka.PlakaNumarasi,
                    Olusturan = d.OlusturanKullanici.FullName,
                    d.OlusturmaTarihi,
                    Cezalar = d.DenetimCezalari.Select(c => new
                    {
                        c.Id,
                        CezaAdi = c.CezaYonetmelik.Yonetmelik
                    }).ToList()
                })
                .FirstOrDefault();

            if (denetim == null)
            {
                return NotFound("PDF indirilemiyor.");
            }

            var document = CreateViolationDocument(denetim);

            using (var memoryStream = new MemoryStream())
            {
                document.GeneratePdf(memoryStream);
                return File(memoryStream.ToArray(), "application/pdf", $"IhlalTutanagi_{denetim.Id}.pdf");
            }
        }




        private string GetDenetimTuruText(int denetimId)
        {
            // Denetim ID ile Denetim Turu ID alınır
            var denetimTuruId = _context.Denetimler
                .Where(x => x.Id == denetimId)
                .Select(x => x.DenetimTuruId)
                .FirstOrDefault();

            // DenetimTuruId'ye göre uygun metin döndürülür
            return denetimTuruId switch
            {
                401 => "İhlal Tutanağı",
                402 => "İhtar",
                _ => "Bilinmiyor"
            };
        }
        //yazdırıken
        private dynamic GetPlakaSahipBilgileriByDenetimId(int denetimId)
        {
            // Denetimden AracPlakaId alınır
            var aracPlakaId = _context.Denetimler
                .Where(d => d.Id == denetimId)
                .Select(d => d.AracPlakaId)
                .FirstOrDefault();

            if (aracPlakaId == null)
            {
                return new { Error = "AracPlakaId bulunamadı." };
            }

            // AracSahipleri tablosundan PlakaSahipId alınır
            var plakaSahipId = _context.AracSahipleri
                .Where(asahip => asahip.AracPlakaId == aracPlakaId)
                .Select(asahip => asahip.PlakaSahipId)
                .FirstOrDefault();

            if (plakaSahipId == null)
            {
                return new { Error = "PlakaSahipId bulunamadı." };
            }

            // PlakaSahipleri tablosundan bilgiler alınır
            var plakaSahipBilgileri = _context.PlakaSahipleri
                .Where(ps => ps.Id == plakaSahipId)
                .Select(ps => new
                {
                    TCKimlik = ps.KimlikNumarasi,
                    VergiNo = ps.VergiNumarasi,
                    Ad = ps.Ad,
                    Soyad = ps.Soyad,
                    Unvan = ps.Unvan,
                    Adres = ps.Adres,
                    Telefon = ps.Telefon,
                    CinsiyetId = ps.CinsiyetId
                })
                .FirstOrDefault();

            if (plakaSahipBilgileri == null)
            {
                return new { Error = "Plaka Sahibi bilgileri bulunamadı." };
            }

            return plakaSahipBilgileri;
        }

        //yazdırırken
        private string GetDelilTipiByDenetimId(int denetimId)
        {
            // Veritabanından denetim bilgilerini alıyoruz
            var denetim = _context.Denetimler
                .Where(d => d.Id == denetimId)
                .Select(d => new
                {
                    d.CezaDelilTipId,
                    d.CezaDelilTip2Id
                })
                .FirstOrDefault();

            if (denetim == null)
            {
                return "Deliller: Bilgi bulunamadı.";
            }

            // Delil Tipi kontrolü
            bool hasFoto = denetim.CezaDelilTipId == 503 || denetim.CezaDelilTip2Id == 503;
            bool hasVideo = denetim.CezaDelilTipId == 502 || denetim.CezaDelilTip2Id == 502;

            if (hasFoto && hasVideo)
            {
                return "Deliller: Fotoğraf ve Video";
            }
            else if (hasFoto)
            {
                return "Deliller: Fotoğraf";
            }
            else if (hasVideo)
            {
                return "Deliller: Video";
            }

            return "Deliller: Belirtilmemiş";
        }

        // Yönetmelik maddeyi yazdırma
        private string GetCezaBilgileriByDenetimId(int denetimId)
        {
            // Lambda ile yerel bir metot tanımlıyoruz
            Func<int?, string> GetPlakaTuruById = plakaTuruId => plakaTuruId switch
            {
                201 => "J Plaka",
                202 => "M Plaka",
                203 => "S Plaka",
                204 => "T Plaka",
                205 => "H Plaka",
                206 => "Diğer Plaka",
                _ => "Bilinmeyen Plaka Türü"
            };

            // DenetimCezalari tablosundan CezaYonetmelikId değerlerini alıyoruz
            var cezaBilgileri = _context.DenetimCezalari
                .Where(dc => dc.DenetimId == denetimId)
                .Select(dc => dc.CezaYonetmelikId)
                .ToList();

            if (cezaBilgileri == null || !cezaBilgileri.Any())
            {
                return "Ceza bilgisi bulunamadı.";
            }

            // CezaYonetmelikleri tablosundan ilgili bilgileri alıyoruz
            var cezaDetaylari = _context.CezaYonetmelikleri
                .Where(cy => cezaBilgileri.Contains(cy.Id))
                .Select(cy => new
                {
                    cy.Madde,
                    cy.Yonetmelik,
                    cy.AnahtarKelime,
                    PlakaTuru = GetPlakaTuruById(cy.PlakaTuruId) // Yerel metot kullanılıyor
                })
                .ToList();

            if (!cezaDetaylari.Any())
            {
                return "Ceza yönetmelik detayları bulunamadı.";
            }

          

            // Plaka türü, yönetmelik ve maddeleri birleştiriyoruz Anahtar kelimeleri yan yana virgülle
            var plakaTuru = cezaDetaylari.First().PlakaTuru; // İlk detayın plaka türünü alıyoruz
            var madde = cezaDetaylari.First().Madde; // İlk detayın madde bilgisi
            var yonetmelik = cezaDetaylari.First().Yonetmelik; // İlk detayın yönetmeliği
            var anahtarKelimeler = string.Join(", ", cezaDetaylari.Select(d => d.AnahtarKelime)); // Anahtar kelimeleri virgülle birleştiriyoruz

            // Dinamik metin oluşturuyoruz
            return $"Yukarıda adı yazılan şahsın bu fiili {plakaTuru} yönetmeliği {madde} maddesine aykırı olarak " +
                   $"{anahtarKelimeler} görüldüğünden ilgili hakkında kanuni muamele yapılmak üzere iş bu tespit " +
                   "tutanağı olay mahallinde tanzim olundu.";


        }

        //Yazdıra Cezakesildigi yeri ekleme Cezaolusturulduysa ekle
        private string GetCezaKesildigiYer(int denetimId)
        {
            // DenetimGecmisleri tablosundan ilgili bilgileri alıyoruz
            var gecmisKaydi = _context.DenetimGecmisleri
                .Where(dg => dg.DenetimId == denetimId && dg.CezaDurumId == 301)
                .Select(dg => new
                {
                    dg.Yorum,
                    dg.LocationName
                })
                .FirstOrDefault();

            if (gecmisKaydi == null || string.IsNullOrWhiteSpace(gecmisKaydi.Yorum))
            {
                return "Cezanın kesildiği yer bilgisi bulunamadı.";
            }

            return $"Cezanın kesildiği yer: {gecmisKaydi.Yorum}";
        }


        private IDocument CreateViolationDocument(dynamic denetim)
        {

            // Dinamik olarak Denetim Türü metni alınır
            string denetimTuruText = GetDenetimTuruText(denetim.Id);


            // Denetim ile ilişkili PlakaNumarasi bilgisi alınıyor
            var plakaSahipBilgileri = GetPlakaSahipBilgileriByDenetimId(denetim.Id);

            // Denetim ID'den delil bilgilerini al
            string delilBilgisi = GetDelilTipiByDenetimId(denetim.Id);

            string cezaBilgileri = GetCezaBilgileriByDenetimId(denetim.Id);
            string cezaKesildigiYer = GetCezaKesildigiYer(denetim.Id);



            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20); // Sayfa kenar boşlukları

                    // Header: Tarih üstte, başlık altta ortalanmış
                    page.Header().Column(headerColumn =>
                    {
                        // Sol üst köşeye tarih
                        // Sol üst köşeye tarih ve saat
                        headerColumn.Item().Row(row =>
                {
            row.RelativeItem().AlignLeft().Text($"Tarih: {DateTime.Now:dd.MM.yyyy} Saat: {DateTime.Now:HH:mm}").FontSize(12);
        });


                        headerColumn.Item().PaddingTop(30).AlignCenter().Column(column =>
                  {
                      // T.C. Başlığı
                    column.Item().AlignCenter().PaddingTop(30).Text("T.C.").FontSize(16).Bold();

                      // MALATYA BÜYÜKŞEHİR BELEDİYESİ Başlığı
                    column.Item().AlignCenter().Text("MALATYA BÜYÜKŞEHİR BELEDİYESİ").FontSize(14).Bold();

                      // Ulaşım Hizmetleri Daire Başkanlığı Başlığı
                    column.Item().AlignCenter().Text("Ulaşım Hizmetleri Daire Başkanlığı").FontSize(14).Bold();

                    column.Item().AlignCenter().PaddingTop(20).Text($" {denetimTuruText}").FontSize(14).Bold(); // Dinamik metin


                      // Denetim Türü Metni (İhlal Tutanağı veya İhtar)
                });

                    });

                    // İçerik
                    page.Content().PaddingTop(20).PaddingHorizontal(20).Column(column =>
                    {
                        column.Item().AlignLeft().Text("İLGİLİNİN:").FontSize(14).Bold();
                        column.Item().PaddingTop(10).Text($"Plaka: {denetim.Plaka}").FontSize(14);
                        column.Item().PaddingTop(10).Text($"T.C. Kimlik: {plakaSahipBilgileri.TCKimlik}").FontSize(14);
                        column.Item().PaddingTop(10).Text($"Vergi No: {plakaSahipBilgileri.VergiNo}").FontSize(14);
                        column.Item().PaddingTop(10).Text($"Adı Soyadı: {plakaSahipBilgileri.Ad} {plakaSahipBilgileri.Soyad}").FontSize(14);


                        column.Item().PaddingTop(10).Text($"Adresi: {plakaSahipBilgileri.Adres}").FontSize(14);



                        // column.Item().Text($"Plaka: {denetim.Plaka}").FontSize(14);
                        column.Item().PaddingTop(10).Text(delilBilgisi).FontSize(14); // Delil bilgilerini ekle


                        column.Item().PaddingTop(10).Text($"Tarih ve Saati: {denetim.OlusturmaTarihi:dd.MM.yyyy HH:mm}").FontSize(14);
                        column.Item().PaddingTop(10).Text(cezaKesildigiYer).FontSize(14);



                        
                        column.Item().PaddingTop(10).Text(cezaBilgileri).FontSize(14);

                    });


                   
                    // page.Footer().AlignRight().Text($"Oluşturma Tarihi: {DateTime.Now:dd.MM.yyyy}");
                    page.Footer().PaddingHorizontal(20).PaddingBottom(200).Row(row =>
            {
                // Sola hizalanan metin
            row.RelativeItem().AlignCenter().Text("Tebellüğ Eden").FontSize(14).Bold();

                // Ortada hizalanan metin
            row.RelativeItem().AlignCenter().Text("Görevli").FontSize(14).Bold();

                // Sağa hizalanan metin
            row.RelativeItem().AlignCenter().Text("Zabıta Memuru").FontSize(14).Bold();
        });



                });
            });
        }


        [HttpGet]
        public async Task<IActionResult> Ihlal()
        {
            var denetimler = await _context.Denetimler
                .Include(d => d.AracPlaka) // Plaka verisini almak için
                .Include(d => d.CezaDurum) // Ceza Durumu almak için
                .Include(d => d.DenetimTuru) // Denetim Turu (Tutanak Tipi) için
                .Include(d => d.DenetimCezalari) // DenetimCezalari ilişkisini dahil ediyoruz
                    .ThenInclude(dc => dc.CezaYonetmelik) // DenetimCezalari üzerinden CezaYonetmelik'e gidiyoruz
                .Include(d => d.DenetimGecmisleri) // DenetimGecmisleri ilişkisini dahil ediyoruz
                    .ThenInclude(dg => dg.CezaDurum) // DenetimGecmisleri üzerinden CezaDurum'una erişim
                .Where(d => d.DenetimTuruId == 401) // Sadece DenetimTuruId'si 401 olanları filtreliyoruz
                .ToListAsync();

            return View(denetimler);
        }

        [HttpGet]
        public async Task<IActionResult> Ihtar()
        {
            var denetimler = await _context.Denetimler
                .Include(d => d.AracPlaka) // Plaka verisini almak için
                .Include(d => d.CezaDurum) // Ceza Durumu almak için
                .Include(d => d.DenetimTuru) // Denetim Turu (Tutanak Tipi) için
                .Include(d => d.DenetimCezalari) // DenetimCezalari ilişkisini dahil ediyoruz
                    .ThenInclude(dc => dc.CezaYonetmelik) // DenetimCezalari üzerinden CezaYonetmelik'e gidiyoruz
                .Include(d => d.DenetimGecmisleri) // DenetimGecmisleri ilişkisini dahil ediyoruz
                    .ThenInclude(dg => dg.CezaDurum) // DenetimGecmisleri üzerinden CezaDurum'una erişim
        .Where(d => d.DenetimTuruId == 402) // Sadece DenetimTuruId'si 401 olanları filtreliyoruz
                .ToListAsync();

            return View(denetimler);
        }



        [HttpGet]
        public async Task<IActionResult> CreateCeza(string plaka, string kontrolTurleriJson,int? plakaNumarasi = null)
        {

    List<string> kontrolTurleriListesi = new List<string>();

    // JSON'dan Listeye Dönüştürme
    if (!string.IsNullOrEmpty(kontrolTurleriJson))
    {
        kontrolTurleriListesi = System.Text.Json.JsonSerializer.Deserialize<List<string>>(kontrolTurleriJson);
    }


            if (plakaNumarasi.HasValue)
            {
                // Plaka türünü al
                var plakaTuruId = await _context.AracPlakalari
                    .Where(ap => ap.Id == plakaNumarasi.Value)
                    .Select(ap => ap.PlakaTuruId)
                    .FirstOrDefaultAsync();

                // Plaka türü bulunmadıysa geri dön
                if (plakaTuruId == 0)
                {
                    Console.WriteLine("Hata: PlakaTuruId değeri 0 veya bulunamadı."); // Hata ayıklama için mesaj
                    return Json(new { success = false, message = "Plaka türü bulunamadı." });
                }

                // Anahtar kelimeleri al
                var anahtarKelimeler = await _context.CezaYonetmelikleri
                    .Where(cy => cy.PlakaTuruId == plakaTuruId && !string.IsNullOrEmpty(cy.AnahtarKelime))
                    .Select(cy => new { Id = cy.Id, AnahtarKelime = cy.AnahtarKelime })
                    .Distinct()
                    .ToListAsync();

                return Json(new { success = true, data = anahtarKelimeler });
            }

            // Plakaları al
            var plakalar = await _context.AracPlakalari
                .Where(p => !string.IsNullOrEmpty(p.PlakaNumarasi)) // Plaka numarası null olmayanlar
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.PlakaNumarasi
                })
                .ToListAsync() ?? new List<SelectListItem>();

            // Tutanak türlerini al
            var tutanakTurleri = await _context.LookupLists
                .Where(l => l.LookUpId == 4)
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                })
                .ToListAsync() ?? new List<SelectListItem>();

            // Delil türlerini al
            var delilTurleri = await _context.LookupLists
                .Where(l => l.LookUpId == 5)
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                })
                .ToListAsync() ?? new List<SelectListItem>();

            // Anahtar kelimeleri al
            var anahtarKelimelerTamamı = await _context.CezaYonetmelikleri
                .Where(c => !string.IsNullOrEmpty(c.AnahtarKelime))
                .Select(c => new SelectListItem
                {
                    Value = c.AnahtarKelime,
                    Text = c.AnahtarKelime
                })
                .Distinct()
                .ToListAsync() ?? new List<SelectListItem>();

            // ViewModel oluştur
            var viewModel = new CreateCezaViewModel
            {
                 Plaka = plaka,
        KontrolTurleri = kontrolTurleriListesi,
                Plakalar = plakalar,
                TutanakTurleri = tutanakTurleri,
                DelilTurleri = delilTurleri,
                AnahtarKelimeler = anahtarKelimelerTamamı
            };


            return View(viewModel);
        }

        // CezaYonetimiController.cs




        [HttpPost]
        public async Task<IActionResult> CreateCeza(CreateCezaViewModel model, List<IFormFile> fotoDosyalar, List<IFormFile> videoDosyalar, List<string> SelectedAnahtarKelimeler, string coordinates)
        {
            // Kullanıcının kimlik bilgilerini kontrol ediyoruz
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(); // Eğer kullanıcı kimliği yoksa yetkisiz erişim döndür
            }

            int olusturanKullaniciId = int.Parse(userIdClaim.Value);

            try
            {
                // Plaka ve Tutanak Türü değerlerini kontrol edin
                if (model.SelectedPlakaId == 0)
                {
                    ModelState.AddModelError("", "Lütfen geçerli bir plaka seçin.");
                    return View(model);
                }

                if (model.SelectedTutanakTuruId == 0)
                {
                    ModelState.AddModelError("", "Lütfen geçerli bir tutanak türü seçin.");
                    return View(model);
                }

                int plakaId = model.SelectedPlakaId;
                int tutanakTuruId = model.SelectedTutanakTuruId;

                // Delil tiplerini kontrol edin
                int? cezaDelilTipId = model.SelectedDelilTurleri != null && model.SelectedDelilTurleri.Count > 0
                    ? (int?)model.SelectedDelilTurleri[0]
                    : null;

                int? cezaDelilTip2Id = model.SelectedDelilTurleri != null && model.SelectedDelilTurleri.Count > 1
                    ? (int?)model.SelectedDelilTurleri[1]
                    : null;

                // Delil tipleri için geçerlilik kontrolü
                if (cezaDelilTipId == 0)
                {
                    ModelState.AddModelError("", "Geçerli bir CezaDelilTipId seçilmedi.");
                    return View(model);
                }

                if (cezaDelilTip2Id == 0 && model.SelectedDelilTurleri.Count > 1)
                {
                    ModelState.AddModelError("", "Geçerli bir CezaDelilTipId2 seçilmedi.");
                    return View(model);
                }

                // İlk olarak Denetim kaydını oluştur ve sadece bu kaydı kaydet
                var denetim = new Denetim
                {
                    AracPlakaId = plakaId,
                    CezaDurumId = 301,
                    DenetimTuruId = tutanakTuruId,
                    CezaDelilTipId = cezaDelilTipId ?? 0, // Eğer null ise varsayılan olarak 0 atanır
                    CezaDelilTip2Id = cezaDelilTip2Id,
                    OlusturmaTarihi = model.Tarih.Date.Add(model.Saat), // Formdan gelen Tarih ve Saat bilgilerini birleştiriyoruz

                    OlusturanKullaniciId = olusturanKullaniciId,
                };

                // Sadece denetim kaydını veritabanına ekle ve kaydet
                await _context.Denetimler.AddAsync(denetim);
                await _context.SaveChangesAsync(); // İlk kaydetme işlemi

                // Denetim ID'sini artık kullanabilirsin
                int denetimId = denetim.Id;

                // Formdan gelen tarih ve saat bilgilerini birleştirerek OlusturmaTarihi'ne atama
                DateTime olusturmaTarihi = model.Tarih.Date;
                if (model.Saat != null)
                {
                    olusturmaTarihi = olusturmaTarihi.Add(model.Saat); // Tarih ve saati birleştiriyoruz
                }

                var locationData = coordinates;

                // DenetimGecmisi kaydını oluştur ve ekle
                var denetimGecmisi = new DenetimGecmisi
                {
                    DenetimId = denetimId, // Yeni oluşturduğumuz denetim ID'sini kullanıyoruz
                    CezaDurumId = 301, // Ceza durumu ID'si
                    OlusturmaTarihi = olusturmaTarihi, // Birleştirilmiş tarih ve saat atanıyor
                    OlusturanKullaniciId = olusturanKullaniciId,
                    Yorum = model.KesildigiYer, // Ceza Kesildiği yer
                    DurumTarihi = null, // İlgili durum tarihi
                    Coordinates = locationData,
                    LocationName = model.LocationName,
                    UyariSuresi = model.BitisGunu // Eğer formda varsa BitisGunu'nu atıyoruz
                };

                await _context.DenetimGecmisleri.AddAsync(denetimGecmisi);

                // Fotoğraf dosyalarını veritabanına kaydet ve AracDenetimGecmisiFotoVideo ile ilişkilendir
                if (fotoDosyalar != null && fotoDosyalar.Count > 0)
                {
                    foreach (var foto in fotoDosyalar)
                    {
                        if (foto.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await foto.CopyToAsync(memoryStream);
                                var fotoVeri = memoryStream.ToArray();

                                var yeniFoto = new Foto
                                {
                                    DataGuid = Guid.NewGuid(),
                                    Veri = fotoVeri,
                                    DosyaAdi = foto.FileName,
                                    DosyaTuruId = null, // Fotoğraf için belirlenen tür ID
                                    OlusturmaTarihi = DateTime.Now,
                                    OlusturanKullaniciId = olusturanKullaniciId
                                };

                                await _context.Fotolar.AddAsync(yeniFoto);
                                await _context.SaveChangesAsync(); // Foto kaydettikten sonra ID alınır

                                // Fotoğrafı AracDenetimGecmisiFotoVideo tablosuna kaydet
                                var aracDenetimGecmisiFotoVideo = new AracDenetimGecmisiFotoVideo
                                {
                                    DenetimGecmisiId = denetimGecmisi.Id, // İlgili denetim geçmişi ID'si
                                    FotoId = yeniFoto.Id // Kaydedilen fotoğrafın ID'si
                                };

                                await _context.AracDenetimGecmisiFotoVideolari.AddAsync(aracDenetimGecmisiFotoVideo);
                            }
                        }
                    }
                }

                // Video dosyalarını veritabanına kaydet ve AracDenetimGecmisiFotoVideo ile ilişkilendir
                if (videoDosyalar != null && videoDosyalar.Count > 0)
                {
                    foreach (var video in videoDosyalar)
                    {
                        if (video.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await video.CopyToAsync(memoryStream);
                                var videoVeri = memoryStream.ToArray();

                                var yeniVideo = new Video
                                {
                                    DataGuid = Guid.NewGuid(),
                                    Veri = videoVeri,
                                    DosyaAdi = video.FileName,
                                    DosyaTuruId = null, // Video için belirlenen tür ID
                                    OlusturmaTarihi = DateTime.Now,
                                    OlusturanKullaniciId = olusturanKullaniciId
                                };

                                await _context.Videolar.AddAsync(yeniVideo);
                                await _context.SaveChangesAsync(); // Video kaydettikten sonra ID alınır

                                // Videoyu AracDenetimGecmisiFotoVideo tablosuna kaydet
                                var aracDenetimGecmisiVideo = new AracDenetimGecmisiFotoVideo
                                {
                                    DenetimGecmisiId = denetimGecmisi.Id, // İlgili denetim geçmişi ID'si
                                    VideoId = yeniVideo.Id // Kaydedilen videonun ID'si
                                };

                                await _context.AracDenetimGecmisiFotoVideolari.AddAsync(aracDenetimGecmisiVideo);
                            }
                        }
                    }
                }


                // Anahtar kelimelerle eşleşen CezaYonetmelik kayıtlarını bulalım
                if (model.SelectedAnahtarKelimeler != null && model.SelectedAnahtarKelimeler.Any())
                {
                    // CezaYonetmelik tablosundan seçilen anahtar kelimeler ile eşleşenleri buluyoruz
                    var matchingCezaYonetmelik = await _context.CezaYonetmelikleri
                        .Where(cy => model.SelectedAnahtarKelimeler.Contains(cy.AnahtarKelime))
                        .ToListAsync();

                    if (matchingCezaYonetmelik.Any())
                    {
                        // Eşleşen ceza yönetmeliklerinin Id'lerini kontrol edelim ve DenetimCeza tablosuna kaydedelim
                        foreach (var ceza in matchingCezaYonetmelik)
                        {
                            // Eşleşen ceza maddelerini DenetimCeza modeline kaydediyoruz
                            var denetimCeza = new DenetimCeza
                            {
                                DenetimId = denetimId, // Foreign key olarak DenetimId atanıyor
                                CezaYonetmelikId = ceza.Id, // Foreign key olarak CezaYonetmelikId atanıyor

                                // Eğer navigation properties'leri set etmek isterseniz:
                                Denetim = denetim, // Denetim nesnesini de set edebilirsiniz
                                CezaYonetmelik = ceza // CezaYonetmelik nesnesini de set edebilirsiniz
                            };

                            // DenetimCeza kaydını veritabanına ekleyelim
                            _context.DenetimCezalari.Add(denetimCeza);
                        }

                        // Değişiklikleri kaydediyoruz
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Eğer eşleşen ceza yönetmeliği bulunamazsa bir hata mesajı ekleyelim
                        ModelState.AddModelError("", "Eşleşen ceza yönetmeliği bulunamadı.");
                        return View(model);
                    }
                }
                else
                {
                    // Eğer anahtar kelime seçilmediyse ya da eşleşen kelime yoksa hata mesajı ekleyelim
                    ModelState.AddModelError("", "Anahtar kelime seçimi yapılmadı.");
                    return View(model);
                }

                // Başarılı yönlendirme
                return RedirectToAction("Ihlal", "CezaYonetimi");
            }
            catch (Exception ex)
            {
                // Hata durumunda, hata mesajını ve stack trace'i ekle
                ModelState.AddModelError("", "Ceza oluşturulurken bir hata meydana geldi: " + ex.Message);
                ModelState.AddModelError("", ex.StackTrace);
                return View(model); // Hata durumunda formu geri yükle
            }
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {

            var denetim = await _context.Denetimler
                .Include(d => d.AracPlaka) // Araç Plaka bilgisi
                    .ThenInclude(p => p.PlakaTuru) // Plaka Türü bilgisi
                .Include(d => d.DenetimTuru) // Tutanak Türü bilgisi
                .Include(d => d.AracPlaka) // Araç Sahibi bilgisi için
                    .ThenInclude(p => p.AracSahipleri) // AracSahipleri bilgisi
                        .ThenInclude(s => s.PlakaSahip) // Araç sahibinin adı ve telefon bilgisi
                .Include(d => d.DenetimCezalari) // DenetimCezalari ilişkisini dahil ediyoruz
                    .ThenInclude(dc => dc.CezaYonetmelik) // Ceza yönetmelik maddesi ilişkisi
                .Include(d => d.DenetimGecmisleri) // Denetim geçmişi ilişkisini dahil ediyoruz
                    .ThenInclude(g => g.AracDenetimGecmisiFotoVideolari) // Foto ve video ilişkisi
                        .ThenInclude(fv => fv.Foto) // Foto ilişkilendirme
                .Include(d => d.DenetimGecmisleri)
                    .ThenInclude(g => g.AracDenetimGecmisiFotoVideolari)
                        .ThenInclude(fv => fv.Video) // Video ilişkilendirme
                .Include(d => d.OlusturanKullanici) // Denetimi oluşturan kullanıcı bilgisi
                .Include(d => d.CezaDurum) // Ceza Durumu bilgisi
                .FirstOrDefaultAsync(d => d.Id == id); // Tıklanan denetimi getir

            if (denetim == null)
            {
                return NotFound();
            }

            // Kullanıcı bilgilerini manuel olarak Users tablosundan çekiyoruz
            if (denetim.OlusturanKullanici != null)
            {
                var user = await _context.Users
                    .Where(u => u.Id == denetim.OlusturanKullanici.Id)
                    .Select(u => new { u.FullName, u.ProfileImage })
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    ViewBag.FullName = user.FullName;
                    ViewBag.ProfileImage = user.ProfileImage;
                }
            }
            // LookupLists tablosundan LookUpId'si 3 olanların sadece Name alanını çekiyoruz
            var cezaDurumlari = await _context.LookupLists
                .Where(l => l.LookUpId == 3)
                .Select(l => l.Name)  // Sadece Name alanını alıyoruz
                .ToListAsync();

            // ViewBag ile view'e gönderiyoruz
            ViewBag.CezaDurumlari = cezaDurumlari;




            var denetimGecmisi = await (from dg in _context.DenetimGecmisleri
                                        join ll in _context.LookupLists
                                            on dg.CezaDurumId equals ll.Id
                                        join u in _context.Users
                                            on dg.OlusturanKullaniciId equals u.Id
                                        join fv in _context.AracDenetimGecmisiFotoVideolari
                                            on dg.Id equals fv.DenetimGecmisiId into fvGroup
                                        from fv in fvGroup.DefaultIfEmpty()
                                        where dg.DenetimId == id && ll.LookUpId == 3
                                        group new { dg, fv } by new
                                        {
                                            dg.Yorum,
                                            dg.DurumTarihi,
                                            dg.OlusturmaTarihi,
                                            CezaDurumu = ll.Name,
                                            KullaniciAdi = u.FullName,
                                            KullaniciProfilResmi = u.ProfileImage
                                        } into g
                                        select new
                                        {
                                            g.Key.Yorum,
                                            g.Key.DurumTarihi,
                                            g.Key.OlusturmaTarihi,
                                            g.Key.CezaDurumu,
                                            g.Key.KullaniciAdi,
                                            g.Key.KullaniciProfilResmi,
                                            Fotolar = g.Where(x => x.fv.Foto != null).Select(x => Convert.ToBase64String(x.fv.Foto.Veri)).ToList(),
                                            Videolar = g.Where(x => x.fv.Video != null).Select(x => Convert.ToBase64String(x.fv.Video.Veri)).ToList()
                                        }).ToListAsync();

            // ViewBag.DenetimGecmisi = denetimGecmisi;
            // Kontrolörünüzde, DenetimGecmisi listesini tarihe göre artan sırada sıralıyoruz
            ViewBag.DenetimGecmisi = denetimGecmisi
                .OrderBy(item => item.DurumTarihi ?? item.OlusturmaTarihi)
                .ToList();



            return View(denetim);
        }



        [HttpGet]
        public IActionResult GetFoto(int id)
        {
            var foto = _context.Fotolar.FirstOrDefault(f => f.Id == id);
            if (foto != null)
            {
                return File(foto.Veri, "image/jpeg"); // Fotoğrafı binary olarak döndür
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetVideo(int id)
        {
            var video = _context.Videolar.FirstOrDefault(v => v.Id == id);
            if (video != null)
            {
                return File(video.Veri, "video/mp4"); // Videoyu binary olarak döndür
            }
            return NotFound();
        }


        //EDİT İŞLEMLER
        [HttpPost]
        public async Task<IActionResult> AddDenetimGecmisi(int id, string CezaDurumu, string KisaAciklama, DateTime CezaTarihi)
        {
            // Kullanıcının kimliğini alıyoruz
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(); // Eğer kullanıcı kimliği yoksa yetkisiz erişim döndür
            }

            int olusturanKullaniciId = int.Parse(userIdClaim.Value);

            // CezaDurumu'nu CezaDurumId'ye eşleştirmek için LookupList'den alıyoruz
            var cezaDurum = await _context.LookupLists
                .Where(l => l.Name == CezaDurumu && l.LookUpId == 3) // LookUpId'si 3 olan verileri çekiyoruz
                .FirstOrDefaultAsync();

            if (cezaDurum == null)
            {
                ModelState.AddModelError("", "Geçerli bir Ceza Durumu seçilmedi.");
                return RedirectToAction("Edit", new { id });
            }

            try
            {
                // Denetim geçmişine yeni bir kayıt ekliyoruz
                var denetimGecmisi = new DenetimGecmisi
                {
                    DenetimId = id, // İlgili denetim ID'si
                    CezaDurumId = cezaDurum.Id, // LookupList'den gelen CezaDurumId
                    Yorum = KisaAciklama, // Formdan gelen kısa açıklama
                    DurumTarihi = CezaTarihi, // Formdan gelen tarih
                    OlusturanKullaniciId = olusturanKullaniciId, // Şu anki kullanıcı
                    UyariSuresi = null // İsteğe bağlı olarak uyarı süresi ekleyebilirsiniz
                };

                // Denetim geçmişini veritabanına kaydediyoruz
                _context.DenetimGecmisleri.Add(denetimGecmisi);
                await _context.SaveChangesAsync();


                // İlgili denetimi getiriyoruz
                var denetim = await _context.Denetimler
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (denetim == null)
                {
                    ModelState.AddModelError("", "Denetim bulunamadı.");
                    return RedirectToAction("Edit", new { id });
                }

                // Denetimin CezaDurumId'sini güncelliyoruz
                denetim.CezaDurumId = cezaDurum.Id;

                // Değişiklikleri veritabanına kaydediyoruz
                await _context.SaveChangesAsync();


                // Başarılı yönlendirme
                return RedirectToAction("Edit", new { id });
            }
            catch (Exception ex)
            {
                // Hata mesajı ekliyoruz
                ModelState.AddModelError("", "Denetim geçmişi eklenirken bir hata meydana geldi: " + ex.Message);
                return RedirectToAction("Edit", new { id });
            }
        }







    }
}