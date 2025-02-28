using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegetableShop.Base
{
    public class BaseService<T>(ILogger<T> logger) where T : class
    {
        protected readonly ILogger<T> _logger = logger;
    }
}
