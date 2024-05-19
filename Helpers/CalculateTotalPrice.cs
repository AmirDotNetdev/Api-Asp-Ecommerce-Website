using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace TestApi.Helpers
{
    public class CalculateTotalPriceDto
    {
        public decimal Price { get; set; }
        public int Discount { get; set; }
    }
    public static class CalculateTotalPrice
    {
        public static decimal CalculateDiscount(CalculateTotalPriceDto model)
        {
            decimal totalPrice ;
            decimal decimalTotalPrice;
            if(model.Discount == 0)
            {

                decimalTotalPrice = model.Price;
                return decimalTotalPrice;

            }
            

            totalPrice = model.Price - (model.Price * model.Discount / 100);
            decimalTotalPrice = decimal.Round(totalPrice, 2);
            return decimalTotalPrice;
        }
    }
}