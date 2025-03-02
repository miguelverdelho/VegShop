using VegetableShop.Models;

namespace VegetableShop.Interfaces
{
    public interface IOfferService : IBaseTransient
    {
        public Receipt ApplyOffers(Receipt receipt);
    }
}
