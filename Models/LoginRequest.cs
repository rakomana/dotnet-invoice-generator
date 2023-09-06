using System.ComponentModel.DataAnnotations;

namespace learnApi.Models 
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username or email is required.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}