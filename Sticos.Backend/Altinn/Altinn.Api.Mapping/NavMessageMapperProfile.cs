using AutoMapper;

using contracts = Altinn.Api.Contratcs;
using domain = Altinn.Api.Domain.Entities;
using dbModels = Altinn.Api.Repositories.Models;

namespace Altinn.Api.Mapping
{
    public class NavMessageMapperProfile : Profile
    {
        public NavMessageMapperProfile()
        {
            CreateMap<domain.NavMessage, dbModels.NavMessage>()
                .ReverseMap();
            CreateMap<contracts.SearchQueryNavMessage, domain.SearchQueryNavMessage>();
        }
    }
}
