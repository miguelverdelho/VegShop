using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegetableShop.Base;
using VegetableShop.Models;

namespace VegetableShop.Interfaces
{
    public interface IShoppingService : IBaseSingleton
    {
        public void LoadProducts(Dictionary<string, decimal> products);
        public void LoadPurchases(Dictionary<string, int> purchases);
        public bool ValidateOrder();
        public Receipt ProcessOrder();
    }
}
