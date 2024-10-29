using System.Linq.Expressions;
using UserServiceAPI.Domain.Models;

namespace UserServiceAPI.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        Task<int> CompleteAsync();
    }
}
