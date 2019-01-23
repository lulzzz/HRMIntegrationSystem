using AutoMapper;
using Common.Api.Contracts;

namespace Common.Api.Mapping
{
    public class AbsenceTypeMapperProfile : Profile
    {
        public AbsenceTypeMapperProfile()
        {
            // SearchUnitQuery
            CreateMap<SearchQueryAbsenceType, Domain.Entities.SearchQueryAbsenceType>().ReverseMap();
        }
    }
}