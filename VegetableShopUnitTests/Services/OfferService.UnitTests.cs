using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VegetableShop.Models;
using VegetableShop.Services;

namespace VegetableShop.UnitTests.Services
{
    public class OfferServiceUnitTests
    {
        private OfferService? _offerService;
        private ILogger<OfferService>? _logger;
        private IConfiguration? _configuration;

        public OfferServiceUnitTests()
        {
            _logger = new LoggerFactory().CreateLogger<OfferService>();

            // Define in-memory configuration dictionary
            var inMemoryConfig = new Dictionary<string, string>
            {
                { "Offers:BuyXGetYFree:0:Product", "Tomato" },
                { "Offers:BuyXGetYFree:0:RequiredQuantity", "3" },
                { "Offers:BuyXGetYFree:0:FreeQuantity", "1" },

                { "Offers:GetFreeXFromMultipleY:0:Product", "Carrot" },
                { "Offers:GetFreeXFromMultipleY:0:RequiredQuantity", "2" },
                { "Offers:GetFreeXFromMultipleY:0:FreeProduct", "Aubergine" },

                { "Offers:DiscountPerPriceAmount:0:Product", "Aubergine" },
                { "Offers:DiscountPerPriceAmount:0:MinAmount", "5" },
                { "Offers:DiscountPerPriceAmount:0:Discount", "1" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfig!)
                .Build();

            _offerService = new OfferService(_logger, _configuration);
        }

        [Fact]
        public void ApplyOffers_AppliesBuyXGetYFreeOffer_Correctly()
        {
            // Arrange
            var receipt = new Receipt();
            receipt.AddItem("Tomato", 6, 0.75m);

            // Act
            var result = _offerService?.ApplyOffers(receipt);

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.AppliedOffers, offer => offer.Contains("Buy 3 Tomato(s), get 1 free"));
        }

        [Fact]
        public void ApplyOffers_AppliesGetFreeXFromMultipleYOffer_Correctly()
        {
            // Arrange
            var receipt = new Receipt();
            receipt.AddItem("Carrot", 4, 1.00m);
            receipt.AddItem("Aubergine", 1, 0.90m);

            // Act
            var result = _offerService?.ApplyOffers(receipt);

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.AppliedOffers, offer => offer.Contains("Get 1 free Aubergine for every 2 Carrots purchased"));
        }

        [Fact]
        public void ApplyOffers_AppliesDiscountPerPriceAmountOffer_Correctly()
        {
            // Arrange
            var receipt = new Receipt();
            receipt.AddItem("Aubergine", 6, 1.00m);

            // Act
            var result = _offerService?.ApplyOffers(receipt);

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result.AppliedOffers, offer => offer.Contains("For every €5.00 spent on Aubergine, get €1.00 off"));
        }

        [Fact]
        public void ApplyOffers_DoesNotApplyOffer_WhenConditionsNotMet()
        {
            // Arrange
            var receipt = new Receipt();
            receipt.AddItem("Carrot", 1, 1.00m);
            receipt.AddItem("Tomato", 2, 0.75m);

            // Act
            var result = _offerService?.ApplyOffers(receipt);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.AppliedOffers);
        }
    }
}
