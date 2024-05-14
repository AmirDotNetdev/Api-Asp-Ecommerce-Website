using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.DTOs.ProductDtos.Request;
using TestApi.DTOs.ProductDtos.Response;
using TestApi.Models.ProductModels;

namespace TestApi.Repositories.MaterialRepository
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        public MaterialRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response_ProductMaterial> GetAllProductMaterial()
        {
            var productMaterials = await _dbContext.Materials.ToListAsync();
            if(productMaterials.Count == 0 )
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
        public async Task<Response_ProductMaterial> AddProductMaterial(Request_ProductMaterial productMaterialDto)
        {
            
            var productMaterialBaseModel = _mapper.Map<Material>(productMaterialDto);
            await _dbContext.Materials.AddAsync(productMaterialBaseModel);
            await _dbContext.SaveChangesAsync();
            return new Response_ProductMaterial
            {
                isSuccess = true,
                Message = " successfuly added",
                Materials = new List<Material>
                {
                    productMaterialBaseModel
                }
            };

        }

        public async Task<Response_ProductMaterial> DeleteProductMaterial(int productMaterialId)
        {
            var productMaterial = await _dbContext.Materials.FirstOrDefaultAsync(x => x.Id == productMaterialId);
            if(productMaterial == null)
            {
                return new Response_ProductMaterial
                {
                    isSuccess = false,
                    Message = "There is no product material with this id"
                };
            }
            _dbContext.Materials.Remove(productMaterial);
            await _dbContext.SaveChangesAsync();
            return new Response_ProductMaterial
            {
                isSuccess = false,
                Message = "Successfuly removed"
            };
        }

        

        public async Task<Response_ProductMaterial> UpdateProductMaterial(int productMaterialId, Request_ProductMaterial Request_ProductMaterialDto)
        {
            
            var productMaterial = await _dbContext.Materials.FirstOrDefaultAsync(x => x.Id == productMaterialId);
            if(productMaterial == null)
            {
                return new Response_ProductMaterial
                {
                    isSuccess = false,
                    Message = "Wrong Id provided"
                };
            }
            productMaterial.Name = Request_ProductMaterialDto.Name;
            await _dbContext.SaveChangesAsync();
            return new Response_ProductMaterial
            {
                isSuccess = true,
                Message = "Successfuly Updated",
                Materials = new List<Material>
                {
                    productMaterial
                }
            };
        }
    }
}