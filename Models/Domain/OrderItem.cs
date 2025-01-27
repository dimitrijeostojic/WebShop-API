using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Domain
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }

        //Navigation properties
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
