using Knb.ServiceContainer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.DataStorage.Shared
{
    [DependsOn]
    public class DataStorageSharedModule : KnbModuleBase
    {
        public DataStorageSharedModule(IConfiguration configuration)
            : base(configuration)
        { }

        public override void ConfigureServices(IServiceCollection services)
        {
            // Data storage shared
        }
    }
}
