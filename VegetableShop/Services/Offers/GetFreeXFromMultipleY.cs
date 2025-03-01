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

        public Receipt Apply(Receipt receipt)
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

            return receipt;
        }
    }
}
