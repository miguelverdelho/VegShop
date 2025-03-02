using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegetableShop.Models.Error_Handling
{
    public class InvalidFileInputException : Exception
    {
        public InvalidFileInputException(string? message) : base(message)
        {
        }
        public InvalidFileInputException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
