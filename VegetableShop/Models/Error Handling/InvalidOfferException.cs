namespace VegetableShop.Models.Error_Handling
{
    public class InvalidOfferException : Exception
    {
        public InvalidOfferException(string? message) : base(message)
        {
        }
    }
}
