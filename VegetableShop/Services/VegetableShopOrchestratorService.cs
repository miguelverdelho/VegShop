using Microsoft.Extensions.Logging;
using VegetableShop.Interfaces;

namespace VegetableShop.Services
{
    public class VegetableShopOrchestratorService : IVegetableShopOrchestratorService
    {
        private readonly ILogger<VegetableShopOrchestratorService> _logger;
        private readonly IFileReaderService _fileReaderService;
        private readonly IShoppingService _shoppingService;
        private readonly IOfferService _offerService;
        private readonly IErrorHandlerService _errorHandlerService;

        public VegetableShopOrchestratorService(ILogger<VegetableShopOrchestratorService> logger,
            IFileReaderService fileReaderService,
            IShoppingService shoppingService,
            IOfferService offerService,
            IErrorHandlerService errorHandlerService)
        {
            _logger = logger;
            _fileReaderService = fileReaderService;
            _shoppingService = shoppingService;
            _offerService = offerService;
            _errorHandlerService = errorHandlerService;
        }

        public void Run()
        {
            try
            {
                // Step 1: Read input files
                var products = _fileReaderService.ReadProductsFromFile();
                var purchases = _fileReaderService.ReadPurchasesFromFile();

                // Step 2: Load data into Shopping Service
                _shoppingService.LoadProducts(products);
                _shoppingService.LoadPurchases(purchases);

                // Step 3: Validate order
                if (!_shoppingService.ValidateOrder())
                {
                    throw new Exception("Invalid order");
                }

                // Step 4: Process the order
                var receipt = _shoppingService.ProcessOrder();

                // Step 5: Apply Offers 
                _offerService.ApplyOffers(receipt);

                // Step 6: Print the receipt
                receipt.GenerateReceipt();
            }
            catch (Exception ex)
            {
                _errorHandlerService.HandleException(ex);
            }
        }
    }
}
