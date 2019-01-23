using AutoMapper;
using Common.Api.Contracts;

namespace Common.Api.Mapping
{
    public class UnitMapperProfile : Profile
    {
        public UnitMapperProfile()
        {
            // Mapping contracts to domain
            CreateMap<Unit, Domain.Entities.Unit>().ReverseMap();
            CreateMap<Shared.Domain.ValueObjects.Queries.UnitWithParent, UnitWithParent>();

            // SearchUnitQuery
            CreateMap<SearchQueryUnit, Domain.Entities.SearchQueryUnit>().ReverseMap();
        }
    }
}