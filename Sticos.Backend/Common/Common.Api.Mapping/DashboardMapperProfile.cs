using AutoMapper;
using Common.Api.Contracts;

namespace Common.Api.Mapping
{
    public class DashboardMapperProfile : Profile
    {
        public DashboardMapperProfile()
        {
            // Mapping contracts to domain
            CreateMap<Dashboard, Domain.Entities.Dashboard>()
                .ForMember(d => d.DateCreated, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<SearchQueryDashboard, Domain.Entities.SearchQueryDashboard>()
                .ForMember(x=>x.UserId, dest => dest.Ignore());

            // Mappping model to domain
            CreateMap<Repositories.Models.Dashboard, Domain.Entities.Dashboard>()
                .ReverseMap()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id ?? 0));
        }
    }
}