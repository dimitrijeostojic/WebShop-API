using System.ComponentModel.DataAnnotations;
using WebShop.API.Enums;

namespace WebShop.API.Models.Domain
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Guid UserId { get; set; }


        //navigation properties
        public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
