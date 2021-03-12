using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Knb.ServiceContainer
{
    internal class ModuleBootstraper
    {
        protected static readonly IList<Assembly> _assemblies = null;
        protected readonly IList<Type> _types = new List<Type>();
        public void Bootstrap(IServiceCollection services, IConfiguration configuration)
        {
            if (_assemblies != null) throw new InvalidOperationException("Modules are already loaded");

            var asbl = new List<Assembly>();
            var entryAssembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Could not find an entry assembly");
            asbl.Add(entryAssembly);
            var entryModuleType = GetModule(entryAssembly) ?? throw new InvalidOperationException("Entry assembly does not contain a module");
            var instance = Activator.CreateInstance(entryModuleType, new[] { configuration }) as KnbModuleBase; 
            AA(instance, services, configuration);
        }

        protected void AA(KnbModuleBase module, IServiceCollection services, IConfiguration configuration)
        {
            if (module == null) throw new ArgumentNullException();

            var dependsOnAttr = module.GetType().GetCustomAttribute(typeof(DependsOnAttribute)) as DependsOnAttribute;
            
            if (dependsOnAttr != null && dependsOnAttr.Dependencies.Length > 0)
            {
                foreach(var d in dependsOnAttr.Dependencies)
                {
                    if (IsTypeExists(d)) throw new InvalidOperationException("Duplicated modules");                    

                    var instance = Activator.CreateInstance(d, new[] { configuration }) as KnbModuleBase;
                    AA(instance, services, configuration);
                    _types.Add(d);
                }
            }            
            
            module.ConfigureServices(services);            
        }

        protected Type GetModule(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException();

            var types = assembly.GetTypes();
            var moduleTypes = types.Where(t => t.IsSubclassOf(typeof(KnbModuleBase))).ToArray();

            if (moduleTypes.Length > 1) throw new InvalidOperationException("A single project cannot have multiple modules");

            return (moduleTypes.Length > 0) ? moduleTypes[0] : null;
        }

        protected bool IsAssemblyExists(Assembly assembly, Assembly[] assemblies) =>
            assemblies.SingleOrDefault(a => a.GetType().IsEquivalentTo(assembly.GetType())) != null;

        protected bool IsTypeExists(Type type) 
        { 
            var count = _types.Where(t => t.IsEquivalentTo(type)).ToArray().Length;

            if (count > 1) throw new InvalidOperationException("Duplicated modules");

            return (count > 0);
        }
    }
}
