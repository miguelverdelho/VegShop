using Microsoft.Extensions.Logging;
using VegetableShop.Common;
using VegetableShop.Interfaces;
using VegetableShop.Models.Error_Handling;

namespace VegetableShop.Services
{
    public class ErrorHandlerService : BaseService<ErrorHandlerService>, IErrorHandlerService
    {
        public ErrorHandlerService(ILogger<ErrorHandlerService> logger) : base(logger) { }

        public void HandleException(Exception ex)
        {
            // The place to implement different error handling strategies for different exceptions
            if (ex is FileNotFoundException)
            {
                _logger.LogError($"File Error: {ex.Message}");
            }
            else if (ex is FormatException)
            {
                _logger.LogError($"Invalid Data Format: {ex.Message}");
            }
            else if(ex is ArgumentException)
            {
                _logger.LogError($"Invalid Argument Format: {ex.Message}");
            }
            else if (ex is InvalidOfferException)
            {
                _logger.LogError($"Invalid Offer: {ex.Message}");
            }
            else
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
            }

            File.AppendAllText("error.log", $"{DateTime.UtcNow}: {ex}\n");
        }
    }
}
