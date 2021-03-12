
using Knb.ServiceContainer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Simulator.Shared
{
    public class SimulatorSharedModule : KnbModuleBase
    {
        public SimulatorSharedModule(IConfiguration configuration)
            : base(configuration)
        { }

        public override void ConfigureServices(IServiceCollection services)
        {
            
        }
    }
}
