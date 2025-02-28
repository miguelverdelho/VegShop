using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegetableShop.Base;

namespace VegetableShop.Interfaces
{
    public interface IFileReaderService : IBaseSingleton
    {
        public Dictionary<string, decimal> ReadProductsFromFile();
        public Dictionary<string, int> ReadPurchasesFromFile();
    }
}
