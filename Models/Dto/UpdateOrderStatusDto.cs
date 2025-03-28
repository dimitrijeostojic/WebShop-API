using System.Text.Json.Serialization;
using WebShop.API.Enums;

namespace WebShop.API.Models.Dto
{
    public class UpdateOrderStatusDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus NewStatus { get; set; }
    }
}
