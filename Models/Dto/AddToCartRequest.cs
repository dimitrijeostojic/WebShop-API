﻿namespace WebShop.API.Models.Dto
{
    public class AddToCartRequestDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
