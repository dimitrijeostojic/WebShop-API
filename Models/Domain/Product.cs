﻿using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Domain
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public Guid CategoryId{ get; set; }
        public string? CreatedBy { get; set; }

        //Navigation properties
        public Category Category { get; set; }
    }
}
