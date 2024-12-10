using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Portal.Hubs;
using Portal.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Services
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task CheckAracPlakaVizeAndNotifyAdmins()
        {
            var today = DateTime.Today;

            // Vize tarihi geçen araçları al
            var gecikmisAracPlakalari = await _context.AracPlakalari
                .Where(ap => ap.VizeTarihi.HasValue && ap.VizeTarihi.Value < today)
                .ToListAsync();

            foreach (var aracPlaka in gecikmisAracPlakalari)
            {
                string message = $" {aracPlaka.PlakaNumarasi} plakalı aracın vize süresi dolmuş durumda.";
                await SendNotificationToAdminsAsync(message);
            }
        }

     public async Task CheckAracYasiAndNotifyAdmins()
{
    var currentYear = DateTime.Today.Year;

    // Tüm araç plakalarını al ve sonrasında kontrol et
    var aracPlakalari = await _context.AracPlakalari.ToListAsync();

    // 10 yaşından büyük araçları kontrol et
    foreach (var aracPlaka in aracPlakalari)
    {
        if (int.TryParse(aracPlaka.Model, out int modelYear))
        {
            var aracYasi = currentYear - modelYear;
            if (aracYasi > 10)
            {
                string message = $" {aracPlaka.PlakaNumarasi} plakalı araç 10 yaşını geçmiştir.";
                await SendNotificationToAdminsAsync(message);
            }
        }
    }
}

public async Task CheckDenetimUyariSuresiAndNotifyAdmins()
{
    var today = DateTime.Today;

    // Uyarı süresi dolmuş denetimleri al
    var uyariSuresiDolmusDenetimler = await _context.DenetimGecmisleri
        .Where(dg => dg.UyariSuresi.HasValue && today >= dg.OlusturmaTarihi.AddDays(dg.UyariSuresi.Value))
        .ToListAsync();

    foreach (var denetimGecmisi in uyariSuresiDolmusDenetimler)
    {
        // Denetim kaydını al
        var denetim = await _context.Denetimler
            .Where(d => d.Id == denetimGecmisi.DenetimId)
            .FirstOrDefaultAsync();

        if (denetim != null)
        {
            // AracPlaka kaydını al
            var aracPlaka = await _context.AracPlakalari
                .Where(ap => ap.Id == denetim.AracPlakaId)
                .FirstOrDefaultAsync();

            if (aracPlaka != null)
            {
                // Bildirim mesajını oluştur
                string message = $" {aracPlaka.PlakaNumarasi} plakalı aracın uyarı süresi dolmuştur.";
                await SendNotificationToAdminsAsync(message);
            }
        }
    }
}


public async Task SendNotificationToAdminsAsync(string message)
{
    var adminUserIds = await _context.UserRoles
        .Where(ur => ur.RoleId == 4) // Admin rol ID'sini doğrulayın admin yerine USYSYoneticiye gönderdik bildirimleri
        .Select(ur => ur.UserId)
        .Distinct()
        .ToListAsync();

    foreach (var adminUserId in adminUserIds)
    {
        // Aynı kullanıcıya aynı mesaj zaten gönderilmiş mi kontrol et
        var existingNotification = await _context.Notifications
            .AnyAsync(n => n.UserId == adminUserId && n.Message == message);

        if (!existingNotification) // Eğer aynı mesaj yoksa, ekle
        {
            var notification = new Notification
            {
                UserId = adminUserId,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(adminUserId.ToString()).SendAsync("ReceiveNotification", message);
        }
    }
}



        // public async Task SendNotificationToAdminsAsync(string message)
        // {
        //     var adminUserIds = await _context.UserRoles
        //         .Where(ur => ur.RoleId == 1) // Admin rol ID'sini doğrulayın
        //         .Select(ur => ur.UserId)
        //         .Distinct()
        //         .ToListAsync();

        //     foreach (var adminUserId in adminUserIds)
        //     {
        //         var notification = new Notification
        //         {
        //             UserId = adminUserId,
        //             Message = message,
        //             CreatedAt = DateTime.UtcNow
        //         };

        //         _context.Notifications.Add(notification);
        //         await _context.SaveChangesAsync();

        //         await _hubContext.Clients.User(adminUserId.ToString()).SendAsync("ReceiveNotification", message);
        //     }
        // }
    }
}
