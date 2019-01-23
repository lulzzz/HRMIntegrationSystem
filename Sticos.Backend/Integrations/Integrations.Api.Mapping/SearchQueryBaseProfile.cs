using AutoMapper;
using domain = Integrations.Api.Domain.Models;
using contracts = Integrations.Api.Contracts;
using entities = Integrations.Api.Repositories.Models;

namespace Integrations.Api.Mapping
{
    public class SearchQueryBaseProfile : Profile
    {
        public SearchQueryBaseProfile()
        {
            CreateMap<domain.SearchQueryBase, contracts.SearchQueryBase>()
                                        .ReverseMap();

        }
    }
}