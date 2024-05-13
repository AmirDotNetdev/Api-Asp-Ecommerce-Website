using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Repositories.ValidationsRepository
{
    public interface IValidationsRepository
    {
        public Task<bool> ValidateMaterial(string materialName);
        public Task<bool> ValidateMainCategory(string mainCategoryName);
        public Task<bool> ValidateProductColor(string productColorName);
    }
}