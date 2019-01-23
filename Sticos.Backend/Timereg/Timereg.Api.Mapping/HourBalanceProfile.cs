using AutoMapper;
using domain = Timereg.Api.Domain.Models;
using contracts = Timereg.Api.Contracts;

namespace Timereg.Api.Mapping
{
    public class HourBalanceProfile : Profile
    {
        public HourBalanceProfile()
        {
            CreateMap<domain.HourBalance, contracts.HourBalance>()
                                        .ReverseMap();
        }
    }
}
