using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Dto
{
    public class UpdateCartItemRequestDto
    {
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
    }
}
