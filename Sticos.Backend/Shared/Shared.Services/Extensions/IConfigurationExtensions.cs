using Microsoft.Extensions.Configuration;
using System;

namespace Shared.Services.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetValueNotNull<T>(this IConfiguration configuration, string key)
        {
            var value = configuration.GetValue<T>(key);
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                throw new ArgumentException($"Configuration with key={key} is missing");
            }

            return value;
        }
    }
}
