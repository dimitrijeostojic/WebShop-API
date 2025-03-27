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
        public string UserId { get; set; }


        //navigation properties
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
