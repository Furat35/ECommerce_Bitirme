﻿using ECommerce.Core.Entities;

namespace ECommerce.Entity.Entities
{
    public class CartItem : BaseEntity
    {
        public Guid CartId { get; set; }
        public Cart Cart { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
