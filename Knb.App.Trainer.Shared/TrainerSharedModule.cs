using Knb.ServiceContainer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Trainer.Shared
{
    [DependsOn()]
    public class TrainerSharedModule : KnbModuleBase
    {
        public TrainerSharedModule(IConfiguration configuration)
            : base(configuration)
        { }

        public override void ConfigureServices(IServiceCollection services)
        {
            
        }
    }
}
