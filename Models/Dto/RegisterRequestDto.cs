using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Dto
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [DataType(DataType.Text)]
        [MaxLength(20, ErrorMessage = "Username can be up to 20 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [MaxLength(100, ErrorMessage = "Password can be up to 100 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [DataType(DataType.Text)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [DataType(DataType.Text)]
        [MaxLength(50)]
        public string LastName { get; set; }
    }
}
