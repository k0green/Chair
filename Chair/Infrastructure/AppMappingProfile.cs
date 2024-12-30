using AutoMapper;
using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.Chat;
using Chair.BLL.Dto.Contacts;
using Chair.BLL.Dto.ExecutorPromotion;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.Message;
using Chair.BLL.Dto.Minio;
using Chair.BLL.Dto.Order;
using Chair.BLL.Dto.Review;
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
                .ForMember(dest => dest.ExecutorName, opt => opt.MapFrom(src => src.Executor.Name))
                /*.ForMember(dest => dest.AvailableSlots, opt => opt.MapFrom(src => src.Orders
                    .Where(x => x.StarDate >= DateTime.UtcNow)
                    .Where(x => x.ClientId == null)
                    .Count()))
                .ForMember(dest => dest.AvailableSlots, opt => opt.MapFrom(src => src.Orders
                    .Where(x => x.StarDate >= DateTime.UtcNow)
                    .Where(x => x.ClientId == null)
                    .Count()))*/
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Reviews.Average(y => y.Stars)))
                .ForMember(dest => dest.Place, opt => opt.MapFrom(src => new Place()
                {
                    Address  = src.Address,
                    Position = new Position()
                    {
                        Lng = src.Lng,
                        Lat = src.Lat,
                    }
                })).ReverseMap();
            CreateMap<ExecutorService, ExecutorServiceDto>()
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Images))
                .ReverseMap();
            CreateMap<AddExecutorServiceDto, ExecutorService>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Place.Address))
                .ForMember(dest => dest.Lng, opt => opt.MapFrom(src => src.Place.Position.Lng))
                .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Place.Position.Lat));
            CreateMap<UpdateExecutorServiceDto, ExecutorService>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Place.Address))
                .ForMember(dest => dest.Lng, opt => opt.MapFrom(src => src.Place.Position.Lng))
                .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Place.Position.Lat));
            #endregion

            #region ExecutorPromotion

            CreateMap<ExecutorPromotion, ExecutorPromotionDto>()
                .ForMember(dest => dest.ExecutorName, opt => opt.MapFrom(src => src.Executor.Name))
                .ReverseMap();
            CreateMap<ExecutorPromotion, ExecutorPromotionDto>()
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Images))
                .ReverseMap();
            CreateMap<AddExecutorPromotionDto, ExecutorPromotion>();
            CreateMap<UpdateExecutorPromotionDto, ExecutorPromotion>();
            #endregion
            
            #region ServiceType

            CreateMap<ServiceType, ServiceTypeDto>()
                .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent.Name)).ReverseMap();
            CreateMap<ServiceType, AddServiceTypeDto>().ReverseMap();

            #endregion

            #region ExecutorProfile

            CreateMap<ExecutorProfile, ExecutorProfileDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.AccountName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Url))
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.Image.Id))
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

            #region Review

            CreateMap<Review, AddReviewDto>().ReverseMap();
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.AccountName))
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Images.Select(i => new ShortMinioFileDto()
                {
                    Id = i.Id,
                    Url = i.MinioFile.Url
                }).ToList()))
                .ReverseMap();
            CreateMap<Review, UpdateReviewDto>().ReverseMap();

            #endregion

            #region Order

            CreateMap<Order, AddOrderDto>().ReverseMap();
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.User.AccountName))
                .ForMember(dest => dest.ExecutorProfileName, opt => opt.MapFrom(src => src.ExecutorService.Executor.Name))
                .ForMember(dest => dest.ServiceTypeName, opt => opt.MapFrom(src => src.ExecutorService.ServiceType.Name))
                .ForMember(dest => dest.ExecutorName, opt => opt.MapFrom(src => src.ExecutorService.Executor.User.AccountName))
                .ReverseMap();
            CreateMap<Order, UpdateOrderDto>().ReverseMap();

            #endregion            
            
            #region Chat

            CreateMap<Chat, ChatDto>().ReverseMap();

            #endregion      
            
            #region Message

            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<Message, AddMessageDto>().ReverseMap();

            #endregion
        }
    }
}
