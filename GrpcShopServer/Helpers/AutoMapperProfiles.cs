using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrpcShopServer.Models;

namespace GrpcShopServer.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CategoryCreate, Category>();
            CreateMap<Category, CategoryInfo>();
            CreateMap<CategoryInfo, Category>();
            CreateMap<ProductCreate, Product>();
            CreateMap<Product, ProductInfo>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
        }
    }
}
