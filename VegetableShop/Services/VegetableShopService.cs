using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VegetableShop.Base;
using VegetableShop.Interfaces;

namespace VegetableShop.Services
{
    public class VegetableShopService : IVegetableShopService
    {
        private readonly ILogger<VegetableShopService> _logger;
        private readonly IFileReaderService _fileReaderService;
        private readonly IShoppingService _shoppingService;
        private readonly IReceiptPrinterService _receiptPrinterService;

        public VegetableShopService(ILogger<VegetableShopService> logger,
            IFileReaderService fileReaderService, 
            IShoppingService shoppingService,
            IReceiptPrinterService receiptPrinterService)
        {
            _fileReaderService = fileReaderService;
            _logger = logger;
            _shoppingService = shoppingService;
            _receiptPrinterService = receiptPrinterService;
        }

        public void Run()
        {
            //Read input files into shopping service
            _shoppingService.LoadProducts(_fileReaderService.ReadProductsFromFile());
            _shoppingService.LoadPurchases(_fileReaderService.ReadPurchasesFromFile());

            if(!_shoppingService.ValidateOrder())
            {
                throw new Exception("Invalid order");
            }

            var receipt = _shoppingService.ProcessOrder();

            receipt.GenerateReceipt();
        }
    }
}
