using System.ComponentModel.DataAnnotations;
using WebShop.API.Enums;

namespace WebShop.API.Models.Domain
{
    public class Cart
    {
        [Key]
        public Guid CartId { get; set; }
        public string UserId { get; set; }

        //Navigation properties
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
