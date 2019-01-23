using AutoMapper;
using Common.Api.Contracts.Users;

namespace Common.Api.Mapping
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            // Mapping contracts to domain
            CreateMap<SearchQueryUser, Domain.Entities.SearchQueryUser>();
        }
    }
}