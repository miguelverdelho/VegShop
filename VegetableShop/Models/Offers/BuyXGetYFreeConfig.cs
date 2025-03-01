namespace VegetableShop.Models.OffersConfig
{
    public class BuyXGetYFreeConfig
    {
        public string Product { get; set; } = string.Empty;
        public int RequiredQuantity { get; set; }
        public int FreeQuantity { get; set; }
    }
}
