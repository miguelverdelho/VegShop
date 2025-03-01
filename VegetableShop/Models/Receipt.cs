namespace VegetableShop.Models
{
    public class Receipt
    {
        public List<ReceiptItem> Items { get; } = new List<ReceiptItem>();
        public List<string> AppliedOffers { get; } = new List<string>();
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
            AppliedOffers.Add($"{offerDescription} - Discount Applied: {discount:C2}");
        }

        public string GenerateReceipt()
        {
            var receiptText = "Receipt:\n";
            foreach (var item in Items)
            {
                receiptText += $"{item.Product} x{item.Quantity} @ {item.PricePerUnit:C2} = {item.SubTotal:C2}\n";
            }
            receiptText += $"Total: {_total:C2}\n";

            if (AppliedOffers.Any())
            {
                receiptText += "\nOffers Applied:\n";
                receiptText += string.Join("\n", AppliedOffers);
            }
            Console.WriteLine(receiptText);
            return receiptText;
        }
    }

}
