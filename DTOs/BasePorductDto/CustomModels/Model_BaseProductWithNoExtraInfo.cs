using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTOs.BasePorductDto.CustomModels
{
    public class Model_BaseProductWithNoExtraInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MainCategoryId { get; set; }
        public int MaterialId { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}