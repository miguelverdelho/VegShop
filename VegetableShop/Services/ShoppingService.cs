using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegetableShop.Base;
using VegetableShop.Interfaces;
using VegetableShop.Models;

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

            foreach(var purchase in _purchases)
                if (_products.ContainsKey(purchase.Key))
                    receipt.AddItem( purchase.Key, purchase.Value, _products[purchase.Key]);

            return receipt;
        }

        public bool ValidateOrder()
        {
            if(_purchases.Count == 0 || _products.Count == 0)
                return false;

            // all items in purchased need to be available
            // one can only buy positive ammounts
            if (_purchases.Any(p => !_products.Keys.Contains(p.Key) || p.Value <= 0))
                return false;

            // unit price needs to be positive
            if(_products.Any(p => p.Value <= 0))
                return false;

            return true;
        }
    }
}
