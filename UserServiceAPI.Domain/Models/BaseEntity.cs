using System.ComponentModel.DataAnnotations;

namespace UserServiceAPI.Domain.Models
{
    public abstract class BaseEntity<T>
    {
        [Key]
        public Guid Id { get; set; }
    }
}
