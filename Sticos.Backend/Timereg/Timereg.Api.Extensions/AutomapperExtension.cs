using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Timereg.Api.Extensions
{ 
    public static class AutoMapperSetup
    {
        public static readonly MapperConfiguration Config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfiles("Timereg.Api.Mapping");
            cfg.CreateMissingTypeMaps = false;
        });
    }
}
