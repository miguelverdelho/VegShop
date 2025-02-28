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

        public Dictionary<string, decimal> ReadProductsFromFile()
        {
            _logger.LogInformation($"Loading products from {_productsFilePath}");

            var products = ParseLines(_productsFilePath, decimal.Parse);

            if (products.Count == 0)
                throw new Exception("Products file is empty");

            _logger.LogInformation($"Loaded {products.Count} products");

            return products;
        }

        public Dictionary<string, int> ReadPurchasesFromFile()
        {
            _logger.LogInformation($"Loading products from {_purchasesFilePath}");

            var purchases = ParseLines(_purchasesFilePath, int.Parse);

            if(purchases.Count == 0) 
                throw new Exception("Purchases file is empty");

            _logger.LogInformation($"Loaded {purchases.Count} purchases");

            return purchases;
        }

        private Dictionary<string, T> ParseLines<T>(string filePath, Func<string, T> parseValue)
        {
            var dictionary = new Dictionary<string, T>();
            try
            {
                foreach (var line in File.ReadLines(filePath).Skip(1)) // Skip header
                {
                    var parts = line.Split(',');
                    dictionary[parts[0].Trim()] = parseValue(parts[1].Trim());
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error parsing lines", ex);
            }
            return dictionary;
        }
    }
}
