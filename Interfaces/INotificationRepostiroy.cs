using ClaimCare.Models;

namespace ClaimCare.Services.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetUserNotifications(int userId);
        Task<Notification> CreateNotification(Notification notification);
        Task MarkAsRead(int notificationId);
    }
}