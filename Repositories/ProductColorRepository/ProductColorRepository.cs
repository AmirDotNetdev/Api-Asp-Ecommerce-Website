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

namespace TestApi.Repositories.ProductColorRepository
{
    public class ProductColorRepository : IProductColorRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public ProductColorRepository(ApplicationDbContext dbContext , IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response_ProductColor> AddProductColor(Request_ProductColor productColor)
        {
            var productColorBaseModel = _mapper.Map<ProductColor>(productColor);
            await _dbContext.ProductColors.AddAsync(productColorBaseModel);
            await _dbContext.SaveChangesAsync();
            return new Response_ProductColor
            {
                isSuccess = true,
                Message = "Susscess",
                ProductColors = new List<ProductColor>
                {
                    productColorBaseModel
                }
            };

        }

        public async Task<Response_ProductColor> DeleteProductColor(int productColorId)
        {
            var productColors = await _dbContext.ProductColors.FirstOrDefaultAsync(x => x.Id == productColorId);
            if(productColors == null)
            {
                return new Response_ProductColor
                {
                    isSuccess = false,
                    Message = "No Product color"
                };
            }
            _dbContext.ProductColors.Remove(productColors);
            await _dbContext.SaveChangesAsync();
            return new Response_ProductColor
            {
                isSuccess = true,
                Message = "Susscess",
                ProductColors = new List<ProductColor>
                {
                    productColors
                }
            };

        }

        public async Task<Response_ProductColor> GetAllProductColors()
        {
            var productColors = await _dbContext.ProductColors.ToListAsync();
            if(productColors.Count == 0)
            {
                return new Response_ProductColor
                {
                    isSuccess = false,
                    Message = "No Product color"
                };
            }
            return new Response_ProductColor
            {
                isSuccess = true,
                Message = "Susscess",
                ProductColors = productColors
            };
        }

        public async Task<Response_ProductColor> GetProductColorById(int productColorId)
        {
            var productColor = await _dbContext.ProductColors.FirstOrDefaultAsync(x => x.Id == productColorId);
            if(productColor == null)
            {
                return new Response_ProductColor
                {
                    isSuccess = false,
                    Message = "No Product color"
                };
            }
            return new Response_ProductColor
            {
                isSuccess = true,
                Message = "Susscess",
                ProductColors = new List<ProductColor>
                {
                    productColor
                }
            };
        }

        public async Task<Response_ProductColor> UpdateProductColor(int productColorId, Request_ProductColor productColor)
        {
            var existingProductColor = await _dbContext.ProductColors.FirstOrDefaultAsync(x => x.Id == productColorId);
            if(productColor == null)
            {
                return new Response_ProductColor
                {
                    isSuccess = false,
                    Message = "No Product color"
                };
            }
            existingProductColor.Name = productColor.Name;
            
            await _dbContext.SaveChangesAsync();
            return new Response_ProductColor
            {
                isSuccess = true,
                Message = "Susscess",
                ProductColors = new List<ProductColor>
                {
                    existingProductColor
                }
            };

        }
    }
}