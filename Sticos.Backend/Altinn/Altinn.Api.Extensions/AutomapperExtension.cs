using AutoMapper;

namespace Altinn.Api.Extensions
{
    public static class AutoMapperSetup
    {
        public static readonly MapperConfiguration Config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfiles("Altinn.Api.Mapping");
            cfg.CreateMissingTypeMaps = false;
        });
    }
}
