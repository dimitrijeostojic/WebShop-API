using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Domain
{
    public class CartItem
    {
       
        public Guid CartItemId { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        //Navigation properties
        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
