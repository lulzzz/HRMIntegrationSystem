using AutoMapper;
using domain = Timereg.Api.Domain.Models;
using Timereg.Api.Contracts;

namespace Timereg.Api.Mapping
{
    public class SearchQueryExternalSystemProfile : Profile
    {
        public SearchQueryExternalSystemProfile()
        {
            // AbsenceExport domain => repository entities
            CreateMap<SearchQueryExternalSystem, domain.SearchQueryExternalSystem>()
                                     .ReverseMap();
        }
    }
}