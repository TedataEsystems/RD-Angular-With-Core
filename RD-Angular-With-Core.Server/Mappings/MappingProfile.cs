using AutoMapper;
using RD.API.ViewModels;
using RD.Domain.Entities;

namespace RD.API.Mappings 
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<SampleEntity, SampleItemVM>();
            //CreateMap<SampleItemVM, SampleEntity>();
            //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            CreateMap<RDDataVM, RDData>();
            CreateMap<RDData, RDDataVM>();
            CreateMap<LogDataViewModel, LogData>();
            CreateMap<LogData, LogDataViewModel>();
            CreateMap<UsersViewModel, Users>();
            CreateMap<Users, UsersViewModel>();


        }
    }
}
