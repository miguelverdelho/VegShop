using Microsoft.Extensions.Logging;
using Moq;
using VegetableShop.Models.Error_Handling;
using VegetableShop.Services;
using VegetableShop.UnitTests.Services.MockData;

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
        public void ValidateOrder_DoesNotThrow_WhenOrderIsValid()
        {
            // Arrange
            var products = ShoppingServiceMockData.ValidProducts;
            var purchases = ShoppingServiceMockData.ValidOrder;
            LoadItemsToService(products, purchases);

            // Act & Assert
            var exception = Record.Exception(() => _shoppingService?.ValidateOrder());
            Assert.Null(exception);
        }

        [Fact]
        public void ValidateOrder_ThrowsInvalidOrderException_WhenOrderIsEmpty()
        {
            // Arrange
            var products = ShoppingServiceMockData.ValidProducts;
            var purchases = ShoppingServiceMockData.EmptyOrder;
            LoadItemsToService(products, purchases);

            // Act & Assert
            Assert.Throws<InvalidOrderException>(() => _shoppingService?.ValidateOrder());
        }

        [Fact]
        public void ValidateOrder_ThrowsInvalidOrderException_WhenOrderContainsNonExistingItem()
        {
            // Arrange
            var purchases = ShoppingServiceMockData.NonExisitingItem;
            var products = ShoppingServiceMockData.ValidProducts;
            LoadItemsToService(products, purchases);

            // Act & Assert
            Assert.Throws<InvalidOrderException>(() => _shoppingService?.ValidateOrder());
        }

        [Fact]
        public void ValidateOrder_ThrowsInvalidOrderException_WhenOrderContainsNegativeQuantity()
        {
            // Arrange
            var purchases = ShoppingServiceMockData.NegativeQuantityPurchase;
            var products = ShoppingServiceMockData.ValidProducts;
            LoadItemsToService(products, purchases);

            // Act & Assert
            Assert.Throws<InvalidOrderException>(() => _shoppingService?.ValidateOrder());
        }

        [Fact]
        public void ValidateOrder_ThrowsInvalidOrderException_WhenProductsContainNegativePrice()
        {
            // Arrange
            var purchases = ShoppingServiceMockData.ValidOrder;
            var products = ShoppingServiceMockData.NegativePriceProduct;
            LoadItemsToService(products, purchases);

            // Act & Assert
            Assert.Throws<InvalidOrderException>(() => _shoppingService?.ValidateOrder());
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