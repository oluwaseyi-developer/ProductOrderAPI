﻿namespace ProductOrderApi.Application.Common.DTOs
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
