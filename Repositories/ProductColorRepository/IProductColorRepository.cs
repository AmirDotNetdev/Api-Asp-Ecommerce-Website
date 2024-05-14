using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.DTOs.ProductDtos.Request;
using TestApi.DTOs.ProductDtos.Response;

namespace TestApi.Repositories.ProductColorRepository
{
    public interface IProductColorRepository
    {
        Task<Response_ProductColor> GetAllProductColors();
        Task<Response_ProductColor> GetProductColorById(int productColorId);
        Task<Response_ProductColor> AddProductColor(Request_ProductColor productColor);
        Task<Response_ProductColor> UpdateProductColor(int productColorId,
            Request_ProductColor productColor);
        Task<Response_ProductColor> DeleteProductColor(int productColorId);
    }
}