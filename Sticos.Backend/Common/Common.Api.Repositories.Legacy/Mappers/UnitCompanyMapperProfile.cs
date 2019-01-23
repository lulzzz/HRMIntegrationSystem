using AutoMapper;
using Common.Api.Repositories.Legacy.Models;

namespace Common.Api.Repositories.Legacy.Mappers
{
    public class UnitLegacyMapperProfile : Profile
    {
        public UnitLegacyMapperProfile()
        {
            CreateMap<Unit, Domain.Entities.Unit>();
        }
    }
}