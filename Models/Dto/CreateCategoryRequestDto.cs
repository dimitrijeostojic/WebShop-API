using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Dto
{
    public class CreateCategoryRequestDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, ErrorMessage = "Category name must be less than 50 characters.")]
        public string CategoryName { get; set; }
    }
}
