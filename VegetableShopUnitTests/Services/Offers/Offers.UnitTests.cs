using System.Text;
using VegetableShop.Interfaces;
using VegetableShop.Models;
using VegetableShop.Models.Error_Handling;
using VegetableShop.Services.Offers;

namespace VegetableShop.UnitTests.Services.Offers
{
    public class OfferTests
    {
        [Theory]
        [MemberData(nameof(GetOfferTestData))]
        public void Apply_ValidatesOffers_Correctly(Type offerType, object[] constructorArgs, string product, int quantity, decimal pricePerUnit, string expectedOfferText, decimal expectedDiscount)
        {
            // Arrange
            var receipt = new Receipt();
            receipt.AddItem(product, quantity, pricePerUnit);
            var offer = (IOffer)Activator.CreateInstance(offerType, constructorArgs)!;

            // If the offer is GetFreeXFromMultipleY, ensure the free product is also present in the receipt
            if (offerType == typeof(GetFreeXFromMultipleY) && constructorArgs.Length == 3)
            {
                string freeProduct = (string)constructorArgs[2];
                receipt.AddItem(freeProduct, 2, 0.90m); // Assume a unit price for the free product
            }

            // Act
            receipt = offer.Apply(receipt);

            // Assert
            if (expectedDiscount > 0)
            {
                var actualDiscountText = receipt.AppliedOffers.FirstOrDefault(offerText => offerText.Contains(expectedOfferText));
                Assert.NotNull(actualDiscountText);
                Assert.Contains($"€{expectedDiscount:0.00}", actualDiscountText);
            }
            else
            {
                Assert.Empty(receipt.AppliedOffers);
            }
        }

        [Theory]
        [MemberData(nameof(GetInvalidBuyXGetYFreeData))]
        public void BuyXGetYFree_ThrowsInvalidOfferException_WhenInvalidParametersProvided(string product, int requiredQuantity, int freeQuantity)
        {
            // Act & Assert
            Assert.Throws<InvalidOfferException>(() => new BuyXGetYFree(product, requiredQuantity, freeQuantity));
        }

        [Theory]
        [MemberData(nameof(GetInvalidDiscountPerPriceAmountData))]
        public void DiscountPerPriceAmount_ThrowsInvalidOfferException_WhenInvalidParametersProvided(string product, decimal thresholdAmount, decimal discountAmount)
        {
            // Act & Assert
            Assert.Throws<InvalidOfferException>(() => new DiscountPerPriceAmount(product, thresholdAmount, discountAmount));
        }

        [Theory]
        [MemberData(nameof(GetInvalidGetFreeXFromMultipleYData))]
        public void GetFreeXFromMultipleY_ThrowsInvalidOfferException_WhenInvalidParametersProvided(string requiredProduct, int quantityRequired, string freeProduct)
        {
            // Act & Assert
            Assert.Throws<InvalidOfferException>(() => new GetFreeXFromMultipleY(requiredProduct, quantityRequired, freeProduct));
        }

        #region Test Data
        public static IEnumerable<object[]> GetInvalidBuyXGetYFreeData()
        {
            return new List<object[]>
            {
                new object[] { "", 3, 1 }, // Invalid: Empty product name
                new object[] { "Aubergine", 0, 1 } // Invalid: Required quantity <= 0
            };
        }

        public static IEnumerable<object[]> GetInvalidDiscountPerPriceAmountData()
        {
            return new List<object[]>
            {
                new object[] { "Carrot", 0m, 2m }, // Invalid: Threshold amount <= 0
                new object[] { "Carrot", 5m, 0m }, // Invalid: Discount amount <= 0
                new object[] { "", 5m, 2m } // Invalid: Empty product name
            };
        }

        public static IEnumerable<object[]> GetInvalidGetFreeXFromMultipleYData()
        {
            return new List<object[]>
            {
                new object[] { "Tomato", -1, "Aubergine" }, // Invalid: Negative quantity
                new object[] { "Tomato", 2, "" } // Invalid: Empty free product name
            };
        }

        public static IEnumerable<object[]> GetOfferTestData()
        {
            return new List<object[]>
            {
                // BuyXGetYFree Tests
                new object[] { typeof(BuyXGetYFree), new object[] { "Aubergine", 3, 1 }, "Aubergine", 6, 0.9m, "Buy 3 Aubergine(s), get 1 free.", 1.80m },
                new object[] { typeof(BuyXGetYFree), new object[] { "Aubergine", 3, 1 }, "Aubergine", 2, 0.9m, "", 0m },
                new object[] { typeof(BuyXGetYFree), new object[] { "Aubergine", 3, 1 }, "Aubergine", 9, 0.9m, "Buy 3 Aubergine(s), get 1 free.", 2.70m },
                new object[] { typeof(BuyXGetYFree), new object[] { "Aubergine", 3, 1 }, "Tomato", 6, 0.75m, "", 0m },
                
                // DiscountPerPriceAmount Tests
                new object[] { typeof(DiscountPerPriceAmount), new object[] { "Tomato", 4m, 1m }, "Tomato", 8, 0.75m, "For every €4.00 spent on Tomato, get €1.00 off.", 1.00m },
                new object[] { typeof(DiscountPerPriceAmount), new object[] { "Carrot", 5m, 2m }, "Carrot", 3, 1.5m, "", 0m },
                new object[] { typeof(DiscountPerPriceAmount), new object[] { "Tomato", 4m, 1m }, "Tomato", 4, 1.0m, "For every €4.00 spent on Tomato, get €1.00 off.", 1.00m },
                new object[] { typeof(DiscountPerPriceAmount), new object[] { "Carrot", 5m, 2m }, "Carrot", 10, 1.5m, "For every €5.00 spent on Carrot, get €2.00 off.", 6.00m },
                
                // GetFreeXFromMultipleY Tests
                new object[] { typeof(GetFreeXFromMultipleY), new object[] { "Tomato", 2, "Aubergine" }, "Tomato", 3, 0.75m, "Get 1 free Aubergine for every 2 Tomatos purchased.", 0.90m },
                new object[] { typeof(GetFreeXFromMultipleY), new object[] { "Tomato", 2, "Aubergine" }, "Tomato", 1, 0.75m, "", 0m },
                new object[] { typeof(GetFreeXFromMultipleY), new object[] { "Carrot", 3, "Aubergine" }, "Carrot", 6, 1.00m, "Get 1 free Aubergine for every 3 Carrots purchased.", 1.80m },
                new object[] { typeof(GetFreeXFromMultipleY), new object[] { "Carrot", 3, "Aubergine" }, "Carrot", 2, 1.00m, "", 0m }
            };
        }
        #endregion

        #region Print Receipt Tests
        [Fact]
        public void GenerateReceipt_ReturnsFormattedReceipt_WhenItemsAndOffersArePresent()
        {
            // Arrange
            var receipt = new Receipt();
            receipt.AddItem("Tomato", 3, 0.75m);
            receipt.AddItem("Aubergine", 2, 0.90m);
            receipt.ApplyDiscount("Tomato", 1.00m, "For every 4.00€ spent on Tomato, get 1.00€ off.");

            // Act
            var result = receipt.GenerateReceipt();

            // Assert
            var expectedReceipt = new StringBuilder()
                .AppendLine("Receipt:")
                .AppendLine("Tomato x3 @ €0.75 = €2.25")
                .AppendLine("Aubergine x2 @ €0.90 = €1.80")
                .AppendLine("Total: €3.05")
                .AppendLine("\nOffers Applied:")
                .AppendLine("For every 4.00€ spent on Tomato, get 1.00€ off. Discount Applied: €1.00")
                .ToString();

            Assert.Equal(expectedReceipt, result);
        }

        [Fact]
        public void GenerateReceipt_ReturnsFormattedReceipt_WhenNoOffersAreApplied()
        {
            // Arrange
            var receipt = new Receipt();
            receipt.AddItem("Carrot", 5, 1.00m);

            // Act
            var result = receipt.GenerateReceipt();

            // Assert
            var expectedReceipt = new StringBuilder()
                .AppendLine("Receipt:")
                .AppendLine("Carrot x5 @ €1.00 = €5.00")
                .AppendLine("Total: €5.00")
                .ToString();

            Assert.Equal(expectedReceipt, result);
        }
        #endregion
    }
}
