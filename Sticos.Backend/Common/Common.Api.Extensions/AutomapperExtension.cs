using AutoMapper;

namespace Common.Api.Extensions
{
    public static class AutoMapperSetup
    {
        public static readonly MapperConfiguration Config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfiles("Common.Api.Mapping");
            cfg.AddProfiles("Common.Api.Repositories.Legacy");
            cfg.CreateMissingTypeMaps = false;
        });
    }
}