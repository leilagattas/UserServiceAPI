using System.ComponentModel.DataAnnotations;

namespace UserServiceAPI.Services.DTO
{
    public class UserDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
    }
}
