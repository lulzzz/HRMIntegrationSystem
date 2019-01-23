
using AutoMapper;

namespace Integrations.Api.Extensions
{ 
    public static class AutoMapperSetup
    {
        public static readonly MapperConfiguration Config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfiles("Integrations.Api.Mapping");
            cfg.CreateMissingTypeMaps = false;
        });
    }
}
