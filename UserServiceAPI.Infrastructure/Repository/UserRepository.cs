using Microsoft.EntityFrameworkCore;
using UserServiceAPI.Common.Models;
using UserServiceAPI.Domain.Models;
using UserServiceAPI.Infrastructure.Interfaces;

namespace UserServiceAPI.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> dbSet;

        public UserRepository(
            UserDbContext context)
        {
            dbSet = context.Set<User>();
        }
        public async Task<IEnumerable<User>> GetAllActive()
        {
            return await dbSet.Where(x => x.Active).ToListAsync();
        }
        public async Task<bool> Add(User user)
        {
            try
            {
                await dbSet.AddAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                throw new DBErrorException(ex.Message);
            }
        }
        public async Task<bool> Remove(Guid id)
        {
            var user = await FindUserOrThrow(id);
            dbSet.Remove(user);
            return true;
        }
        public async Task<User> UpdateActive(Guid id)
        {
            var user = await FindUserOrThrow(id);
            user.Active = !user.Active;
            dbSet.Update(user);
            return user;
        }

        private async Task<User> FindUserOrThrow(Guid id)
        {
            return await dbSet.FindAsync(id)
                   ?? throw new KeyNotFoundException($"The user with id:{id} was not found in the database");
        }
    }
}
