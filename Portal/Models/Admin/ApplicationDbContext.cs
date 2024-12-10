using Microsoft.EntityFrameworkCore;
using Portal.Models.USYS;

namespace Portal.Models.Admin
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Menu> Menus { get; set; }

        // public DbSet<Personel> Personeller { get; set; }
        public DbSet<Birim> Birimler { get; set; }
        public DbSet<Mudurluk> Mudurlukler { get; set; }
        public DbSet<Sube> Subeler { get; set; }
        public DbSet<KanGrubu> KanGrubus { get; set; }
        public DbSet<AracDenetim> AracDenetimleri { get; set; }
        public DbSet<Denetim> Denetimler { get; set; }
        public DbSet<AracDenetimGecmis> AracDenetimGecmisleri { get; set; }
        public DbSet<AracDenetimGecmisiFotoVideo> AracDenetimGecmisiFotoVideolari { get; set; }

        // Araç ve İlgili Tablolar
        public DbSet<AracPlaka> AracPlakalari { get; set; }
        public DbSet<AracSahibi> AracSahipleri { get; set; }

        public DbSet<PlakaSahip> PlakaSahipleri { get; set; }
        public DbSet<UcretTakvim> UcretTakvimleri { get; set; }


        public DbSet<DenetimCeza> DenetimCezalari { get; set; } // Denetim ve Ceza ilişkisini tutan tablo
        public DbSet<CezaYonetmelik> CezaYonetmelikleri { get; set; }
        // Fotoğraf ve İlgili Tablolar
        public DbSet<Foto> Fotolar { get; set; }
        public DbSet<Video> Videolar { get; set; }

        public DbSet<Guzergah> Guzergahlar { get; set; }
        public DbSet<ZamanCizelge> ZamanCizelgeleri { get; set; }

        // Lookup Tabloları
        public DbSet<LookUp> Lookups { get; set; }
        public DbSet<LookupList> LookupLists { get; set; }
        public DbSet<DenetimGecmisi> DenetimGecmisleri { get; set; } // Denetim Gecmisi tablosu
        public DbSet<PlakaCezaGenel> PlakaCezaGenel { get; set; }

        public DbSet<Notification> Notifications { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Geometri ilişkisini görmezden gel


            // RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete



            // User - Sube Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Sube)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.SubeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Birim - Mudurluk Relationship
            modelBuilder.Entity<Birim>()
                .HasMany(b => b.Mudurlukler)
                .WithOne(m => m.Birim)
                .HasForeignKey(m => m.BirimId)
                .OnDelete(DeleteBehavior.Cascade);

            // Mudurluk - Sube Relationship
            modelBuilder.Entity<Mudurluk>()
                .HasMany(m => m.Subeler)
                .WithOne(s => s.Mudurluk)
                .HasForeignKey(s => s.MudurlukId)
                .OnDelete(DeleteBehavior.Cascade);
            // User ve KanGrubu arasındaki Foreign Key ilişkisi






            // AracDenetim ve Kullanıcı İlişkisi
            modelBuilder.Entity<AracDenetim>()
                .HasOne(ad => ad.Kullanici)
                .WithMany(u => u.AracDenetimleri)
                .HasForeignKey(ad => ad.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // AracDenetim ve AracPlaka İlişkisi
            modelBuilder.Entity<AracDenetim>()
                .HasOne(ad => ad.AracPlaka)
                .WithMany(ap => ap.AracDenetimleri)
                .HasForeignKey(ad => ad.AracPlakaId)
                .OnDelete(DeleteBehavior.Restrict);
            // User ve AracDenetim İlişkisi
            modelBuilder.Entity<User>()
                .HasMany(u => u.AracDenetimleri)
                .WithOne(ad => ad.Kullanici)
                .HasForeignKey(ad => ad.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AracDenetim>()
.HasOne(ad => ad.AracPlaka)
.WithMany(ap => ap.AracDenetimleri)
.HasForeignKey(ad => ad.AracPlakaId)
.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AracDenetim>()
                .HasOne(ad => ad.Kullanici)
                .WithMany(u => u.AracDenetimleri)
                .HasForeignKey(ad => ad.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // AracDenetimGecmis modeli için ModelBuilder konfigürasyonu
            modelBuilder.Entity<AracDenetimGecmis>()
                .HasOne(adg => adg.AracDenetim)
                .WithMany(ad => ad.AracDenetimGecmisleri)
                .HasForeignKey(adg => adg.AracDenetimId)
                .OnDelete(DeleteBehavior.Cascade); // AracDenetim silindiğinde, ilişkili denetim geçmişi de silinsin

            modelBuilder.Entity<AracDenetimGecmis>()
                .HasOne(adg => adg.KontrolTuru)
                .WithMany()
                .HasForeignKey(adg => adg.KontrolTuruId)
                .OnDelete(DeleteBehavior.Restrict); // Kontrol türü silindiğinde denetim geçmişi silinmesin

            // User ve AracDenetim ile DenetimGecmisi arasındaki ilişkiler
            modelBuilder.Entity<User>()
                .HasMany(u => u.AracDenetimleri)
                .WithOne(ad => ad.Kullanici)
                .HasForeignKey(ad => ad.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse denetim bilgisi etkilenmez


            modelBuilder.Entity<User>()
                .HasMany(u => u.DenetimGecmisleri)
                .WithOne(dg => dg.OlusturanKullanici)
                .HasForeignKey(dg => dg.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.SetNull); // Kullanıcı silinirse denetim geçmişi bilgisi null olur


            modelBuilder.Entity<UcretTakvim>()
                .Property(ut => ut.Ucret)
                .HasColumnType("decimal(18,2)"); // 18 basamaklı, 2 ondalık haneli bir decimal tipi tanımla
                                                 // // AracPlaka ve AracSahibi İlişkisi
                                                 // modelBuilder.Entity<AracSahibi>()
                                                 //     .HasOne(as => as.AracPlaka) // AracSahibi'nin AracPlaka ile ilişkisi
                                                 //     .WithMany(ap => ap.AracSahipleri) // AracPlaka'nın AracSahipleri ile ilişkisi
                                                 //     .HasForeignKey(as => as.PlakaId) // ForeignKey olarak PlakaId kullanılıyor
                                                 //     .OnDelete(DeleteBehavior.Restrict); // Plaka silindiğinde araç sahipliği etkilenmesin

            // // AracSahibi ve PlakaSahip İlişkisi
            // modelBuilder.Entity<AracSahibi>()
            //     .HasOne(as => as.PlakaSahip) // AracSahibi'nin PlakaSahip ile ilişkisi
            //     .WithMany(ps => ps.AracSahipleri) // PlakaSahip'in AracSahipleri ile ilişkisi
            //     .HasForeignKey(as => as.PlakaSahipId) // ForeignKey olarak PlakaSahipId kullanılıyor
            //     .OnDelete(DeleteBehavior.Restrict); // Araç sahibi silindiğinde araç sahipliği etkilenmesin


            // ModelBuilder Konfigürasyonu
            modelBuilder.Entity<AracPlaka>()
                .HasOne(ap => ap.PlakaTuru)
                .WithMany()
                .HasForeignKey(ap => ap.PlakaTuruId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AracPlaka>()
                .HasOne(ap => ap.Guzergah)
                .WithMany()
                .HasForeignKey(ap => ap.GuzergahId)
                .OnDelete(DeleteBehavior.SetNull);

            // ModelBuilder Konfigürasyonu
            modelBuilder.Entity<User>()
                .HasMany(u => u.AracDenetimleri)
                .WithOne(ad => ad.Kullanici)
                .HasForeignKey(ad => ad.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict);


            // ModelBuilder Konfigürasyonu
            modelBuilder.Entity<LookupList>()
                .HasMany(ll => ll.AracPlakalari)
                .WithOne(ap => ap.PlakaTuru)
                .HasForeignKey(ap => ap.PlakaTuruId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LookupList>()
                .HasMany(ll => ll.CezaYonetmelikleri)
                .WithOne(cy => cy.PlakaTuru)
                .HasForeignKey(cy => cy.PlakaTuruId)
                .OnDelete(DeleteBehavior.Restrict);


            // ModelBuilder Konfigürasyonu
            modelBuilder.Entity<CezaYonetmelik>()
                .HasOne(cy => cy.PlakaTuru)
                .WithMany(l => l.CezaYonetmelikleri)
                .HasForeignKey(cy => cy.PlakaTuruId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CezaYonetmelik>()
                .HasOne(cy => cy.OlusturanKullanici)
                .WithMany(u => u.CezaYonetmelikleri)
                .HasForeignKey(cy => cy.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // ModelBuilder Konfigürasyonu
            modelBuilder.Entity<User>()
                .HasMany(u => u.CezaYonetmelikleri)
                .WithOne(cy => cy.OlusturanKullanici)
                .HasForeignKey(cy => cy.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.AracDenetimleri)
                .WithOne(ad => ad.Kullanici)
                .HasForeignKey(ad => ad.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // ModelBuilder Konfigürasyonu
            modelBuilder.Entity<LookupList>()
                .HasMany(ll => ll.CezaYonetmelikleri)
                .WithOne(cy => cy.PlakaTuru)
                .HasForeignKey(cy => cy.PlakaTuruId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LookupList>()
                .HasMany(ll => ll.AracPlakalari)
                .WithOne(ap => ap.PlakaTuru)
                .HasForeignKey(ap => ap.PlakaTuruId)
                .OnDelete(DeleteBehavior.Restrict);


            // User ve Denetim İlişkisi
            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.OlusturanKullanici)   // Denetimi oluşturan kullanıcı ile ilişki
                .WithMany(u => u.Denetimler)         // Kullanıcının oluşturduğu denetim listesi
                .HasForeignKey(d => d.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);  // Kullanıcı silinirse denetim bilgisi etkilenmez

            // Denetim ve LookupList İlişkisi (CezaDurum)
            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.CezaDurum)             // Ceza durumu bilgisi
                .WithMany()                           // LookupList ile direkt ilişki
                .HasForeignKey(d => d.CezaDurumId)    // LookupList ID
                .OnDelete(DeleteBehavior.Restrict);   // Ceza durumu LookupList'ten silinirse denetim silinmez

            // Denetim ve LookupList İlişkisi (DenetimTuru)
            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.DenetimTuru)           // Denetim türü bilgisi
                .WithMany()                           // LookupList ile direkt ilişki
                .HasForeignKey(d => d.DenetimTuruId)  // LookupList ID
                .OnDelete(DeleteBehavior.Restrict);   // Denetim türü LookupList'ten silinirse denetim silinmez

            // Denetim ve LookupList İlişkisi (CezaDelilTip)
            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.CezaDelilTip)          // Ceza delil tipi bilgisi
                .WithMany()                           // LookupList ile direkt ilişki
                .HasForeignKey(d => d.CezaDelilTipId) // LookupList ID
                .OnDelete(DeleteBehavior.Restrict);   // Ceza delil tipi LookupList'ten silinirse denetim silinmez

            // Denetim ve AracPlaka İlişkisi
            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.AracPlaka)             // Denetimle ilişkilendirilen araç plaka bilgisi
                .WithMany()                           // Araç plakası ile direkt ilişki
                .HasForeignKey(d => d.AracPlakaId)    // AracPlaka ID
                .OnDelete(DeleteBehavior.Restrict);   // Araç plakası silindiğinde denetim bilgisi silinmez
                                                      // User ve Denetim İlişkisi
            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.OlusturanKullanici)
                .WithMany(u => u.Denetimler)
                .HasForeignKey(d => d.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse denetimler silinmez

            // User ve CezaYonetmelik İlişkisi
            modelBuilder.Entity<CezaYonetmelik>()
                .HasOne(c => c.OlusturanKullanici)
                .WithMany(u => u.CezaYonetmelikleri)
                .HasForeignKey(c => c.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse ceza yönetmelikleri silinmez


            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.CezaDurum)
                .WithMany()
                .HasForeignKey(d => d.CezaDurumId)
                .OnDelete(DeleteBehavior.Restrict); // LookupList silinirse denetim silinmez

            // Denetim ve LookupList İlişkisi (DenetimTuru)
            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.DenetimTuru)
                .WithMany()
                .HasForeignKey(d => d.DenetimTuruId)
                .OnDelete(DeleteBehavior.Restrict); // LookupList silinirse denetim silinmez

            // Denetim ve LookupList İlişkisi (CezaDelilTip)
            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.CezaDelilTip)
                .WithMany()
                .HasForeignKey(d => d.CezaDelilTipId)
                .OnDelete(DeleteBehavior.Restrict); // LookupList silinirse denetim silinmez

            // User ve DenetimGecmisi İlişkisi
            modelBuilder.Entity<DenetimGecmisi>()
                .HasOne(dg => dg.OlusturanKullanici)
                .WithMany(u => u.DenetimGecmisleri)
                .HasForeignKey(dg => dg.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silindiğinde denetim geçmişi bilgisi etkilenmez

            // User ve Denetim İlişkisi
            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.OlusturanKullanici)
                .WithMany(u => u.Denetimler)
                .HasForeignKey(d => d.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse denetim bilgisi silinmez

            // Denetim ve LookupList (CezaDurum) İlişkisi
            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.CezaDurum)
                .WithMany()
                .HasForeignKey(d => d.CezaDurumId)
                .OnDelete(DeleteBehavior.Restrict); // Ceza Durum silinirse Denetim bilgisi silinmez

            // Denetim ve LookupList (DenetimTuru) İlişkisi
            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.DenetimTuru)
                .WithMany()
                .HasForeignKey(d => d.DenetimTuruId)
                .OnDelete(DeleteBehavior.Restrict); // Denetim Türü silinirse Denetim bilgisi silinmez

            // Denetim ve LookupList (CezaDelilTip) İlişkisi
            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.CezaDelilTip)
                .WithMany()
                .HasForeignKey(d => d.CezaDelilTipId)
                .OnDelete(DeleteBehavior.Restrict); // Ceza Delil Tip silinirse Denetim bilgisi silinmez

            // DenetimGecmisi ve LookupList (CezaDurum) İlişkisi
            modelBuilder.Entity<DenetimGecmisi>()
                .HasOne(dg => dg.CezaDurum)
                .WithMany()
                .HasForeignKey(dg => dg.CezaDurumId)
                .OnDelete(DeleteBehavior.Restrict); // Ceza Durum silinirse Denetim Gecmisi bilgisi silinmez

            // DenetimGecmisi ve Denetim İlişkisi
            modelBuilder.Entity<DenetimGecmisi>()
                .HasOne(dg => dg.Denetim)
                .WithMany(d => d.DenetimGecmisleri)
                .HasForeignKey(dg => dg.DenetimId)
                .OnDelete(DeleteBehavior.Cascade); // Denetim silinirse, ilgili Denetim Gecmisi bilgileri de silinir

            // LookupList ve AracPlaka İlişkisi
            modelBuilder.Entity<AracPlaka>()
                .HasOne(ap => ap.PlakaTuru)
                .WithMany(ll => ll.AracPlakalari)
                .HasForeignKey(ap => ap.PlakaTuruId)
                .OnDelete(DeleteBehavior.Restrict); // Plaka Türü silindiğinde araç plaka bilgisi etkilenmez
                                                    // DenetimGecmisi ve Denetim İlişkisi
            modelBuilder.Entity<DenetimGecmisi>()
                .HasOne(dg => dg.Denetim)
                .WithMany(d => d.DenetimGecmisleri)
                .HasForeignKey(dg => dg.DenetimId)
                .OnDelete(DeleteBehavior.Cascade); // Denetim silindiğinde, ilgili Denetim Gecmisi bilgileri de silinir

            // DenetimGecmisi ve Kullanıcı (OlusturanKullanici) İlişkisi
            modelBuilder.Entity<DenetimGecmisi>()
                .HasOne(dg => dg.OlusturanKullanici)
                .WithMany(u => u.DenetimGecmisleri)
                .HasForeignKey(dg => dg.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silindiğinde denetim geçmişi bilgisi etkilenmez

            // DenetimGecmisi ve LookupList (CezaDurum) İlişkisi
            modelBuilder.Entity<DenetimGecmisi>()
                .HasOne(dg => dg.CezaDurum)
                .WithMany()
                .HasForeignKey(dg => dg.CezaDurumId)
                .OnDelete(DeleteBehavior.Restrict); // Ceza Durum silinirse Denetim Gecmisi bilgisi silinmez






            modelBuilder.Entity<CezaYonetmelik>()
                .HasOne(cy => cy.PlakaTuru)
                .WithMany(ll => ll.CezaYonetmelikleri)
                .HasForeignKey(cy => cy.PlakaTuruId)
                .OnDelete(DeleteBehavior.Restrict); // Plaka Türü silindiğinde ceza yönetmelikleri silinmez

            // LookupList ve AracPlaka İlişkisi
            modelBuilder.Entity<AracPlaka>()
                .HasOne(ap => ap.PlakaTuru)
                .WithMany(ll => ll.AracPlakalari)
                .HasForeignKey(ap => ap.PlakaTuruId)
                .OnDelete(DeleteBehavior.Restrict); // Plaka Türü silindiğinde araç plaka bilgisi etkilenmez

            // User ve Denetim İlişkisi
            modelBuilder.Entity<Denetim>()
                .HasOne(d => d.OlusturanKullanici)
                .WithMany(u => u.Denetimler)
                .HasForeignKey(d => d.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse denetim bilgisi silinmez

            // User ve CezaYonetmelik İlişkisi
            modelBuilder.Entity<CezaYonetmelik>()
                .HasOne(cy => cy.OlusturanKullanici)
                .WithMany(u => u.CezaYonetmelikleri)
                .HasForeignKey(cy => cy.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse ceza yönetmelik bilgisi etkilenmez

            // Kullanıcı ve Foto İlişkisi
            modelBuilder.Entity<Foto>()
                .HasOne(f => f.OlusturanKullanici)
                .WithMany(u => u.Fotolar)
                .HasForeignKey(f => f.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silindiğinde fotoğrafın oluşturan bilgisi null olur


            // ModelBuilder ile ilişki yapılandırmaları burada tanımlanacak.
            modelBuilder.Entity<DenetimGecmisi>()
                .HasOne(dg => dg.Denetim)
                .WithMany(d => d.DenetimGecmisleri)
                .HasForeignKey(dg => dg.DenetimId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DenetimGecmisi>()
                .HasOne(dg => dg.OlusturanKullanici)
                .WithMany(u => u.DenetimGecmisleri)
                .HasForeignKey(dg => dg.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DenetimGecmisi>()
                .HasOne(dg => dg.CezaDurum)
                .WithMany()
                .HasForeignKey(dg => dg.CezaDurumId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Foto>()
                .HasOne(f => f.OlusturanKullanici)
                .WithMany(u => u.Fotolar)
                .HasForeignKey(f => f.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);


            // Güzergah ve İlçe (LookupList) İlişkisi

            // Guzergah ve Ilce ilişkisi
            modelBuilder.Entity<Guzergah>()
                .HasOne(g => g.Ilce)
                .WithMany(l => l.IlceGuzergahlar) // LookupList'teki IlceGuzergahlar ile ilişki
                .HasForeignKey(g => g.IlceId)
                .OnDelete(DeleteBehavior.Restrict); // İlçe silindiğinde güzergahlar etkilenmesin

            // Guzergah ve Geometri ilişkisi
            modelBuilder.Entity<Guzergah>()
                .HasOne(g => g.Geometri)
                .WithMany(l => l.GeometriGuzergahlar) // LookupList'teki GeometriGuzergahlar ile ilişki
                .HasForeignKey(g => g.GeometriId)
                .OnDelete(DeleteBehavior.Restrict); // Geometri bilgisi silindiğinde güzergahlar etkilenmesin

            // Güzergah ve Geometri (LookupList) İlişkisi
            modelBuilder.Entity<Guzergah>()
                .HasOne(g => g.Geometri)
                .WithMany()
                .HasForeignKey(g => g.GeometriId)
                .OnDelete(DeleteBehavior.Restrict); // Geometri bilgisi silindiğinde güzergahlar etkilenmesin

            // Güzergah ve GuzergahZamanCizelge İlişkisi
            modelBuilder.Entity<Guzergah>()
                .HasMany(g => g.ZamanCizelgeleri)
                .WithOne(z => z.Guzergah)
                .HasForeignKey(z => z.GuzergahId)
                .OnDelete(DeleteBehavior.Cascade); // Güzergah silindiğinde zaman çizelgeleri de silinsin

            // GuzergahZamanCizelge ve YerTuru (LookupList) İlişkisi
            modelBuilder.Entity<ZamanCizelge>()
                .HasOne(z => z.YerTuru)
                .WithMany()
                .HasForeignKey(z => z.YerTuruId)
                .OnDelete(DeleteBehavior.Restrict); // Yer türü silindiğinde zaman çizelgeleri etkilenmesin

            // GuzergahZamanCizelge ve Gun (LookupList) İlişkisi
            modelBuilder.Entity<ZamanCizelge>()
                .HasOne(z => z.Gun)
                .WithMany()
                .HasForeignKey(z => z.GunId)
                .OnDelete(DeleteBehavior.Restrict); // Gün bilgisi silindiğinde zaman çizelgeleri etkilenmesin

            // PlakaSahip ve Cinsiyet İlişkisi (LookupList ile)
            modelBuilder.Entity<PlakaSahip>()
                .HasOne(ps => ps.Cinsiyet)
                .WithMany()
                .HasForeignKey(ps => ps.CinsiyetId)
                .OnDelete(DeleteBehavior.Restrict); // Cinsiyet bilgisi silindiğinde PlakaSahip etkilenmesin

            // PlakaSahip ve User İlişkisi (OlusturanKullanici)
            modelBuilder.Entity<PlakaSahip>()
                .HasOne(ps => ps.OlusturanKullanici)
                .WithMany(u => u.PlakaSahipleri) // Kullanıcıdan PlakaSahip ile ilişki
                .HasForeignKey(ps => ps.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.SetNull); // Kullanıcı silindiğinde oluşturma bilgisi null olur


            // UcretTakvim ve Guzergah İlişkisi
            modelBuilder.Entity<UcretTakvim>()
                .HasOne(ut => ut.Guzergah)
                .WithMany(g => g.UcretTakvimleri)
                .HasForeignKey(ut => ut.GuzergahId)
                .OnDelete(DeleteBehavior.Restrict); // Güzergah silindiğinde ücret takvimi etkilenmesin

            // Guzergah ve Ilce İlişkisi

            // Ilce ve Guzergah ilişkisi
            modelBuilder.Entity<Guzergah>()
                .HasOne(g => g.Ilce)
                .WithMany(l => l.IlceGuzergahlar)
                .HasForeignKey(g => g.IlceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Geometri ve Guzergah ilişkisi
            modelBuilder.Entity<Guzergah>()
                .HasOne(g => g.Geometri)
                .WithMany(l => l.GeometriGuzergahlar)
                .HasForeignKey(g => g.GeometriId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DenetimCeza>()
               .HasOne(dc => dc.CezaYonetmelik)
               .WithMany(cy => cy.DenetimCezalari)
               .HasForeignKey(dc => dc.CezaYonetmelikId)
               .OnDelete(DeleteBehavior.Cascade);  // Cascade Delete özelliğini etkinleştiriyoruz





        }

    }
}