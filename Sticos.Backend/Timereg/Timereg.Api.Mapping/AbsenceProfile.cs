using AutoMapper;
using contracts = Timereg.Api.Contracts;
using domain = Timereg.Api.Domain.Models;

namespace Timereg.Api.Mapping
{
    public class AbsenceProfile : Profile
    {
        public AbsenceProfile()
        {
            CreateMap<contracts.Absence, domain.Absence>()
                .ReverseMap();
            CreateMap<contracts.AbsenceEntry, domain.AbsenceEntry>()
                .ReverseMap();
        }
    }
}