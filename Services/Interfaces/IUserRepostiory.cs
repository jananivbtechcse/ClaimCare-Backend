using ClaimCare.DTOs.PaginationDTO;
using ClaimCare.Models;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers(PaginationParams pagination);
    Task<IEnumerable<User>> GetAllPatients(PaginationParams pagination);
    Task<IEnumerable<User>> GetAllProviders(PaginationParams pagination);
    Task<IEnumerable<User>> GetAllInsuranceCompanies(PaginationParams pagination);

    Task<User?> GetUserById(int id);
    Task DeleteUser(int id);
}