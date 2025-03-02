using VegetableShop.Models;

namespace VegetableShop.Interfaces
{
    public interface IShoppingService : IBaseTransient
    {
        public void LoadProducts(Dictionary<string, decimal> products);
        public void LoadPurchases(Dictionary<string, int> purchases);
        public void ValidateOrder();
        public Receipt ProcessOrder();
    }
}
