using AutoMapper;

namespace AdvertisingBoard.Maps
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDto, User>();
        }
    }
}
