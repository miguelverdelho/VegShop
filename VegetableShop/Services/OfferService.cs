using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using VegetableShop.Common;
using VegetableShop.Interfaces;
using VegetableShop.Models;
using VegetableShop.Models.OffersConfig;
using VegetableShop.Services.Offers;

namespace VegetableShop.Services
{
    public class OfferService : BaseService<OfferService>, IOfferService
    {
        private readonly List<IOffer> _offers = new();

        public OfferService(ILogger<OfferService> logger, IConfiguration configuration) : base(logger)
        {
            LoadOffersFromConfig(configuration);
        }

        private void LoadOffersFromConfig(IConfiguration configuration)
        {
            var buyXGetYFreeOffers = configuration.GetSection("Offers:BuyXGetYFree").Get<List<BuyXGetYFreeConfig>>();
            if (buyXGetYFreeOffers != null)
            {
                foreach (var offer in buyXGetYFreeOffers)
                {
                    _offers.Add(new BuyXGetYFree(offer.Product, offer.RequiredQuantity, offer.FreeQuantity));
                }
            }

            var getFreeXFromMultipleYOffers = configuration.GetSection("Offers:GetFreeXFromMultipleY").Get<List<GetFreeXFromMultipleYConfig>>();
            if (getFreeXFromMultipleYOffers != null)
            {
                foreach (var offer in getFreeXFromMultipleYOffers)
                {
                    _offers.Add(new GetFreeXFromMultipleY(offer.Product, offer.RequiredQuantity, offer.FreeProduct));
                }
            }

            var discountPerPriceAmountOffers = configuration.GetSection("Offers:DiscountPerPriceAmount").Get<List<DiscountPerPriceAmountConfig>>();
            if (discountPerPriceAmountOffers != null)
            {
                foreach (var offer in discountPerPriceAmountOffers)
                {
                    _offers.Add(new DiscountPerPriceAmount(offer.Product, offer.MinAmount, offer.Discount));
                }
            }
        }

        public Receipt ApplyOffers(Receipt receipt)
        {
            var receiptItems = receipt.Items;
            foreach (var item in receiptItems)
            {
                var offers = _offers.Where(o => o.RequiredProduct == item.Product).ToList();
                foreach (var offer in offers)
                {
                    _logger.LogInformation($"Applying offer {offer.GetType().Name} for product {item.Product}");
                    receipt = offer.Apply(receipt);
                }
            }
            return receipt;
        }
    }

}
