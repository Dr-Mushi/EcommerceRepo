using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingEFRelations.Dtos.ProductDto;
using TestingEFRelations.Models;

namespace TestingEFRelations.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            //source --> destination
            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto,Product>();
            CreateMap<Product, ProductUpdateDto>();
        }
    }
}
