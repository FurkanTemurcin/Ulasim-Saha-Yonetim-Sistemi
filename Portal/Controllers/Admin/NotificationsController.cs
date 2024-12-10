using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal.Models.Admin;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Portal.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllNotifications")]
public async Task<IActionResult> GetAllNotifications()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (userId == null)
    {
        return Unauthorized();
    }

    var notifications = await _context.Notifications
        .Where(n => n.UserId == int.Parse(userId))
        .OrderByDescending(n => n.CreatedAt)
        .ToListAsync();

    return Ok(notifications);
}



        [HttpGet("GetUnreadNotificationCount")]
public async Task<IActionResult> GetUnreadNotificationCount()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (userId == null)
    {
        return Unauthorized();
    }

    var unreadCount = await _context.Notifications
        .Where(n => n.UserId == int.Parse(userId) && !n.IsRead)
        .CountAsync();

    return Ok(unreadCount);
}


[HttpGet("GetNotifications")]
public async Task<IActionResult> GetNotifications()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (userId == null)
    {
        return Unauthorized();
    }

    // Kullanıcının okunmamış bildirimlerini al
    var notifications = await _context.Notifications
        .Where(n => n.UserId == int.Parse(userId) && !n.IsRead)
        .OrderByDescending(n => n.CreatedAt)
        .ToListAsync();

    // Bildirimleri okundu olarak işaretle
    foreach (var notification in notifications)
    {
        notification.IsRead = true;
    }
    await _context.SaveChangesAsync();

    return Ok(notifications);
}


        [HttpPost("MarkAsRead")]
        public async Task<IActionResult> MarkAsRead()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var notifications = await _context.Notifications
                .Where(n => n.UserId == int.Parse(userId) && !n.IsRead)
                .ToListAsync();

            notifications.ForEach(n => n.IsRead = true);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
