using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Data.CustomModels;
using TestApi.DTOs.BasePorductDto.Request;
using TestApi.Models.ProductModels;

namespace TestApi.DtoConvertations
{
    public static class ConvertToBase
    {
        public static BaseProduct ConvetToBaseProdcutFromDto (this Request_BaseProduct productCustom)
        {
            decimal totalPrice;
            decimal decimalTotalPrice;

            if(productCustom.Discount > 0)
            {
                totalPrice = (productCustom.Price * productCustom.Discount / 100) - productCustom.Price;
                decimalTotalPrice = decimal.Round(totalPrice,2);
            }
            else
            {
                totalPrice = productCustom.Price;
                decimalTotalPrice = decimal.Round(totalPrice, 2);
            }
            return new BaseProduct
            {
                Description = productCustom.Description,
                Discount = productCustom.Discount,
                Name = productCustom.Name,
                MainCategoryId = productCustom.MainCategoryId,
                MaterialId = productCustom.MaterialId,
                Price = productCustom.Price,
                TotalPrice = totalPrice,
            };
        }
    }
}