using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.DTOs.ProductDtos.Request;
using TestApi.DTOs.ProductDtos.Response;

namespace TestApi.Repositories.MaterialRepository
{
    public interface IMaterialRepository
    {
        Task<Response_ProductMaterial> GetAllProductMaterial();
        Task<Response_ProductMaterial> GetProductMaterialById(int productMaterialId);
        Task<Response_ProductMaterial> AddProductMaterial(Request_ProductMaterial productMaterialDto);
        Task<Response_ProductMaterial> UpdateProductMaterial(int productMaterialId, Request_ProductMaterial Request_ProductMaterialDto);
        Task<Response_ProductMaterial> DeleteProductMaterial(int productMaterialId);

        

    }
}