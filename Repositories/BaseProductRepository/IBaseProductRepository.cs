using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TestApi.Data.CustomModels;
using TestApi.DTOs.BasePorductDto.Request;
using TestApi.DTOs.BasePorductDto.Response;
using TestApi.Models.ProductModels;

namespace TestApi.Repositories.BaseProductRepository
{
    public interface IBaseProductRepository
    {
        Task<IEnumerable<BaseProduct>> GetAllAsync();
        Task<IEnumerable<Model_BaseProductCustom>> GetAllWithFullInfoAsync();
        Task<Response_BaseProductWithPaging> GetAllWithFullInfoByPages(int pageNumber, int pageSize);
        Task<Response_BaseProduct> GetByIdWithNoInfo(int baseProductId);
        Task<Response_BaseProductWithFullInfo> GetByIdWithFullInfo(int baseProductId);
        Task<Response_BaseProduct> AddBaseProduct(Request_BaseProduct baseProduct);
        Task<Response_BaseProduct> UpdateBaseProduct(int baseProductId, Request_BaseProduct baseProduct);
        Task<Response_BaseProduct> UpdateBaseProductPrice(int baseProductId ,Request_BaseProductPrice baseProductPrice);
        Task<Response_BaseProduct> UpdateBaseProductDiscount(int baseProductId ,Request_BaseProductDiscount baseProductDiscount);
        Task<Response_BaseProduct> UpdateBaseProductMainCategory(int baseProductId ,Request_BaseProductMainCategory baseProductMainCategory);   
        Task<Response_BaseProduct> UpdateBaseProductMaterial(int baseProductId ,Request_BaseProductMaterial baseProductMaterial);   
        Task<Response_BaseProduct> RemoveBaseProduct(int baseProductId);  

        //search

        Task<IEnumerable<string>> GetProductSearchSuggestions(string searchText);
        Task<IEnumerable<Model_BaseProductCustom>> GetProductSearch(string searchText);
        Task<Response_BaseProductWithPaging> GetProductSearchWithPaging(string searchText, int pageNumber, int pageSize); 
        Task<IEnumerable<Model_BaseImageCustom>> SearchProducts(int[] materialsIds, int[] mainCategoryIds, int[] productColorIds, int [] productSizesIds);




    }
}