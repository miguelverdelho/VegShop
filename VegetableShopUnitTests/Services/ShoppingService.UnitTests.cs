using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegetableShop.Services;

namespace VegetableShopUnitTests.Services
{
    public class ShoppingServiceUnitTests
    {
        private ShoppingService? _shoppingService;
        private Mock<ILogger<ShoppingService>>? _mockLogger;

        public ShoppingServiceUnitTests()
        {
            _mockLogger = new Mock<ILogger<ShoppingService>>();
            _shoppingService = new ShoppingService(_mockLogger.Object);
        }
        #region ValidateOrder
        [Fact]
        public void ValidateOrder_ReturnsTrue_WhenOrderIsValid()
        {
            // Arrange
            var products = ShoppingServiceTestData.ValidProducts;
            var purchases = ShoppingServiceTestData.ValidOrder;
            LoadItemsToService(products, purchases);

            // Act
            var result = _shoppingService?.ValidateOrder();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateOrder_ReturnsFalse_WhenOrderIsEmpty()
        {
            // Arrange
            var products = ShoppingServiceTestData.ValidProducts;
            var purchases = ShoppingServiceTestData.EmptyOrder;
            LoadItemsToService(products, purchases);

            // Act
            var result = _shoppingService?.ValidateOrder();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateOrder_ReturnsFalse_WhenOrderContainsNonExisitingItem()
        {
            // Arrange
            var purchases = ShoppingServiceTestData.NonExisitingItem;
            var products = ShoppingServiceTestData.ValidProducts;
            LoadItemsToService(products,purchases);

            // Act
            var result = _shoppingService?.ValidateOrder();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateOrder_ReturnsFalse_WhenOrderContainsNegativeQuantity()
        {
            // Arrange
            var purchases = ShoppingServiceTestData.NegativeQuantityPurchase;
            var products = ShoppingServiceTestData.ValidProducts;
            LoadItemsToService(products, purchases);

            // Act
            var result = _shoppingService?.ValidateOrder();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateOrder_ReturnsFalse_WhenProductsContainsNegativePrice()
        {
            // Arrange
            var purchases = ShoppingServiceTestData.ValidOrder;
            var products = ShoppingServiceTestData.NegativePriceProduct;
            LoadItemsToService(products, purchases);

            // Act
            var result = _shoppingService?.ValidateOrder();

            // Assert
            Assert.False(result);
        }
        #endregion

        #region ProcessOrder
        [Fact]
        public void ProcessOrder_ReturnsReceiptWithItems()
        {
            // Arrange
            var products = ShoppingServiceTestData.ValidProducts;
            var purchases = ShoppingServiceTestData.ValidOrder;
            LoadItemsToService(products, purchases);

            // Act
            var receipt = _shoppingService?.ProcessOrder();

            // Assert
            Assert.NotNull(receipt);
            Assert.Equal(3, receipt.Items.Count);
            foreach(var item in receipt.Items)
            {
                Assert.True(products.ContainsKey(item.Product) && products[item.Product] == item.PricePerUnit);
                Assert.True(purchases.ContainsKey(item.Product) && purchases[item.Product] == item.Quantity);
            }
        }

        [Fact]
        public void ProcessOrder_ReturnsReceiptWithEmptyItems_WhenPurchasesIsEmpty()
        {
            // Arrange
            var products = ShoppingServiceTestData.ValidProducts;
            var purchases = ShoppingServiceTestData.EmptyOrder;
            LoadItemsToService(products, purchases);

            // Act
            var receipt = _shoppingService?.ProcessOrder();

            // Assert
            Assert.NotNull(receipt);
            Assert.Empty(receipt.Items);
        }

        [Fact]
        public void ProcessOrder_ReturnsReceiptWithEmptyItems_WhenProductsIsEmpty()
        {
            // Arrange
            var products = ShoppingServiceTestData.EmptyProducts;
            var purchases = ShoppingServiceTestData.ValidOrder;
            LoadItemsToService(products, purchases);

            // Act
            var receipt = _shoppingService?.ProcessOrder();

            // Assert
            Assert.NotNull(receipt);
            Assert.Empty(receipt.Items);
        }
        #endregion 

        #region HelperMethods

        private void LoadItemsToService(Dictionary<string, decimal> products, Dictionary<string, int> purchases)
        {
            _shoppingService?.LoadProducts(products);
            _shoppingService?.LoadPurchases(purchases);
        }
        #endregion
    }

    public static class ShoppingServiceTestData
    {
        public static Dictionary<string, decimal> ValidProducts = new()
        {
            {"Tomato", 0.75m},
            {"Aubergine", 0.90m},
            {"Carrot", 1.00m}
        };

        public static Dictionary<string, decimal> NegativePriceProduct = new()
        {
            {"Tomato", -0.75m},
            {"Aubergine", 0.90m},
            {"Carrot", 1.00m}
        };

        public static Dictionary<string, decimal> EmptyProducts = new();

        public static Dictionary<string, int> ValidOrder = new()
        {
            {"Tomato", 3},
            {"Aubergine", 25},
            {"Carrot", 12}
        };

        public static Dictionary<string, int> NonExisitingItem = new()
        {
            {"NonExisitingItem", 1 }
        };
        
        public static Dictionary<string, int> NegativeQuantityPurchase = new()
        {
            {"Tomato", -1 }
        };

        public static Dictionary<string, int> EmptyOrder = new();
    }

}