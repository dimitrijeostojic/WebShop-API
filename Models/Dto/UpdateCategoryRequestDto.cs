using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Dto
{
    public class UpdateCategoryRequestDto
    {
        [Required(ErrorMessage = "CategoryName is required")]
        public string CategoryName { get; set; }
    }
}
