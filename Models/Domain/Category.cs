using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Domain
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        //Navigation properties
        public ICollection<Product> Products { get; set; }
    }
}
