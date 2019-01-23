using AutoMapper;

namespace Absence.Api.Extensions
{
    public static class AutoMapperSetup
    {
        public static readonly MapperConfiguration Config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfiles("Shared.Mapping");
            cfg.CreateMissingTypeMaps = false;
        });
    }
}