using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Dto
{
    public class UpdateProductRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999)]
        public decimal Price { get; set; }
      
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    
        [Required(ErrorMessage = "ImageUrl is required")]
        [Url]
        public string ImageUrl { get; set; }
        
        [Required(ErrorMessage = "CategoryId is required")]
        public Guid CategoryId { get; set; }
    }
}
