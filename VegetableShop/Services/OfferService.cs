using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegetableShop.Base;
using VegetableShop.Interfaces;
using VegetableShop.Models;

namespace VegetableShop.Services
{
    public class OfferService : BaseService<OfferService>, IOfferService
    {
        List<IOffer> _offers = new List<IOffer>();

        public OfferService(ILogger<OfferService> logger) : base(logger)
        {
        }
    }
}
