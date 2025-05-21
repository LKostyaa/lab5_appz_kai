using AutoMapper;
using ChildrenLeisure.BLL.DTOs;
using ChildrenLeisure.DAL.Entities;

namespace ChildrenLeisure.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Attraction, AttractionDto>().ReverseMap();
            CreateMap<FairyCharacter, FairyCharacterDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
        }
    }
}
