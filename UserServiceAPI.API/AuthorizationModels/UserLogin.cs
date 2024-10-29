using System.ComponentModel.DataAnnotations;

namespace UserServiceAPI.API.AuthorizationModels
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
