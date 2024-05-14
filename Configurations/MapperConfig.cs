using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TestApi.DTOs.ProductDtos.Request;
using TestApi.Models.ProductModels;

namespace TestApi.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Request_MainCategory, MainCategory>().ReverseMap();
            CreateMap<Request_ProductMaterial, Material>().ReverseMap();
            CreateMap<Request_ProductColor, ProductColor>().ReverseMap();
        }
    }
}