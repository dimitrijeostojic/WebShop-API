using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebShop.API.Enums;

namespace WebShop.API.Models.Dto
{
    public class UpdateOrderStatusDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = "Order status is requeired")]
        public OrderStatus NewStatus { get; set; }
    }
}
