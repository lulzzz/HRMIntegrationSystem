using AutoMapper;
using domain = Integrations.Api.Domain.Models;
using contracts = Integrations.Api.Contracts;
using entities = Integrations.Api.Repositories.Models;

namespace Integrations.Api.Mapping
{
    public class EntityMapProfile : Profile
    {
        public EntityMapProfile()
        {
            // Entity map domain => contracts
            CreateMap<domain.EntityMap, contracts.EntityMap>()
                                      .ReverseMap();

            // Entity map domain => repository entities
            CreateMap<domain.EntityMap, entities.EntityMap>()
                                       .ReverseMap();
        }
    }
}
