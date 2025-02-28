
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using VegetableShop.Base;
using VegetableShop.Interfaces;

namespace VegetableShop.Services
{
    public class FileReaderService : BaseService<FileReaderService>, IFileReaderService
    {
        private readonly string _productsFilePath = string.Empty;
        private readonly string _purchasesFilePath  = string.Empty;
        public FileReaderService(ILogger<FileReaderService> logger, IConfiguration configuration) : base(logger) 
        {            
            var baseDirectory = Directory.GetCurrentDirectory();
            var fileConfigSection = configuration.GetSection("FileSettings") ?? throw new Exception("FileSettings not found in configuration");
            var relativeProductFilePath = fileConfigSection["ProductsFilePath"] ?? throw new Exception("ProductsFilePath not found in configuration");
            var relativepurchasesFilePath = fileConfigSection["PurchasesFilePath"] ?? throw new Exception("PurchasesFilePath not found in configuration");

            _productsFilePath = Path.Combine(baseDirectory, relativeProductFilePath);
            _purchasesFilePath = Path.Combine(baseDirectory, relativepurchasesFilePath);

            if (!File.Exists(_productsFilePath) || !File.Exists(_purchasesFilePath))
            {
                throw new FileNotFoundException("Input File not found.");
            }
            
        }

        public Dictionary<string, decimal> LoadProducts()
        {
            _logger.LogInformation($"Loading products from {_productsFilePath}");

            var products = new Dictionary<string, decimal>();
            try
            {
                foreach (var line in File.ReadLines(_productsFilePath).Skip(1)) // Skip header
                {
                    var parts = line.Split(',');
                    products[parts[0].Trim()] = decimal.Parse(parts[1].Trim());
                }
            }
            catch(Exception ex)
            {
                throw new ArgumentException("Error loading products", ex);
            }

            _logger.LogInformation($"Loaded {products.Count} products");

            return products;
        }

        public Dictionary<string, int> LoadPurchases()
        {
            _logger.LogInformation($"Loading products from {_purchasesFilePath}");

            var purchases = new Dictionary<string, int>();
            try
            {
                foreach (var line in File.ReadLines(_purchasesFilePath).Skip(1)) // Skip header
                {
                    var parts = line.Split(',');
                    purchases[parts[0].Trim()] = int.Parse(parts[1].Trim());
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error loading purchases", ex);
            }

            _logger.LogInformation($"Loaded {purchases.Count} purchases");

            return purchases;
        }
    }
}
