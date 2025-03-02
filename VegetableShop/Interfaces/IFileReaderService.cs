namespace VegetableShop.Interfaces
{
    public interface IFileReaderService : IBaseTransient
    {
        public Dictionary<string, decimal> ReadProductsFromFile();
        public Dictionary<string, int> ReadPurchasesFromFile();
    }
}
