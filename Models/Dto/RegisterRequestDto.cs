using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Dto
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(10, ErrorMessage = "Username has to be maximum of 10 characters")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password has to be minimum of 6 characters")]
        [MaxLength(10, ErrorMessage = "Password has to be maximum of 10 characters")]
        public string Password { get; set; }
        [Required]
        public string[] Roles { get; set; }

    }
}
