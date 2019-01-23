using domain = Timereg.Api.Domain.Models;
using contracts = Timereg.Api.Contracts;
using AutoMapper;

namespace Timereg.Api.Mapping
{
    public class ExternalSystemProfile : Profile
    {
        public ExternalSystemProfile()
        {
            CreateMap<domain.ExternalSystem, contracts.ExternalSystem>()
                                    .ReverseMap();
        }
       
    }
}
