using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegetableShop.UnitTests.Services.TestData
{
    internal static class FileReaderServiceMockData
    {
        internal static List<string> ValidProducts = new List<string> { "PRODUCT,PRICE", "Tomato,0.75", "Aubergine,0.9", "Carrot,1" };
        internal static List<string> InvalidProducts = new List<string> { "Tomato", "" };
        internal static List<string> EmptyProducts = new List<string> { "" };

        internal static List<string> ValidPurchases = new List<string> { "PRODUCT,QUANTITY", "Tomato,1", "Aubergine,2", "Carrot,3" };
        internal static List<string> InvalidPurchases = new List<string> { "Tomato", "" };
        internal static List<string> EmptyPurchases = new List<string> { "" };
    }
}
