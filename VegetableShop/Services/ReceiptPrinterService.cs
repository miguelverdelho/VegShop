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
    public class ReceiptPrinterService : BaseService<ReceiptPrinterService>, IReceiptPrinterService
    {
        public ReceiptPrinterService(ILogger<ReceiptPrinterService> logger) : base(logger) { }

        public void PrintReceipt(Receipt receipt) => Console.WriteLine(receipt.ToString());
    }
}
