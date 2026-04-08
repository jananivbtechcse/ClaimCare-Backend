using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClaimCare.Data;
using ClaimCare.DTOs.NotificationDTO;
using ClaimCare.DTOs.PaginationDTO;
using System.Security.Claims;
using ClaimCare.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ClaimCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ClaimCareDbContext _context;
        private readonly IMapper _mapper;

        public NotificationController(ClaimCareDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

          
       [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            var result = _mapper.Map<List<NotificationResponseDTO>>(notifications);

            return Ok(result);
        }

          
      
        [HttpPut("read/{id}")]
        [Authorize]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);

            if (notification == null)
                return NotFound();

            notification.IsEmailSent = true;

            await _context.SaveChangesAsync();

            return Ok("Notification marked as read");
        }
        [HttpGet("unread-count")]
        [Authorize]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var count = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsEmailSent)
                .CountAsync();

            return Ok(new { unread = count });
        }
                
        
          
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllNotifications([FromQuery] PaginationParams pagination)
        {
            var notifications = await _context.Notifications
    .Include(n => n.User)
    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
    .Take(pagination.PageSize)
    .ToListAsync();

            return Ok(notifications);
        }
    }
}