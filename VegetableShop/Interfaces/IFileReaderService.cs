namespace VegetableShop.Interfaces
{
    public interface IFileReaderService : IBaseSingleton
    {
        public Dictionary<string, decimal> ReadProductsFromFile();
        public Dictionary<string, int> ReadPurchasesFromFile();
    }
}
