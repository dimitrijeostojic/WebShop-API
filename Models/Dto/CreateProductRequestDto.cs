using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Dto
{
    public class CreateProductRequestDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name must be less than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 999999)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description must be less than 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Stock is required.")]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required(ErrorMessage = "ImageUrl is required.")]
        [Url]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        public Guid CategoryId { get; set; }
    }
}
