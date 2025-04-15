using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Dto
{
    public class AddToCartRequestDto
    {
        [Required(ErrorMessage = "ProductId is required")]
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 100, ErrorMessage = "The quantity must not be negative")]
        public int Quantity { get; set; }
    }
}
