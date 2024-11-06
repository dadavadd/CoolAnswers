using System.ComponentModel.DataAnnotations;

namespace CoolAnswers.DTOs.User
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
