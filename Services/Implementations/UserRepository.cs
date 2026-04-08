using Microsoft.EntityFrameworkCore;
using ClaimCare.Data;
using ClaimCare.Models;
using ClaimCare.Services.Interfaces;
using ClaimCare.DTOs.PaginationDTO;

namespace ClaimCare.Services.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ClaimCareDbContext _context;

        public UserRepository(ClaimCareDbContext context)
        {
            _context = context;
        }

       
        public async Task<IEnumerable<User>> GetAllUsers(PaginationParams pagination)
        {
            var users = await _context.Users
                .Include(u => u.Role) 
                .AsNoTracking()
                .OrderBy(u => u.UserId)
                .ToListAsync();

            return users
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize);
        }

       
        public async Task<IEnumerable<User>> GetAllPatients(PaginationParams pagination)
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.RoleName == "Patient")
                .AsNoTracking()
                .OrderBy(u => u.UserId)
                .ToListAsync();

            return users
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize);
        }

       
        public async Task<IEnumerable<User>> GetAllProviders(PaginationParams pagination)
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.RoleName == "HealthcareProvider")
                .AsNoTracking()
                .OrderBy(u => u.UserId)
                .ToListAsync();

            return users
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize);
        }

      
        public async Task<IEnumerable<User>> GetAllInsuranceCompanies(PaginationParams pagination)
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.RoleName == "InsuranceCompany")
                .AsNoTracking()
                .OrderBy(u => u.UserId)
                .ToListAsync();

            return users
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize);
        }

       
        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

       
        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}