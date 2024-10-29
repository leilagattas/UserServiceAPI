using System.Linq.Expressions;
using UserServiceAPI.Domain.Models;

namespace UserServiceAPI.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllActive();
        Task<bool> Add(User user);
        Task<bool> Remove(Guid id);
        Task<User> UpdateActive(Guid id);
    }
}
