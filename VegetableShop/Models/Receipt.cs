using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegetableShop.Models
{
    public class Receipt
    {
        public decimal Total { get; set; }
        public List<ReceiptItem> Items = [];        
    }

    public class ReceiptItem
    {
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class Offer
    {
        public decimal Discount { get; set; }
        public string Product { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
