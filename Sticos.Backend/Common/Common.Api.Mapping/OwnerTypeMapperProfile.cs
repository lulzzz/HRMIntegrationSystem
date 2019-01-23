using AutoMapper;
using Common.Api.Contracts;

namespace Common.Api.Mapping
{
    public class OwnerTypeMapperProfile : Profile
    {
        public OwnerTypeMapperProfile()
        {
            // Mapping contracts to domain
            CreateMap<OwnerType, Domain.Entities.OwnerType>().ReverseMap();
            CreateMap<SearchQueryOwnerType, Domain.Entities.SearchQueryOwnerType>();

            // Mappping model to domain
            CreateMap<Repositories.Models.OwnerType, Domain.Entities.OwnerType>().ReverseMap()
                .ForMember(dest => dest.Dashboards, opt => opt.Ignore());
        }
    }
}