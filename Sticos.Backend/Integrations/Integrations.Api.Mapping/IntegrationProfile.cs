using AutoMapper;
using domain = Integrations.Api.Domain.Models;
using contracts = Integrations.Api.Contracts;
using entities = Integrations.Api.Repositories.Models;

namespace Integrations.Api.Mapping
{
    public class IntegrationProfile : Profile
    {
        public IntegrationProfile()
        {
            // Integration domain => contracts
            CreateMap<domain.Integration, contracts.Integration>()
                                        .ReverseMap();

            // Entity map domain => repository entities
            CreateMap<domain.Integration, entities.Integration>()
                                        .ReverseMap();

            CreateMap<domain.SearchQueryIntegration, contracts.SearchQueryIntegration>()
                                                   .ReverseMap();
        }
    }
}