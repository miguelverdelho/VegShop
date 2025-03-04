﻿namespace VegetableShop.Models
{
    public class ReceiptItem
    {
        public string Product { get; }
        public int Quantity { get; }
        public decimal PricePerUnit { get; }
        public decimal SubTotal { get; }

        public ReceiptItem(string product, int quantity, decimal pricePerUnit)
        {
            Product = product;
            Quantity = quantity;
            PricePerUnit = pricePerUnit;
            SubTotal = quantity * pricePerUnit;
        }
    }
}
