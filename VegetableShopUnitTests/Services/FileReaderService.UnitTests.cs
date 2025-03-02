using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using VegetableShop.Services;
using VegetableShop.UnitTests.Services.TestData;

namespace VegetableShopUnitTests.Services
{
    public class FileReaderServiceUnitTests
    {
        private const string FILE_SETTINGS = "FileSettings";
        private const string PRODUCTS_FILE_SETTING = "ProductsFilePath";
        private const string PURCHASES_FILE_SETTING = "PurchasesFilePath";
        private FileReaderService? _fileReaderService;
        private Mock<ILogger<FileReaderService>>? _mockLogger = new();
        private Mock<IConfiguration> _mockConfiguration = new();
        private string _productsTestFilePath;
        private string _purchasesTestFilePath;

        public FileReaderServiceUnitTests()
        {
            _productsTestFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestResources", "ProductsTestFile.csv");
            _purchasesTestFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestResources", "PurchasesTestFile.csv");
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
            var purchases = FileReaderServiceMockData.ValidPurchases;
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
            var purchases = FileReaderServiceMockData.EmptyPurchases;
            File.WriteAllLines(_productsTestFilePath, purchases);

            // Act & Assert
            Assert.Throws<Exception>(() => _fileReaderService?.ReadPurchasesFromFile());
        }

        [Fact]
        public void ReadPurchasesFromFile_ThrowsArgumentException_InvalidFile()
        {
            // Arrange
            var purchases = FileReaderServiceMockData.InvalidPurchases;
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
            var products = FileReaderServiceMockData.ValidProducts;
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
            var products = FileReaderServiceMockData.InvalidProducts;
            File.WriteAllLines(_productsTestFilePath, products);

            // Act && Assert
            Assert.Throws<ArgumentException>(() => _fileReaderService?.ReadProductsFromFile());
        }

        [Fact]
        public void ReadProductsFromFile_ThrowsEmptyListException_EmptyValidFile()
        {
            // Arrange
            var products = FileReaderServiceMockData.EmptyProducts;
            File.WriteAllLines(_productsTestFilePath, products);

            // Act & Assert
            Assert.Throws<Exception>(() => _fileReaderService?.ReadProductsFromFile());
        }
        #endregion

        #region Helpers
        public FileReaderService CreateDefaultService() => new FileReaderService(_mockLogger!.Object, _mockConfiguration.Object);
        #endregion
    }
}