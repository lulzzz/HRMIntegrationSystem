using AutoMapper;
using domain = Timereg.Api.Domain.Models;
using unimicro = Timereg.Api.Unimicro.Models;

namespace Timereg.Api.Mapping.Unimicro
{
    public class HourBalanceUnimicroProfile : Profile
    {
        public HourBalanceUnimicroProfile()
        {
            CreateMap<domain.HourBalance, unimicro.HourBalance>()
                                        .ReverseMap();
        }
    }
}
