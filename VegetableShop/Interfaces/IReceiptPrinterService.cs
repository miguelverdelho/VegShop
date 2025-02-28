﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegetableShop.Base;
using VegetableShop.Models;

namespace VegetableShop.Interfaces
{
    public interface IReceiptPrinterService : IBaseSingleton
    {
        public void PrintReceipt(Receipt receipt);
    }
}
