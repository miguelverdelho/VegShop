using VegetableShop.Interfaces;
using VegetableShop.Models;
using VegetableShop.Models.Error_Handling;

namespace VegetableShop.Services.Offers
{
    public class DiscountPerPriceAmount : IOffer
    {
        public string RequiredProduct { get; }
        public decimal ThresholdAmount { get; }
        public decimal DiscountAmount { get; }
        public string Description => $"For every {ThresholdAmount:C2} spent on {RequiredProduct}, get {DiscountAmount:C2} off.";

        public DiscountPerPriceAmount(string product, decimal thresholdAmount, decimal discountAmount)
        {
            if (thresholdAmount <= 0 || discountAmount <= 0)
                throw new InvalidOfferException("Thresholds and discounts must be greater than 0.");
            if (string.IsNullOrEmpty(product))
                throw new InvalidOfferException("Product cannot be empty.");

            RequiredProduct = product;
            ThresholdAmount = thresholdAmount;
            DiscountAmount = discountAmount;
        }

        public Receipt Apply(Receipt receipt)
        {
            var item = receipt.Items.FirstOrDefault(i => i.Product == RequiredProduct);
            if (item != null)
            {
                var discountMultiples = (int)(item.PricePerUnit * item.Quantity / ThresholdAmount);
                var totalDiscount = discountMultiples * DiscountAmount;

                if (totalDiscount > 0)
                {
                    receipt.ApplyDiscount(RequiredProduct, totalDiscount, Description);
                }
            }

            return receipt;
        }
    }
}
