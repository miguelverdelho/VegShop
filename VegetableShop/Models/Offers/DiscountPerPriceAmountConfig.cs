namespace VegetableShop.Models.OffersConfig
{
    public class DiscountPerPriceAmountConfig
    {
        public string Product { get; set; } = string.Empty;
        public decimal MinAmount { get; set; }
        public decimal Discount { get; set; }
    }
}
