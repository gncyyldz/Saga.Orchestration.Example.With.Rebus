﻿using Order.API.Models.Enums;

namespace Order.API.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}