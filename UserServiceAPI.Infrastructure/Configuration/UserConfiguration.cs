using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserServiceAPI.Domain.Models;

namespace UserServiceAPI.Infrastructure.Configuration
{
    public class UserConfiguration
    {
        public UserConfiguration(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(d => d.Id)
                .ValueGeneratedOnAdd();
            builder
                .Property(x => x.Name)
                .IsRequired();
        }
    }
}
