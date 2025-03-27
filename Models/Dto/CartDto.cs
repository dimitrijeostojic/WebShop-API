using WebShop.API.Enums;

namespace WebShop.API.Models.Dto
{
    public class CartDto
    {
        public Guid CartId { get; set; }
        public CartStatus CartStatus { get; set; }
        public Guid UserId { get; set; }
        public decimal Total => CartItems?.Sum(x => x.Product.Price * x.Quantity) ?? 0;

        public List<CartItemDto> CartItems { get; set; }
    }
}
