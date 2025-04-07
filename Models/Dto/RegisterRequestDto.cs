using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Dto
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [DataType(DataType.Text)]
        [MaxLength(20, ErrorMessage = "Username can be up to 20 characters.")]
        [MinLength(4, ErrorMessage = "Username must be at least 4 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MaxLength(100, ErrorMessage = "Password can be up to 100 characters.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "First name can be up to 50 characters.")]
        [MinLength(4, ErrorMessage = "First name must be at least 4 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Last name can be up to 50 characters.")] 
        [MinLength(4, ErrorMessage = "Last name must be at least 4 characters.")]
        public string LastName { get; set; }
    }
}
