using AutoMapper;
using domain = Integrations.Api.Domain.Models;
using contracts = Integrations.Api.Contracts;
using entities = Integrations.Api.Repositories.Models;

namespace Integrations.Api.Mapping
{
    public class SearchQueryEntityMapProfile : Profile
    {
        public SearchQueryEntityMapProfile()
        {
            CreateMap<domain.SearchQueryEntityMap, contracts.SearchQueryEntityMap>()
                                        .ReverseMap();

        }
    }
}