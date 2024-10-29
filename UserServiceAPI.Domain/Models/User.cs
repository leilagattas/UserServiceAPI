using System.ComponentModel.DataAnnotations;

namespace UserServiceAPI.Domain.Models
{
    public class User : BaseEntity<User>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }
        public bool Active { get; set; } = true;
    }
}