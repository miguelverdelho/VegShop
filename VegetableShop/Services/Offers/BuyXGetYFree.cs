using VegetableShop.Interfaces;
using VegetableShop.Models;

namespace VegetableShop.Services.Offers
{
    public class BuyXGetYFree : IOffer
    {
        public string RequiredProduct { get; }
        public int RequiredQuantity { get; }
        public int FreeQuantity { get; }
        public string Description => $"Buy {RequiredQuantity} {RequiredProduct}(s), get {FreeQuantity} free.";

        public BuyXGetYFree(string product, int requiredQuantity, int freeQuantity)
        {
            RequiredProduct = product;
            RequiredQuantity = requiredQuantity;
            FreeQuantity = freeQuantity;
        }

        public Receipt Apply(Receipt receipt)
        {
            var item = receipt.Items.FirstOrDefault(i => i.Product == RequiredProduct);
            if (item != null && item.Quantity >= RequiredQuantity)
            {
                int freeItems = item.Quantity / RequiredQuantity * FreeQuantity;
                decimal discount = freeItems * item.PricePerUnit;

                receipt.ApplyDiscount(RequiredProduct, discount, Description);
            }

            return receipt;
        }
    }

}
