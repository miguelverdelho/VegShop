using Microsoft.Extensions.Logging;
using Moq;
using VegetableShop.Services;
using VegetableShop.UnitTests.Services.TestData;

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
            var products = ShoppingServiceMockData.ValidProducts;
            var purchases = ShoppingServiceMockData.ValidOrder;
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
            var products = ShoppingServiceMockData.ValidProducts;
            var purchases = ShoppingServiceMockData.EmptyOrder;
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
            var purchases = ShoppingServiceMockData.NonExisitingItem;
            var products = ShoppingServiceMockData.ValidProducts;
            LoadItemsToService(products, purchases);

            // Act
            var result = _shoppingService?.ValidateOrder();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateOrder_ReturnsFalse_WhenOrderContainsNegativeQuantity()
        {
            // Arrange
            var purchases = ShoppingServiceMockData.NegativeQuantityPurchase;
            var products = ShoppingServiceMockData.ValidProducts;
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
            var purchases = ShoppingServiceMockData.ValidOrder;
            var products = ShoppingServiceMockData.NegativePriceProduct;
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
            var products = ShoppingServiceMockData.ValidProducts;
            var purchases = ShoppingServiceMockData.ValidOrder;
            LoadItemsToService(products, purchases);

            // Act
            var receipt = _shoppingService?.ProcessOrder();

            // Assert
            Assert.NotNull(receipt);
            Assert.Equal(3, receipt.Items.Count);
            foreach (var item in receipt.Items)
            {
                Assert.True(products.ContainsKey(item.Product) && products[item.Product] == item.PricePerUnit);
                Assert.True(purchases.ContainsKey(item.Product) && purchases[item.Product] == item.Quantity);
            }
        }

        [Fact]
        public void ProcessOrder_ReturnsReceiptWithEmptyItems_WhenPurchasesIsEmpty()
        {
            // Arrange
            var products = ShoppingServiceMockData.ValidProducts;
            var purchases = ShoppingServiceMockData.EmptyOrder;
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
            var products = ShoppingServiceMockData.EmptyProducts;
            var purchases = ShoppingServiceMockData.ValidOrder;
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

    

}