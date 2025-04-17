using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Dto
{
    public class NewsletterSubscriptionDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateSubscribed { get; set; }
    }
}
