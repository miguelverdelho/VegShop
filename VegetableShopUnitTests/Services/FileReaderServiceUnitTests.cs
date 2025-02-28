using Microsoft.Extensions.Logging;
using Moq;
using VegetableShop.Services;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.Extensions.Configuration;
using VegetableShop.Models;

namespace VegetableShopUnitTests.Services
{
    public class FileReaderServiceUnitTests
    {
        private FileReaderService? _fileReaderService;
        private Mock<ILogger<FileReaderService>>? _logger;
        private Mock<IConfiguration> _configuration = new Mock<IConfiguration>(); // Mock IConfiguration>
        private string _productsTestFilePath;
        private string _purchasesTestFilePath;

        public FileReaderServiceUnitTests()
        {
            _logger = new ();
            _productsTestFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "ProductsTestFile.csv");
            _purchasesTestFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "PurchasesTestFile.csv");
            File.WriteAllLines(_purchasesTestFilePath, new List<string>());
            File.WriteAllLines(_productsTestFilePath, new List<string>());

            // Use Always the same test file - change its content according to the test
            _configuration.Setup(c => c.GetSection("FileSettings")["ProductsFilePath"]).Returns(_productsTestFilePath);
            _configuration.Setup(c => c.GetSection("FileSettings")["PurchasesFilePath"]).Returns(_purchasesTestFilePath);

            _fileReaderService = new FileReaderService(_logger.Object, _configuration.Object);
        }

        #region File Path & Config Validations
        [Fact]
        public void CreateService_ProductsFileNotFound_ThrowsException()
        {
            // Arrange - Make sure products file only is missing
            File.Delete(_productsTestFilePath);
            File.WriteAllLines(_purchasesTestFilePath, new List<string>());

            // Act && Assert
            Assert.Throws<FileNotFoundException>(() => new FileReaderService(_logger!.Object, _configuration.Object));
        }

        [Fact]
        public void CreateService_PurchasesFileNotFound_ThrowsException()
        {
            // Arrange  - Make sure products file only is missing
            File.Delete(_purchasesTestFilePath);
            File.WriteAllLines(_productsTestFilePath, new List<string>());

            // Act && Assert
            Assert.Throws<FileNotFoundException>(() => new FileReaderService(_logger!.Object, _configuration.Object));
        }

        [Fact]
        public void CreateService_FileConfigSectionNotFound_ThrowsException()
        {
            // Arrange
            _configuration.Setup(c => c.GetSection("FileSettings")).Returns(() => null);

            // Act && Assert
            Assert.Throws<Exception>(() => new FileReaderService(_logger!.Object, _configuration.Object));
        }

        [Fact]
        public void CreateService_ProductsFileConfigNotFound_ThrowsException()
        {
            // Arrange
            _configuration.Setup(c => c.GetSection("FileSettings")["ProductsFilePath"]).Returns(() => null);

            // Act && Assert
            Assert.Throws<Exception>(() => new FileReaderService(_logger!.Object, _configuration.Object));
        }

        [Fact]
        public void CreateService_PurchaseFileConfigNotFound_ThrowsException()
        {
            // Arrange
            _configuration.Setup(c => c.GetSection("FileSettings")["PurchasesFilePath"]).Returns(() => null);

            // Act && Assert
            Assert.Throws<Exception>(() => new FileReaderService(_logger!.Object, _configuration.Object));
        }

        #endregion

        #region LoadPurchases
        [Fact]
        public void LoadPurchases_ValidFile_ReturnsPurchases()
        {
            // Arrange
            var purchases = new List<string> { "PRODUCT,QUANTITY", "Tomato,3", "Aubergine,25", "Carrot,12" };
            File.WriteAllLines(_purchasesTestFilePath, purchases);

            // Act
            var result = _fileReaderService?.LoadPurchases();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, kvp => kvp.Key == "Tomato" && kvp.Value == 3);
            Assert.Contains(result, kvp => kvp.Key == "Aubergine" && kvp.Value == 25);
            Assert.Contains(result, kvp => kvp.Key == "Carrot" && kvp.Value == 12);
        }

        [Fact]
        public void LoadPurchases_EmptyValidFile_ReturnsEmptyList()
        {
            // Arrange
            var purchases = new List<string> { };
            File.WriteAllLines(_productsTestFilePath, purchases);

            // Act
            var result = _fileReaderService?.LoadPurchases();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void LoadPurchases_InvalidFile_ThrowsArgumentException()
        {
            // Arrange
            var purchases = new List<string> { "PRODUCT,QUANTITY", "Tomato 3", "Aubergine,25", "Carrot,12" };
            File.WriteAllLines(_purchasesTestFilePath, purchases);

            // Act && Assert
            Assert.Throws<ArgumentException>(() => _fileReaderService?.LoadPurchases());
        }
        #endregion

        #region LoadProducts
        [Fact]
        public void LoadProducts_ValidFile_ReturnsProducts()
        {
            // Arrange
            var products = new List<string> { "PRODUCT,PRICE", "Tomato,0.75", "Aubergine,0.9", "Carrot,1" };
            File.WriteAllLines(_productsTestFilePath, products);

            // Act
            var result = _fileReaderService?.LoadProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, kvp => kvp.Key == "Tomato" && kvp.Value == 0.75m);
            Assert.Contains(result, kvp => kvp.Key == "Aubergine" && kvp.Value == 0.9m);
            Assert.Contains(result, kvp => kvp.Key == "Carrot" && kvp.Value == 1m);
        }
        [Fact]
        public void LoadProducts_InvalidFile_ThrowsArgumentException()
        {
            // Arrange
            var products = new List<string> { "PRODUCT,PRICE", "Tomato 0.75", "Aubergine,0.9", "Carrot,1" };
            File.WriteAllLines(_productsTestFilePath, products);

            // Act && Assert
            Assert.Throws<ArgumentException>(() =>  _fileReaderService?.LoadProducts());
        }

        [Fact]
        public void LoadProducts_EmptyValidFile_ReturnsEmptyList()
        {
            // Arrange
            var products = new List<string> { };
            File.WriteAllLines(_productsTestFilePath, products);

            // Act
            var result = _fileReaderService?.LoadProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        #endregion
    }
}