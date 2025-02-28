using VegetableShop.Base;

namespace VegetableShop.Interfaces
{
    public interface IVegetableShopService : IBaseSingleton
    {
        public void Run();
    }
}