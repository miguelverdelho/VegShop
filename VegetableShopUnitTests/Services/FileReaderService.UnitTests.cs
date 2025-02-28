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
        private const string FILE_SETTINGS = "FileSettings";
        private const string PRODUCTS_FILE_SETTING = "ProductsFilePath";
        private const string PURCHASES_FILE_SETTING = "PurchasesFilePath";
        private FileReaderService? _fileReaderService;
        private Mock<ILogger<FileReaderService>>? _mockLogger = new ();
        private Mock<IConfiguration> _mockConfiguration = new ();
        private string _productsTestFilePath;
        private string _purchasesTestFilePath;

        public FileReaderServiceUnitTests()
        {
            _productsTestFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "ProductsTestFile.csv");
            _purchasesTestFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "PurchasesTestFile.csv");
            File.WriteAllLines(_purchasesTestFilePath, new List<string>());
            File.WriteAllLines(_productsTestFilePath, new List<string>());

            // Use Always the same test file - change its content according to the test
            _mockConfiguration.Setup(c => c.GetSection(FILE_SETTINGS)[PRODUCTS_FILE_SETTING]).Returns(_productsTestFilePath);
            _mockConfiguration.Setup(c => c.GetSection(FILE_SETTINGS)[PURCHASES_FILE_SETTING]).Returns(_purchasesTestFilePath);

            _fileReaderService = CreateDefaultService();
        }

        #region File Path & Config Validations
        [Fact]
        public void CreateService_ThrowsException_ProductsFileNotFound()
        {
            // Arrange - Make sure products file only is missing
            File.Delete(_productsTestFilePath);
            File.WriteAllLines(_purchasesTestFilePath, new List<string>());

            // Act && Assert
            Assert.Throws<FileNotFoundException>(() => new FileReaderService(_mockLogger!.Object, _mockConfiguration.Object));
        }

        [Fact]
        public void CreateService_ThrowsException_PurchasesFileNotFound()
        {
            // Arrange  - Make sure products file only is missing
            File.Delete(_purchasesTestFilePath);
            File.WriteAllLines(_productsTestFilePath, new List<string>());

            // Act && Assert
            Assert.Throws<FileNotFoundException>(() => CreateDefaultService());
        }

        [Fact]
        public void CreateService_ThrowsException_FileConfigSectionNotFound()
        {
            // Arrange
            _mockConfiguration.Setup(c => c.GetSection(FILE_SETTINGS)).Returns(() => null);

            // Act && Assert
            Assert.Throws<Exception>(() => CreateDefaultService());
        }

        [Fact]
        public void CreateService_ThrowsException_ProductsFileConfigNotFound()
        {
            // Arrange
            _mockConfiguration.Setup(c => c.GetSection(FILE_SETTINGS)[PRODUCTS_FILE_SETTING]).Returns(() => null);

            // Act && Assert
            Assert.Throws<Exception>(() => CreateDefaultService());
        }

        [Fact]
        public void CreateService_ThrowsException_PurchaseFileConfigNotFound()
        {
            // Arrange
            _mockConfiguration.Setup(c => c.GetSection(FILE_SETTINGS)[PURCHASES_FILE_SETTING]).Returns(() => null);

            // Act && Assert
            Assert.Throws<Exception>(() => CreateDefaultService());
        }

        #endregion

        #region ReadPurchasesFromFile
        [Fact]
        public void ReadPurchasesFromFile_ReturnsPurchases_ValidFile()
        {
            // Arrange
            var purchases = FileReaderServiceTestData.ValidPurchases;
            File.WriteAllLines(_purchasesTestFilePath, purchases);

            // Act
            var result = _fileReaderService?.ReadPurchasesFromFile();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);            
            foreach (var kvp in result)
            {
                Assert.Contains($"{kvp.Key},{kvp.Value}", purchases);
            }
        }

        [Fact]
        public void ReadPurchasesFromFile_ThrowsEmptyListException_EmptyValidFile()
        {
            // Arrange
            var purchases = FileReaderServiceTestData.EmptyPurchases;
            File.WriteAllLines(_productsTestFilePath, purchases);

            // Act & Assert
            Assert.Throws<Exception>(() => _fileReaderService?.ReadPurchasesFromFile());
        }

        [Fact]
        public void ReadPurchasesFromFile_ThrowsArgumentException_InvalidFile()
        {
            // Arrange
            var purchases =  FileReaderServiceTestData.InvalidPurchases;
            File.WriteAllLines(_purchasesTestFilePath, purchases);

            // Act && Assert
            Assert.Throws<ArgumentException>(() => _fileReaderService?.ReadPurchasesFromFile());
        }
        #endregion

        #region ReadProductsFromFile
        [Fact]
        public void ReadProductsFromFile_ReturnsProducts_ValidFile()
        {
            // Arrange
            var products = FileReaderServiceTestData.ValidProducts;
            File.WriteAllLines(_productsTestFilePath, products);

            // Act
            var result = _fileReaderService?.ReadProductsFromFile();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            foreach (var kvp in result)
            {
                Assert.Contains($"{kvp.Key},{kvp.Value}", products);
            }
        }
        [Fact]
        public void ReadProductsFromFile_ThrowsArgumentException_InvalidFile()
        {
            // Arrange
            var products = FileReaderServiceTestData.InvalidProducts;
            File.WriteAllLines(_productsTestFilePath, products);

            // Act && Assert
            Assert.Throws<ArgumentException>(() =>  _fileReaderService?.ReadProductsFromFile());
        }

        [Fact]
        public void ReadProductsFromFile_ThrowsEmptyListException_EmptyValidFile()
        {
            // Arrange
            var products = FileReaderServiceTestData.EmptyProducts;
            File.WriteAllLines(_productsTestFilePath, products);

            // Act & Assert
            Assert.Throws<Exception>(() => _fileReaderService?.ReadProductsFromFile());
        }
        #endregion

        #region Helpers
        public FileReaderService CreateDefaultService() => new FileReaderService(_mockLogger!.Object, _mockConfiguration.Object);
        #endregion
    }

    class FileReaderServiceTestData
    {
        internal static List<string> ValidProducts = new List<string> { "PRODUCT,PRICE", "Tomato,0.75", "Aubergine,0.9", "Carrot,1" };
        internal static List<string> InvalidProducts = new List<string> { "Tomato","" };
        internal static List<string> EmptyProducts = new List<string> { "" };

        internal static List<string> ValidPurchases = new List<string> { "PRODUCT,QUANTITY", "Tomato,1", "Aubergine,2", "Carrot,3" };
        internal static List<string> InvalidPurchases = new List<string> { "Tomato", "" };
        internal static List<string> EmptyPurchases = new List<string> { "" };
    }
}