using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.ServiceContainer
{
    public abstract class KnbModuleBase
    {
        protected IConfiguration Configuration { get; }        

        public KnbModuleBase(IConfiguration config)     
        {
            Configuration = config;
        }

        public abstract void ConfigureServices(IServiceCollection services);
    }
}
