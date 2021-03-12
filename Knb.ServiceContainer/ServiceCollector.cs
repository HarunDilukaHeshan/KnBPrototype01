using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Knb.ServiceContainer
{
    internal class ServiceCollector
    {
        protected IServiceCollection Services { get; } = new ServiceCollection();
        public ServiceCollector()
        {
            
        }

        public IServiceCollection Collect(IConfiguration configuration)
        {
            if (Services.Count > 0) throw new InvalidOperationException("Services are already collected");
            
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var matchingType = typeof(KnbModuleBase);
            foreach(var assembly in assemblies)
            {
                var typeArr = assembly.GetTypes().Where(t => t.IsSubclassOf(matchingType)).ToArray();
                if (typeArr.Length > 1) throw new InvalidOperationException();
                if (typeArr.Length == 0) continue;

                var instance = Activator.CreateInstance(typeArr[0], configuration);
                var configureServicesMethod = typeArr[0].GetMethod("ConfigureServices");
                configureServicesMethod.Invoke(instance, new[] { Services });
            }

            return Services;
        }
    }
}
