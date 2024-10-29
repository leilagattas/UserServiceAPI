using UserServiceAPI.Common.Models;
using UserServiceAPI.Infrastructure.Interfaces;

namespace UserServiceAPI.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserDbContext _context;
        public IUserRepository Users { get; }

        public UnitOfWork(UserDbContext dbContext,
            IUserRepository userRepository)
        {
            _context = dbContext;
            Users = userRepository;
        }
        public async Task<int> CompleteAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DBErrorException(ex.Message);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
