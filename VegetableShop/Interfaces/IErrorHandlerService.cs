namespace VegetableShop.Interfaces
{
    public interface IErrorHandlerService : IBaseTransient
    {
        void HandleException(Exception ex);
    }
}
