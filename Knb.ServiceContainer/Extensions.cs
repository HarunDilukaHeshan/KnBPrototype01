using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.ServiceContainer.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddServiceContainers(this IServiceCollection services, IConfiguration configuration)
        {
            var bootstraper = new ModuleBootstraper();
            bootstraper.Bootstrap(services, configuration);
            return services;
        }        
    }
}
