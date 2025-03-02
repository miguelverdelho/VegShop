using VegetableShop.Models;

namespace VegetableShop.Interfaces
{
    public interface IOffer
    {
        public string RequiredProduct { get; }
        public string Description { get; }
        public Receipt Apply(Receipt receipt);
    }
}
