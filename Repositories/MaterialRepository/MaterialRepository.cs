using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.DTOs.ProductDtos.Request;
using TestApi.DTOs.ProductDtos.Response;
using TestApi.Models.ProductModels;

namespace TestApi.Repositories.MaterialRepository
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public MaterialRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Response_ProductMaterial> GetAllProductMaterial()
        {
            var productMaterials = await _dbContext.Materials.ToListAsync();
            if(productMaterials.Count < 0 )
            {
                return new Response_ProductMaterial
                {
                    isSuccess = false,
                    Message = "Theres no Material available"
                };

            }
            return new Response_ProductMaterial
            {
                isSuccess = true,
                Message = "This is your materials",
                Materials = productMaterials
            };
        }

        public async Task<Response_ProductMaterial> GetProductMaterialById(int productMaterialId)
        {
            var productMaterials = await _dbContext.Materials.FirstOrDefaultAsync(x => x.Id == productMaterialId);
            if(productMaterials == null )
            {
                return new Response_ProductMaterial
                {
                    isSuccess = false,
                    Message = "Theres no Material available"
                };

            }
            return new Response_ProductMaterial
            {
                isSuccess = true,
                Message = "This is your materials",
                Materials = new List<Material>
                {
                    productMaterials
                }
            };
        }
        public Task<Response_ProductMaterial> AddProductMaterial(Request_ProductMaterial productMaterialDto)
        {
            throw new NotImplementedException();
        }

        public Task<Response_ProductMaterial> DeleteProductMaterial(int productMaterialId)
        {
            throw new NotImplementedException();
        }

        

        public Task<Response_ProductMaterial> UpdateProductMaterial(int productMaterialId, Request_ProductMaterial Request_ProductMaterialDto)
        {
            throw new NotImplementedException();
        }
    }
}