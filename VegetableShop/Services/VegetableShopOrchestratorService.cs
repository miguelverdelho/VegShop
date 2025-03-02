using Microsoft.Extensions.Logging;
using VegetableShop.Common;
using VegetableShop.Interfaces;

namespace VegetableShop.Services
{
    public class VegetableShopOrchestratorService : BaseService<VegetableShopOrchestratorService>, IVegetableShopOrchestratorService
    {
        private readonly IFileReaderService _fileReaderService;
        private readonly IShoppingService _shoppingService;
        private readonly IOfferService _offerService;
        private readonly IErrorHandlerService _errorHandlerService;

        public VegetableShopOrchestratorService(ILogger<VegetableShopOrchestratorService> logger,
            IFileReaderService fileReaderService,
            IShoppingService shoppingService,
            IOfferService offerService,
            IErrorHandlerService errorHandlerService) : base(logger)
        {
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
                _logger.LogInformation("Reading input files");
                var products = _fileReaderService.ReadProductsFromFile();
                var purchases = _fileReaderService.ReadPurchasesFromFile();

                // Step 2: Load data into Shopping Service
                _logger.LogInformation("Loading data into Shopping Service");
                _shoppingService.LoadProducts(products);
                _shoppingService.LoadPurchases(purchases);

                // Step 3: Validate order
                _logger.LogInformation("Validating order");
                _shoppingService.ValidateOrder();
                

                // Step 4: Process the order
                _logger.LogInformation("Processing order");
                var receipt = _shoppingService.ProcessOrder();

                // Step 5: Apply Offers 
                _logger.LogInformation("Applying offers");
                _offerService.ApplyOffers(receipt);

                // Step 6: Print the receipt
                _logger.LogInformation("Printing receipt");
                receipt.GenerateReceipt();
            }
            catch (Exception ex)
            {
                _errorHandlerService.HandleException(ex);
            }
        }
    }
}
