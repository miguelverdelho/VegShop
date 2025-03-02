using Microsoft.Extensions.Logging;

namespace VegetableShop.Common
{
    public abstract class BaseService<T>(ILogger<T> logger) where T : class
    {
        protected readonly ILogger<T> _logger = logger;
    }
}
