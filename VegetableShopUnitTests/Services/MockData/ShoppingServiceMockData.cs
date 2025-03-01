using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegetableShop.UnitTests.Services.TestData
{
    internal static class ShoppingServiceMockData
    {
        internal static Dictionary<string, decimal> ValidProducts = new()
        {
            {"Tomato", 0.75m},
            {"Aubergine", 0.90m},
            {"Carrot", 1.00m}
        };

        internal static Dictionary<string, decimal> NegativePriceProduct = new()
        {
            {"Tomato", -0.75m},
            {"Aubergine", 0.90m},
            {"Carrot", 1.00m}
        };

        internal static Dictionary<string, decimal> EmptyProducts = new();

        internal static Dictionary<string, int> ValidOrder = new()
        {
            {"Tomato", 3},
            {"Aubergine", 25},
            {"Carrot", 12}
        };

        internal static Dictionary<string, int> NonExisitingItem = new()
        {
            {"NonExisitingItem", 1 }
        };

        internal static Dictionary<string, int> NegativeQuantityPurchase = new()
        {
            {"Tomato", -1 }
        };

        internal static Dictionary<string, int> EmptyOrder = new();
    }
}
