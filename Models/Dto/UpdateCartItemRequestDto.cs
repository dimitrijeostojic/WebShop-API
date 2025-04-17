using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Dto
{
    public class UpdateCartItemRequestDto
    {
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 100, ErrorMessage = "The quantity must not be negative")]
        public int Quantity { get; set; }
    }
}
