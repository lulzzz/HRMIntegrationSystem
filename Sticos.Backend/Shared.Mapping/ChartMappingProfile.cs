using AutoMapper;
using contract = Shared.Contracts.Charts;
using domain = Shared.Domain.Interfaces;
namespace Shared.Mapping
{
    public class ChartMappingProfile : Profile
    {
        public ChartMappingProfile()
        {
            CreateMap<contract.IChart, domain.IChart>().ReverseMap();
            CreateMap<contract.IChartSerie, domain.IChartSerie>().ReverseMap();
            CreateMap<contract.IChartValue, domain.IChartValue>().ReverseMap();
        }
    }
}