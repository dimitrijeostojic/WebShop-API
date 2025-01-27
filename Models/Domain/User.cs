using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Domain
{
    public class User:IdentityUser
    {
        [Key]
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Navigation properties
        public ICollection<Order> Orders { get; set; }
        public Cart Cart { get; set; }
    }
}
