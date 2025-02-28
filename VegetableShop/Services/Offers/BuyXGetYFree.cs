using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegetableShop.Interfaces;
using VegetableShop.Models;

namespace VegetableShop.Services.Offers
{
    public class BuyXGetYFree : IOffer
    {
        public string Product { get; }
        public int RequiredQuantity { get; }
        public int FreeQuantity { get; }
        public string Description => $"Buy {RequiredQuantity} {Product}(s), get {FreeQuantity} free.";

        public BuyXGetYFree(string product, int requiredQuantity, int freeQuantity)
        {
            Product = product;
            RequiredQuantity = requiredQuantity;
            FreeQuantity = freeQuantity;
        }

        public void Apply(ref Receipt receipt)
        {
            var item = receipt.Items.FirstOrDefault(i => i.Product == Product);
            if (item != null && item.Quantity >= RequiredQuantity)
            {
                int freeItems = (item.Quantity / RequiredQuantity) * FreeQuantity;
                decimal discount = freeItems * item.PricePerUnit;

                receipt.ApplyDiscount(Product, discount, Description);
            }
        }
    }

}
