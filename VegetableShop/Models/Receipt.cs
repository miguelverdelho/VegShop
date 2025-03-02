using System.Text;

namespace VegetableShop.Models
{
    public class Receipt
    {
        public List<ReceiptItem> Items { get; } = [];
        public List<string> AppliedOffers { get; } = [];
        private decimal _total;

        public void AddItem(string product, int quantity, decimal pricePerUnit)
        {
            var item = new ReceiptItem(product, quantity, pricePerUnit);
            Items.Add(item);
            _total += item.SubTotal;
        }

        public void ApplyDiscount(string product, decimal discount, string offerDescription)
        {
            _total -= discount;
            AppliedOffers.Add($"{offerDescription} Discount Applied: {discount:C2}");
        }

        public string GenerateReceipt()
        {
            var receiptBuilder = new StringBuilder();
            receiptBuilder.AppendLine("Receipt:");

            foreach (var item in Items)
            {
                receiptBuilder.AppendLine($"{item.Product} x{item.Quantity} @ {item.PricePerUnit:C2} = {item.SubTotal:C2}");
            }

            receiptBuilder.AppendLine($"Total: {_total:C2}");

            if (AppliedOffers.Count != 0)
            {
                receiptBuilder.AppendLine("\nOffers Applied:");
                foreach (var offer in AppliedOffers)
                {
                    receiptBuilder.AppendLine(offer);
                }
            }

            string receiptText = receiptBuilder.ToString();
            Console.WriteLine(receiptText);
            return receiptText;
        }
    }

}
