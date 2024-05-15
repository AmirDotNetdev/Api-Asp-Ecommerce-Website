using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Data.CustomModels
{
    public class Model_BaseImageCustom
    {
        public long Id { get; set; }
        public DateTime AddedOn { get; set; }
        public int BaseProductId { get; set; }
        public string staticPath { get; set; }
        
    }
}