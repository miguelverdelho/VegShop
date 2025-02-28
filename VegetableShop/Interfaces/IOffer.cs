using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegetableShop.Models;

namespace VegetableShop.Interfaces
{
    public interface IOffer
    {
        public string RequiredProduct { get; }
        public void Apply(ref Receipt receipt);
    }
}
