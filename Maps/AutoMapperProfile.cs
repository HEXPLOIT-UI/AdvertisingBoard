using AdvertisingBoard.ModelsDTO;
using AutoMapper;

namespace AdvertisingBoard.Maps
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterUserViewModel, User>();
            CreateMap<LoginUserViewModel, User>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();
            CreateMap<AdvertisementViewModel, Advertisement>();
        }
    }
}
