using Microsoft.EntityFrameworkCore;
using ClaimCare.Data;
using ClaimCare.Models;
using ClaimCare.Services.Interfaces;

namespace ClaimCare.Services.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ClaimCareDbContext _context;

        public NotificationRepository(ClaimCareDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetUserNotifications(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }

        public async Task<Notification> CreateNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task MarkAsRead(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);

            if (notification != null)
            {
                notification.IsEmailSent = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}