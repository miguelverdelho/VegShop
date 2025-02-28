using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegetableShop.Base;
using VegetableShop.Interfaces;
using VegetableShop.Models;
using VegetableShop.Services.Offers;

namespace VegetableShop.Services
{
    public class OfferService : BaseService<OfferService>, IOfferService
    {
        List<IOffer> _offers = new List<IOffer>();

        public OfferService(ILogger<OfferService> logger) : base(logger)
        {
            _offers.Add(new BuyXGetYFree("Tomato", 3, 1));
            _offers.Add(new GetFreeXFromMultipleY("Aubergine", 10, "Carrot"));
            _offers.Add(new DiscountPerPriceAmount("Carrot", 5, 0.5m));
        }

        public void ApplyOffers(ref Receipt receipt)
        {
            foreach (var item in receipt.Items)
            {
                //check if there are offers for this product
                var offers = _offers.Where(o => o.RequiredProduct == item.Product).ToList();
                foreach (var offer in offers)
                {
                    _logger.LogInformation($"Applying offer {offer.GetType().Name} for product {item.Product}");
                    offer.Apply(ref receipt);
                }
            }
        }
    }
}
