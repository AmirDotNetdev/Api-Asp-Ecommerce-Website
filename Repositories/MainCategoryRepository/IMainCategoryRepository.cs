using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.DTOs.ProductDtos.Request;
using TestApi.DTOs.ProductDtos.Response;

namespace TestApi.Repositories.MainCategoryRepository
{
    public interface IMainCategoryRepository
    {
        Task<Response_MainCategory> GetMainCategory();
        Task<Response_MainCategory> GetMainCategoryById(int MainCategoryId);
        Task<Response_MainCategory> AddMainCategory(Request_MainCategory MainCategoryDto);
        Task<Response_MainCategory> UpdateMainCategory(int MainCategoryId, Request_MainCategory Request_MainCategoryDto);
        Task<Response_MainCategory> DeleteMainCategory(int MainCategoryId);
    }
}