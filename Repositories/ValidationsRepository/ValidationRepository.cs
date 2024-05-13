using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestApi.Data;

namespace TestApi.Repositories.ValidationsRepository
{
    public class ValidationRepository : IValidationsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ValidationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> ValidateMainCategory(string mainCategoryName)
        {
            var mainCategory = await _dbContext.MainCategories.FirstOrDefaultAsync(x => x.Name.ToLower() == mainCategoryName.ToLower() );
            if(mainCategory != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ValidateMaterial(string materialName)
        {
            var material = await _dbContext.Materials.FirstOrDefaultAsync(x => x.Name.ToLower() == materialName.ToLower());
            if(material != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ValidateProductColor(string productColorName)
        {
             var productColor = await _dbContext.ProductColors.FirstOrDefaultAsync(x => x.Name.ToLower() == productColorName.ToLower());
            if(productColor != null)
            {
                return true;
            }
            return false;
        }
    }
}