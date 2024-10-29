using System.ComponentModel.DataAnnotations;

namespace UserServiceAPI.Services.DTO
{
    public class UserCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
    }
}
