using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AntBlazor.Docs.Server
{
    [DependsOn(
        typeof(AntBlazorDocsModule),
        typeof(AbpAutofacModule), 
        typeof(AbpAspNetCoreMvcModule)
        )]
    public class AntBlazorDocServerWebModule : AbpModule
    { 
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();
#if DEBUG
            context.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
#endif
            context.Services.AddMvc(options => options.EnableEndpointRouting = false)
               .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            context.Services.AddRazorPages();
            context.Services.AddServerSideBlazor();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseVirtualFiles();
            app.UseRouting();

            app.UseAbpRequestLocalization();
            app.UseMvcWithDefaultRoute();

            app.UseEndpoints(endpoints => {
                endpoints.MapBlazorHub();
                endpoints.MapRazorPages();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
