using AutoMapper;

namespace AdvertisingBoard.Maps
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterUserViewModel, User>();
            CreateMap<LoginUserViewModel, User>();
            CreateMap<CategoryViewModel, Category>();
            CreateMap<AdvertisementViewModel, Advertisement>();
            CreateMap<Advertisement, AdvertisementViewModel>();
        }
    }
}
