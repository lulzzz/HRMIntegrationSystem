using AutoMapper;
using Timereg.Api.Contracts;
using domain = Timereg.Api.Domain.Models;
using entities = Timereg.Api.Repositories.Models;

namespace Timereg.Api.Mapping
{
    public class AbsenceExportProfile : Profile
    {
        public AbsenceExportProfile()
        {
            // AbsenceExport domain => repository entities
            CreateMap<domain.AbsenceExport, entities.AbsenceExport>()
                                     .ReverseMap();

            // AbsenceExport contracts => domain models
            CreateMap<domain.AbsenceExport, AbsenceExport>()
                                      .ReverseMap();
        }
    }
}