using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Interfaces.Models
{
    public interface IConfigurableStartUp : IStartup
    {
            Action<IServiceCollection> PostConfigureServiceCollection { get; set; }
    }
}