using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegetableShop.Interfaces;
using VegetableShop.Models;

namespace VegetableShop.Services.Offers
{
    public class DiscountPerPriceAmount : IOffer
    {
        public string Product { get; }
        public decimal ThresholdAmount { get; }
        public decimal DiscountAmount { get; }
        public string Description => $"For every {ThresholdAmount}€ spent on {Product}, get {DiscountAmount}€ off.";

        public DiscountPerPriceAmount(string product, decimal thresholdAmount, decimal discountAmount)
        {
            Product = product;
            ThresholdAmount = thresholdAmount;
            DiscountAmount = discountAmount;
        }

        public void Apply(ref Receipt receipt)
        {
            var item = receipt.Items.FirstOrDefault(i => i.Product == Product);
            if (item != null)
            {
                int discountMultiples = (int)(item.PricePerUnit * item.Quantity / ThresholdAmount);
                decimal totalDiscount = discountMultiples * DiscountAmount;

                if (totalDiscount > 0)
                {
                    receipt.ApplyDiscount(Product, totalDiscount, Description);
                }
            }
        }
    }
}
