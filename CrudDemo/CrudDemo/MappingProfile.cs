using AutoMapper;
using CrudDemo.DTO;
using CrudDemo.Models;

namespace BGVC.Airline.Backend.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GlossaryItem, GlossaryItemDto>();
            CreateMap<GlossaryItemDto, GlossaryItem>();
            CreateMap<GlossaryItemAddDto, GlossaryItem>();
        }       
    }
}
    