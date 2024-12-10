using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Portal.Services;

namespace Portal.Services
{
    public class DailyNotificationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _scheduledTime = new TimeSpan(10, 00, 0); // Her gün 06:00'da çalışır

        public DailyNotificationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentTime = DateTime.Now.TimeOfDay;
                var delay = _scheduledTime - currentTime;
                if (delay < TimeSpan.Zero)
                {
                    delay = TimeSpan.FromDays(1) + delay;
                }

                await Task.Delay(delay, stoppingToken); // Belirtilen saate kadar bekler

                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
                    await notificationService.CheckAracPlakaVizeAndNotifyAdmins(); // Vize kontrolünü yap
                        await notificationService.CheckAracYasiAndNotifyAdmins(); // Araç yaşını kontrol et
                          await notificationService.CheckDenetimUyariSuresiAndNotifyAdmins(); // Araç yaşını kontrol et

                        

                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // Bir sonraki gün aynı saatte çalışmak için bekler
            }
        }
    }
}
