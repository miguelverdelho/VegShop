using VegetableShop.Interfaces;
using VegetableShop.Models;
using VegetableShop.Models.Error_Handling;

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
            if(requiredQuantity <= 0 || freeQuantity <= 0)
                throw new InvalidOfferException("Quantities must be greater than 0.");

            if (string.IsNullOrEmpty(product))
                throw new InvalidOfferException("Product cannot be empty.");

            RequiredProduct = product;
            RequiredQuantity = requiredQuantity;
            FreeQuantity = freeQuantity;
        }

        public Receipt Apply(Receipt receipt)
        {
            var item = receipt.Items.FirstOrDefault(i => i.Product == RequiredProduct);
            if (item != null && item.Quantity >= RequiredQuantity)
            {
                var freeItems = item.Quantity / RequiredQuantity * FreeQuantity;
                var discount = freeItems * item.PricePerUnit;

                receipt.ApplyDiscount(RequiredProduct, discount, Description);
            }

            return receipt;
        }
    }

}
