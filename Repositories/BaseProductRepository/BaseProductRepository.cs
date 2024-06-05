using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.Data.CustomModels;
using TestApi.DtoConvertations;
using TestApi.DTOs.BasePorductDto.CustomModels;
using TestApi.DTOs.BasePorductDto.Request;
using TestApi.DTOs.BasePorductDto.Response;
using TestApi.Helpers;
using TestApi.Models.ProductModels;

namespace TestApi.Repositories.BaseProductRepository
{
    public class BaseProductRepository : IBaseProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        
        public BaseProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Response_BaseProduct> AddBaseProduct(Request_BaseProduct baseProduct)
        {
            var baseProductDb = baseProduct.ConvetToBaseProdcutFromDto();
            if(baseProductDb == null)
            {
                return new Response_BaseProduct()
                {
                    isSuccess = false,
                    Message = "No request",

                };
            }
            await _dbContext.BaseProducts.AddAsync(baseProductDb);
            await _dbContext.SaveChangesAsync();
            var convertedToDto = baseProductDb.ConvertToDtoProductNoInfo();
            return new Response_BaseProduct
            {
                isSuccess = true,
                Message = "Successfuly added",
                baseProducts = new List<Model_BaseProductWithNoExtraInfo>
                {
                    convertedToDto
                }
            };
        }

        public async Task<IEnumerable<BaseProduct>> GetAllAsync()
        {
            var baseProducts = await _dbContext.BaseProducts.Include(p => p.MainCategory).Include(p => p.Material).Include(p => p.productVariants).ThenInclude(p => p.ProductColor)
            .Include(p => p.productVariants).ThenInclude(p => p.ProductSize)
            .Include(p => p.imageBases).ToListAsync();

            return baseProducts;
        }

        public async Task<IEnumerable<Model_BaseProductCustom>> GetAllWithFullInfoAsync()
        {
            var baseProducts = await _dbContext.BaseProducts.Include(p => p.MainCategory).Include(p => p.Material).Include(p => p.productVariants).ThenInclude(p => p.ProductColor)
            .Include(p => p.productVariants).ThenInclude(p => p.ProductSize)
            .Include(p => p.imageBases).ToListAsync();

            var baseProductCustom = baseProducts.ConvertToDtoListCustomProduct();
            return baseProductCustom;
        }

        public async Task<Response_BaseProductWithPaging> GetAllWithFullInfoByPages(int pageNumber, int pageSize)
        {
            var baseProductWithPaging = new Response_BaseProductWithPaging();
            float numberpp = (float)pageNumber;
            var totalPages = Math.Ceiling((await GetAllAsync()).Count()/numberpp);
            int totPages = (int)totalPages;
             var baseProducts = await _dbContext.BaseProducts.Include(p => p.MainCategory).Include(p => p.Material).Include(p => p.productVariants).ThenInclude(p => p.ProductColor)
            .Include(p => p.productVariants).ThenInclude(p => p.ProductSize)
            .Include(p => p.imageBases).Skip((pageNumber -1)*pageSize).ToListAsync();
            // convert To dto

            var customBaseProducts = baseProducts.ConvertToDtoListCustomProduct();
            baseProductWithPaging.BaseProduct = customBaseProducts.ToList();
            baseProductWithPaging.TotalPages = totPages;
            return baseProductWithPaging;

        }

        public async Task<Response_BaseProductWithFullInfo> GetByIdWithFullInfo(int baseProductId)
        {
             var existingBaseProduct = await _dbContext.BaseProducts.Include(p => p.MainCategory).Include(p => p.Material).Include(p => p.productVariants).ThenInclude(p => p.ProductColor)
            .Include(p => p.productVariants).ThenInclude(p => p.ProductSize)
            .Include(p => p.imageBases).FirstOrDefaultAsync(x => x.Id == baseProductId);

            if(existingBaseProduct is null)
            {
                return new Response_BaseProductWithFullInfo()
                {
                    isSuccess = false,
                    Message = "No base product with given Id",

                };

            }
            // convert to dto object
            var baseProductCustom = existingBaseProduct.ConvertToDtoCustomProduct();

            return new Response_BaseProductWithFullInfo
            {
                isSuccess = true,
                Message = "base product custom is retrived",
                baseProduct = baseProductCustom
            };


        }

        public async Task<Response_BaseProduct> GetByIdWithNoInfo(int baseProductId)
        {
            var existingBaseProduct = await _dbContext.BaseProducts.FirstOrDefaultAsync(x => x.Id == baseProductId);
            if(existingBaseProduct is null)
            {
                return new Response_BaseProduct()
                {
                    isSuccess = false,
                    Message = "No product found with this id",

                };
            }
            // convert to dto

            var baseProductCustom = existingBaseProduct.ConvertToDtoProductNoInfo();
            var listBaseProductCustom = new List<Model_BaseProductWithNoExtraInfo>();
            listBaseProductCustom.Add(baseProductCustom);
            return new Response_BaseProduct
            {
                isSuccess = true,
                Message = "Successfuly",
                baseProducts = listBaseProductCustom
            };
        }

        public Task<IEnumerable<Model_BaseProductCustom>> GetProductSearch(string searchText)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetProductSearchSuggestions(string searchText)
        {
            throw new NotImplementedException();
        }

        public Task<Response_BaseProductWithPaging> GetProductSearchWithPaging(string searchText, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<Response_BaseProduct> RemoveBaseProduct(int baseProductId)
        {
            var existingBaseProduct = await _dbContext.BaseProducts.FirstOrDefaultAsync(x => x.Id == baseProductId);
            if (existingBaseProduct == null)
            {
                return new Response_BaseProduct
                {
                    isSuccess = false,
                    Message = "Not found",

                };
            }
            _dbContext.BaseProducts.Remove(existingBaseProduct);
            await _dbContext.SaveChangesAsync();
            var baseProductWithNoInfo = existingBaseProduct.ConvertToDtoProductNoInfo();
            return new Response_BaseProduct
            {
                isSuccess = true,
                Message = "Successful",
                baseProducts = new List<Model_BaseProductWithNoExtraInfo>
                {
                    baseProductWithNoInfo
                }
            };
        }

        public Task<IEnumerable<Model_BaseImageCustom>> SearchProducts(int[] materialsIds, int[] mainCategoryIds, int[] productColorIds, int[] productSizesIds)
        {
            throw new NotImplementedException();
        }

        public async Task<Response_BaseProduct> UpdateBaseProduct(int baseProductId, Request_BaseProduct baseProduct)
        {
            var existingBaseProduct = await _dbContext.BaseProducts.FirstOrDefaultAsync(x => x.Id == baseProductId);
            if (existingBaseProduct is null)
            {
                return new Response_BaseProduct
                {
                    isSuccess = false,
                    Message = "no product found with this id"
                };
            }
            existingBaseProduct.Description = baseProduct.Description;
            existingBaseProduct.Discount = baseProduct.Discount;
            existingBaseProduct.MainCategoryId = baseProduct.MainCategoryId;
            existingBaseProduct.MaterialId = baseProduct.MaterialId;
            existingBaseProduct.Name = baseProduct.Name;
            // calculate totla price
            existingBaseProduct.TotalPrice = baseProduct.Price - (baseProduct.Price * baseProduct.Discount / 100);

            await _dbContext.SaveChangesAsync();
            var convertedModel = existingBaseProduct.ConvertToDtoProductNoInfo();
            return new Response_BaseProduct
            {
                isSuccess = true,
                Message = "Successfuly Updated",
                baseProducts = new List<Model_BaseProductWithNoExtraInfo>
                {
                    convertedModel
                }
            };
        }

        public async Task<Response_BaseProduct> UpdateBaseProductDiscount(int baseProductId, Request_BaseProductDiscount baseProductDiscount)
        {
            var existingBaseProduct = await _dbContext.BaseProducts.FirstOrDefaultAsync(x => x.Id == baseProductId);
            if(existingBaseProduct is null)
            {
                return new Response_BaseProduct
                {
                    isSuccess = false,
                    Message = "No product found",

                };
            }
            if(baseProductDiscount.Discount > 99 || baseProductDiscount.Discount < 0)
            {
                return new Response_BaseProduct
                {
                    isSuccess = false,
                    Message = "wrong discount",

                }; 
            }
            existingBaseProduct.Discount = baseProductDiscount.Discount;
            
            var totalPrice = CalculateTotalPrice.CalculateDiscount(new CalculateTotalPriceDto {Discount = baseProductDiscount.Discount, Price = existingBaseProduct.Price});
            
            existingBaseProduct.TotalPrice = totalPrice;
            await _dbContext.SaveChangesAsync();
            var convertedModel = existingBaseProduct.ConvertToDtoProductNoInfo();
            return new Response_BaseProduct
            {
                isSuccess = true,
                Message = "successfuly updated",
                baseProducts = new List<Model_BaseProductWithNoExtraInfo>
                {
                    convertedModel
                }
            };
        }

        public async Task<Response_BaseProduct> UpdateBaseProductMainCategory(int baseProductId, Request_BaseProductMainCategory baseProductMainCategory)
        {
            var existingBaseProduct = await _dbContext.BaseProducts.FirstOrDefaultAsync(x => x.Id == baseProductId);
            if(existingBaseProduct == null)
            {
                return new Response_BaseProduct
                {
                    isSuccess = false,
                    Message = "Not found",

                };
            }
            existingBaseProduct.MainCategoryId = baseProductMainCategory.MainCategoryId;
            await _dbContext.SaveChangesAsync();
            var baseProductWithNoInfo = existingBaseProduct.ConvertToDtoProductNoInfo();
            return new Response_BaseProduct
            {
                isSuccess = true,
                Message = "Successful",
                baseProducts = new List<Model_BaseProductWithNoExtraInfo>
                {
                    baseProductWithNoInfo
                }
            };
        }

        public async Task<Response_BaseProduct> UpdateBaseProductMaterial(int baseProductId, Request_BaseProductMaterial baseProductMaterial)
        {
            var existingBaseProduct = await _dbContext.BaseProducts.FirstOrDefaultAsync(x => x.Id == baseProductId);
            if (existingBaseProduct == null)
            {
                return new Response_BaseProduct
                {
                    isSuccess = false,
                    Message = "Not found",

                };
            }
            existingBaseProduct.MainCategoryId = baseProductMaterial.MaterialId;
            await _dbContext.SaveChangesAsync();
            var baseProductWithNoInfo = existingBaseProduct.ConvertToDtoProductNoInfo();
            return new Response_BaseProduct
            {
                isSuccess = true,
                Message = "Successful",
                baseProducts = new List<Model_BaseProductWithNoExtraInfo>
                {
                    baseProductWithNoInfo
                }
            };
        }

        public async Task<Response_BaseProduct> UpdateBaseProductPrice(int baseProductId, Request_BaseProductPrice baseProductPrice)
        {
            var existingProduct = await _dbContext.BaseProducts.FirstOrDefaultAsync(x => x.Id == baseProductId);
            if(existingProduct == null)
            {
                return new Response_BaseProduct
                {
                    isSuccess = false,
                    Message = "Not found"
                };
            }
            existingProduct.Price = baseProductPrice.Price;

            decimal decimalTotalPrice;
            if(existingProduct.Discount == 0)
            {
                var totalPrice = existingProduct.Price;
                decimalTotalPrice = decimal.Round(totalPrice, 2);
            }
            else
            {
                var totalPrice = existingProduct.Price - (existingProduct.Price * existingProduct.Discount/100);
                decimalTotalPrice =  decimal.Round(totalPrice, 2);
            }
            existingProduct.TotalPrice = decimalTotalPrice;
            await _dbContext.SaveChangesAsync();
            var baseProductWithNoInfo = existingProduct.ConvertToDtoProductNoInfo();

            return new Response_BaseProduct
            {
                isSuccess = true,
                Message = "Successful",
                baseProducts = new List<Model_BaseProductWithNoExtraInfo>()
                {
                    baseProductWithNoInfo,
                }
            };

        }
    }
}