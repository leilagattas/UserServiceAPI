using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceAPI.Domain.Models;

namespace UserServiceAPI.Infrastructure
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> User { get; set; } = null!;
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }
    }
}
