namespace WebShop.API.Models.Dto
{
    public class CartItemDto
    {
        public Guid CartItemId { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public ProductDto Product { get; set; }
    }
}
