using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace AntBlazor.Docs
{
    public class AntBlazorDocsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAntBlazor();
            context.Services.AddAntBlazorDocs();
        }
    }
}
