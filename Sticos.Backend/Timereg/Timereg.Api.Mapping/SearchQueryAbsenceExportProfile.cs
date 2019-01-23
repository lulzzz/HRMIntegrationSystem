using AutoMapper;
using Timereg.Api.Contracts;
using domain = Timereg.Api.Domain.Models;

namespace Timereg.Api.Mapping
{
    public class SearchQueryAbsenceExportProfile : Profile
    {
        public SearchQueryAbsenceExportProfile()
        {
            // AbsenceExport domain => repository entities
            CreateMap<domain.SearchQueryAbsenceExport, SearchQueryAbsenceExport>()
                                     .ReverseMap();
        }
    }
}