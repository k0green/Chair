using AutoMapper;
using Chair.BLL.Dto.Contacts;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.ServiceType;
using Chair.DAL.Data.Entities;

namespace Chair.Infrastructure
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            #region ExecutorService

            CreateMap<ExecutorService, ExecutorServiceDto>()
                .ForMember(dest => dest.ServiceTypeName, opt => opt.MapFrom(src => src.ServiceType.Name))
                .ReverseMap();
            CreateMap<ExecutorService, ExecutorServiceDto>()
                .ForMember(dest => dest.ImageURLs, opt => opt.MapFrom(src => src.Images.Select(x=>x.URL)))
                .ReverseMap();
            CreateMap<ExecutorService, UpdateExecutorServiceDto>().ReverseMap();
            CreateMap<ExecutorService, AddExecutorServiceDto>().ReverseMap();
            #endregion


            #region ServiceType

            CreateMap<ServiceType, ServiceTypeDto>().ReverseMap();
            CreateMap<ServiceType, AddServiceTypeDto>().ReverseMap();

            #endregion

            #region ExecutorProfile

            CreateMap<ExecutorProfile, ExecutorProfileDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.AccountName))
                .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.Contacts))
                .ReverseMap();
            CreateMap<ExecutorProfile, UpdateExecutorProfileDto>().ReverseMap();
            CreateMap<ExecutorProfile, AddExecutorProfileDto>().ReverseMap();
            CreateMap<ExecutorProfile, ExecutorProfileDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.AccountName))
                .ReverseMap();
            #endregion

            #region Contact

            CreateMap<Contact, AddContactsDto>().ReverseMap();
            CreateMap<Contact, ContactsDto>().ReverseMap();
            CreateMap<Contact, UpdateContactsDto>().ReverseMap();
            #endregion
        }
    }
}
