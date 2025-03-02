namespace VegetableShop.Models.Error_Handling
{
    public class InvalidOrderException : Exception
    {
        public InvalidOrderException(string? message) : base(message)
        {
        }
    }
}
