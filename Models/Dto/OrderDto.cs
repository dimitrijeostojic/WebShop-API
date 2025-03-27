using WebShop.API.Enums;

namespace WebShop.API.Models.Dto
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Guid UserId { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }
    }
}
