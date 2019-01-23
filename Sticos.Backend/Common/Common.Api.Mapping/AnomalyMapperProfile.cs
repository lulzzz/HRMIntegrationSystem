using AutoMapper;
using Common.Api.Contracts;

namespace Common.Api.Mapping
{
    public class AnomalyMapperProfile : Profile
    {
        public AnomalyMapperProfile()
        {
            // Mapping contracts to domain
            CreateMap<Anomaly, Domain.Entities.Anomaly>().ReverseMap();
            CreateMap<SearchQueryAnomaly, Domain.Entities.SearchQueryAnomaly>();
        }
    }
}