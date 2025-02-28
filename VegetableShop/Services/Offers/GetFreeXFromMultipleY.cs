using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegetableShop.Interfaces;
using VegetableShop.Models;

namespace VegetableShop.Services.Offers
{
    class GetFreeXFromMultipleY : IOffer
    {
        public string RequiredProduct { get; }
        public string FreeProduct { get; }
        public int QuantityRequired { get; }
        public string Description => $"Get 1 free {FreeProduct} for every {QuantityRequired} {RequiredProduct + (QuantityRequired > 1 ? "s" : "")} purchased.";

        public GetFreeXFromMultipleY(string requiredProduct, int quantityRequired, string freeProduct) 
        {
            RequiredProduct = requiredProduct;
            FreeProduct = freeProduct;
            QuantityRequired = quantityRequired;
        }

        public void Apply(ref Receipt receipt)
        {
            var requiredItem = receipt.Items.FirstOrDefault(i => i.Product == RequiredProduct);
            var freeItem = receipt.Items.FirstOrDefault(i => i.Product == FreeProduct);

            if (requiredItem != null && requiredItem.Quantity >= QuantityRequired 
                && freeItem != null && freeItem.Quantity > 0)
            {
                int freeItems = requiredItem.Quantity / QuantityRequired;

                decimal discount = freeItems * freeItem.PricePerUnit;

                receipt.ApplyDiscount(FreeProduct, discount, Description);
            }
        }
    }
}
