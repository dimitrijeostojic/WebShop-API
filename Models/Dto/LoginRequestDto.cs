using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Dto
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Username can be up to 50 characters.")]
        [MinLength(4, ErrorMessage = "Username must be at least 4 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MaxLength(100, ErrorMessage = "Password can be up to 100 characters.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
    }

}
