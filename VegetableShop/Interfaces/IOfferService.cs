using VegetableShop.Models;

namespace VegetableShop.Interfaces
{
    public interface IOfferService : IBaseSingleton
    {
        public Receipt ApplyOffers(Receipt receipt);
    }
}
