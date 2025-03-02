namespace VegetableShop.Interfaces
{
    public interface IErrorHandlerService : IBaseSingleton
    {
        void HandleException(Exception ex);
    }
}
