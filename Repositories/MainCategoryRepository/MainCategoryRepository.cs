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

namespace TestApi.Repositories.MainCategoryRepository
{
    public class MainCategoryRepository : IMainCategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public MainCategoryRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response_MainCategory> GetMainCategory()
        {
            var MainCategorys = await _dbContext.MainCategories.ToListAsync();
            if(MainCategorys.Count < 0 )
            {
                return new Response_MainCategory
                {
                    isSuccess = false,
                    Message = "Theres no MainCategory available"
                };

            }
            return new Response_MainCategory
            {
                isSuccess = true,
                Message = "This is your MainCategory",
                MainCategory = MainCategorys
            };
        }

        public async Task<Response_MainCategory> GetMainCategoryById(int MainCategoryId)
        {
            var MainCategorys = await _dbContext.MainCategories.FirstOrDefaultAsync(x => x.Id == MainCategoryId);
            if(MainCategorys== null )
            {
                return new Response_MainCategory
                {
                    isSuccess = false,
                    Message = "Theres no MainCategory available"
                };

            }
            return new Response_MainCategory
            {
                isSuccess = true,
                Message = "This is your MainCategory",
                MainCategory = new List<MainCategory>
                {
                    MainCategorys
                }
            };
        }

        public async Task<Response_MainCategory> AddMainCategory(Request_MainCategory MainCategoryDto)
        {
            var baseMainCategoryModel = _mapper.Map<MainCategory>(MainCategoryDto);
            await _dbContext.MainCategories.AddAsync(baseMainCategoryModel);
            await _dbContext.SaveChangesAsync();
            return new Response_MainCategory
            {
                isSuccess = true,
                MainCategory = new List<MainCategory>()
                {
                    baseMainCategoryModel
                },
            };
        }

        public async Task<Response_MainCategory> DeleteMainCategory(int MainCategoryId)
        {
            var existingMainCategory = await _dbContext.MainCategories.FirstOrDefaultAsync(x => x.Id == MainCategoryId);
            if(existingMainCategory == null)
            {
                return new Response_MainCategory
                {
                    isSuccess = false,
                    Message = "Delete faild"
                }; 
            }
            _dbContext.Remove(existingMainCategory);
            await _dbContext.SaveChangesAsync();
             return new Response_MainCategory
            {
                isSuccess = true,
                MainCategory = new List<MainCategory>()
                {
                    existingMainCategory
                },
            };
        }

        public async Task<Response_MainCategory> UpdateMainCategory(int MainCategoryId, Request_MainCategory Request_MainCategoryDto)
        {
            var existingMainCategory = await _dbContext.MainCategories.FirstOrDefaultAsync(x => x.Id == MainCategoryId);
            if(existingMainCategory == null)
            {
                return new Response_MainCategory
                {
                    isSuccess = false,
                    Message = "Update faild"
                }; 
            }
            existingMainCategory.Name = Request_MainCategoryDto.Name;
            await _dbContext.SaveChangesAsync();
             return new Response_MainCategory
            {
                isSuccess = true,
                MainCategory = new List<MainCategory>()
                {
                    existingMainCategory
                },
            };

        }
    }

}
