using AutoMapper;
using domain = Altinn.Api.Domain.Entities;
using contracts = Altinn.Api.Contratcs;

namespace Altinn.Api.Mapping
{
    public class SearchQueryExternalSystemProfile : Profile
    {
        public SearchQueryExternalSystemProfile()
        {
            CreateMap<contracts.SearchQueryExternalSystem, domain.SearchQueryExternalSystem>()
                                              .ReverseMap();
        }
    }
}
