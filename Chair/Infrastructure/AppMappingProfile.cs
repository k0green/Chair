using AutoMapper;
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
            CreateMap<ExecutorService, UpdateExecutorServiceDto>().ReverseMap();
            CreateMap<ExecutorService, AddExecutorServiceDto>().ReverseMap();
            #endregion


            #region ServiceType

            CreateMap<ServiceType, ServiceTypeDto>().ReverseMap();
            CreateMap<ServiceType, AddServiceTypeDto>().ReverseMap();

            #endregion
        }
    }
}
