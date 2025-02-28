using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VegetableShop.Base;
using VegetableShop.Interfaces;

namespace VegetableShop
{
    public class VegetableShopService
    {
        private readonly ILogger<VegetableShopService> _logger;
        private readonly IFileReaderService _fileReaderService;

        public VegetableShopService(IFileReaderService fileReaderService, ILogger<VegetableShopService> logger)
        {
            _fileReaderService = fileReaderService;
            _logger = logger;
        }

        public void Run()
        {
            var products = _fileReaderService.LoadProducts();
            var purchases = _fileReaderService.LoadPurchases();
        }
    }
}
