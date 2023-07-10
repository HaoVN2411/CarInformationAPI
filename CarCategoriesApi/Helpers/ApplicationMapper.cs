using AutoMapper;
using CarCategoriesApi.Data;
using CarCategoriesApi.Models;

namespace CarCategoriesApi.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() 
        { 
            CreateMap<CarInfo, CreateCarRequestModel>().ReverseMap();
            CreateMap<BrandInfo, CreateBrandInfoModel>().ReverseMap();
            CreateMap<BrandInfo, BrandResponseModel>().ReverseMap();
            CreateMap<UserModel, UserLogin>().ReverseMap();
        }
    }
}
