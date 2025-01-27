using System.ComponentModel.DataAnnotations;
using WebShop.API.Enums;

namespace WebShop.API.Models.Domain
{
    public class Cart
    {
        [Key]
        public Guid CartId { get; set; }
        public CartStatus CartStatus { get; set; }
        public Guid UserId { get; set; }

        //Navigation properties
        public User User { get; set; }
        public ICollection<CartItem> CartItems{ get; set; }
    }
}
