using AutoMapper;
using Common.Api.Repositories.Repositories;
using BrregEntity = Common.Api.Contracts.BrregEntity;

namespace Common.Api.Mapping
{
    public class BrregEntityMapperProfile : Profile
    {
        public BrregEntityMapperProfile()
        {
            CreateMap<BrregOrganization, Domain.Entities.BrregEntity>()
                .ForMember(x => x.OrganizationNumber, dest => dest.MapFrom(source => source.OrganisasjonsNummer))
                .ForMember(x => x.Name, dest => dest.MapFrom(source => source.Navn))
                .ForMember(x => x.Type, dest => dest.UseValue(Domain.Entities.BrregEntityType.Child))
                .ForMember(x => x.Children, dest => dest.Ignore());
                
            CreateMap<Domain.Entities.BrregEntity, BrregEntity>();
        }
    }
}