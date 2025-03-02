using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegetableShop.Interfaces;
using VegetableShop.Models;
using VegetableShop.Services;

namespace VegetableShop.UnitTests.Services
{
    public class VegetableShopOrchestratorServiceUnitTests
    {
        private readonly Mock<ILogger<VegetableShopOrchestratorService>> _mockLogger;
        private readonly Mock<IFileReaderService> _mockFileReaderService;
        private readonly Mock<IShoppingService> _mockShoppingService;
        private readonly Mock<IOfferService> _mockOfferService;
        private readonly Mock<IErrorHandlerService> _mockErrorHandlerService;
        private readonly VegetableShopOrchestratorService _orchestratorService;

        public VegetableShopOrchestratorServiceUnitTests()
        {
            _mockLogger = new Mock<ILogger<VegetableShopOrchestratorService>>();
            _mockFileReaderService = new Mock<IFileReaderService>();
            _mockShoppingService = new Mock<IShoppingService>();
            _mockOfferService = new Mock<IOfferService>();
            _mockErrorHandlerService = new Mock<IErrorHandlerService>();

            _orchestratorService = new VegetableShopOrchestratorService(
                _mockLogger.Object,
                _mockFileReaderService.Object,
                _mockShoppingService.Object,
                _mockOfferService.Object,
                _mockErrorHandlerService.Object
            );
        }

        [Fact]
        public void Run_ExecutesAllStepsSuccessfully()
        {
            // Arrange
            var mockProducts = new Dictionary<string, decimal> { { "Tomato", 0.75m }, { "Carrot", 1.00m } };
            var mockPurchases = new Dictionary<string, int> { { "Tomato", 3 }, { "Carrot", 2 } };
            var mockReceipt = new Receipt();

            _mockFileReaderService.Setup(s => s.ReadProductsFromFile()).Returns(mockProducts);
            _mockFileReaderService.Setup(s => s.ReadPurchasesFromFile()).Returns(mockPurchases);
            _mockShoppingService.Setup(s => s.ProcessOrder()).Returns(mockReceipt);
            
            // Act
            _orchestratorService.Run();

            // Assert
            _mockFileReaderService.Verify(s => s.ReadProductsFromFile(), Times.Once);
            _mockFileReaderService.Verify(s => s.ReadPurchasesFromFile(), Times.Once);
            _mockShoppingService.Verify(s => s.LoadProducts(mockProducts), Times.Once);
            _mockShoppingService.Verify(s => s.LoadPurchases(mockPurchases), Times.Once);
            _mockShoppingService.Verify(s => s.ValidateOrder(), Times.Once);
            _mockShoppingService.Verify(s => s.ProcessOrder(), Times.Once);
            _mockOfferService.Verify(s => s.ApplyOffers(mockReceipt), Times.Once);
            _mockErrorHandlerService.Verify(s => s.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        [Fact]
        public void Run_HandlesException_WhenErrorOccurs()
        {
            // Arrange
            var exception = new Exception("Test Exception");
            _mockFileReaderService.Setup(s => s.ReadProductsFromFile()).Throws(exception);

            // Act
            _orchestratorService.Run();

            // Assert
            _mockErrorHandlerService.Verify(s => s.HandleException(exception), Times.Once);
        }
    }
}
