using Microsoft.Extensions.Logging;
using VegetableShop.Common;
using VegetableShop.Interfaces;
using VegetableShop.Models;
using VegetableShop.Models.Error_Handling;

namespace VegetableShop.Services
{
    public class ShoppingService(ILogger<ShoppingService> logger) : BaseService<ShoppingService>(logger), IShoppingService
    {
        private Dictionary<string, decimal> _products = []; // <name, price>
        private Dictionary<string, int> _purchases = []; // <name, quantity>

        public void LoadProducts(Dictionary<string, decimal> products) => _products = products;
        public void LoadPurchases(Dictionary<string, int> purchases) => _purchases = purchases;

        public Receipt ProcessOrder()
        {
            var receipt = new Receipt();

            foreach (var purchase in _purchases)
                if (_products.ContainsKey(purchase.Key))
                    receipt.AddItem(purchase.Key, purchase.Value, _products[purchase.Key]);

            return receipt;
        }

        public void ValidateOrder()
        {
            if (_purchases.Count == 0 || _products.Count == 0)
                throw new InvalidOrderException("Invalid order. Products or purchases are empty.");

            // all items in purchased need to be available
            // one can only buy positive ammounts
            if (_purchases.Any(p => !_products.Keys.Contains(p.Key) || p.Value <= 0))
                throw new InvalidOrderException("Purchase includes non existing product.");

            // unit price needs to be positive
            if (_products.Any(p => p.Value <= 0))
                throw new InvalidOrderException("Unit price needs to be positive.");
        }
    }
}
