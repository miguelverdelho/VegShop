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
    public class ShoppingService : BaseService<ShoppingService>, IShoppingService
    {
        private readonly Dictionary<string, decimal> _products; // <name, price>
        private readonly Dictionary<string, int> _purchases; // <name, quantity>
        public ShoppingService(ILogger<ShoppingService> logger, Dictionary<string, decimal> products, Dictionary<string, int> purchases) : base(logger) 
        {
            _products = products;
            _purchases = purchases;
        }

        public Receipt ProcessOrder()
        {
            // TODO move out of processorder()
            if (!ValidateOrder())
            {
                throw new Exception("Invalid order");
            }

            var receipt = new Receipt();

            foreach(var purchase in _purchases)
            {
                receipt.Items.Add(new(){
                    Product = purchase.Key,
                    Quantity = purchase.Value,
                    SubTotal = _products[purchase.Key] * purchase.Value
                });
            }

            // Get Offers and process them


            // Print Receipt (maybe out of here)
            return receipt;
        }

        public bool ValidateOrder()
        {
            // all items in purchase are available to buy
            foreach(var purchasedItem in _purchases.Keys)
            {
                if (!_products.ContainsKey(purchasedItem))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
