using VegetableShop.Models;

namespace VegetableShop.Interfaces
{
    public interface IOffer
    {
        public string RequiredProduct { get; }
        public Receipt Apply(Receipt receipt);
    }
}
