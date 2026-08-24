using CRUD_Application.Models;

namespace CRUD_Application.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User> AddAsync(User user);
    }
}
