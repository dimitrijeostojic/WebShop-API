using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Dto
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Username can be up to 50 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [MaxLength(100, ErrorMessage = "Password can be up to 100 characters.")]
        public string Password { get; set; }
    }

}
