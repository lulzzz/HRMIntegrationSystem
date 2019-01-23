using AutoMapper;
using Common.Api.Contracts;

namespace Common.Api.Mapping
{
    public class NotificationMapperProfile : Profile
    {
        public NotificationMapperProfile()
        {
            // Mapping contracts to domain
            CreateMap<SearchQueryNotification, Domain.Entities.SearchQueryNotification>();
            CreateMap<Notification, Domain.Entities.Notification>().ReverseMap();
        }
    }
}