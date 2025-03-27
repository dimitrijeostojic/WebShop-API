namespace WebShop.API.Models.Dto
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public Guid CategoryId { get; set; }

        public string? CategoryName { get; set; } // korisno za prikaz
    }
}
