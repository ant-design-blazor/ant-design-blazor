using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;

namespace AntDesign.Docs.Desktop.WinForms
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddBlazorWebView();
            ConfigureServices(serviceCollection);
            var services = serviceCollection.BuildServiceProvider();

            BlazorWebView blazorWebView = new();
            blazorWebView.Dock = DockStyle.Fill;
            blazorWebView.HostPage = @"wwwroot\index.html";
            blazorWebView.Services = services;
            blazorWebView.RootComponents.Add<App>("app");

            Controls.Add(blazorWebView);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<HttpHandler>();
            services.AddHttpClient("Default", client =>
            {
                client.DefaultRequestHeaders.Add(
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.129 Safari/537.36 Edg/81.0.416.68");
            }).AddHttpMessageHandler<HttpHandler>();
            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Default"));

            services.AddAntDesignDocs();
        }
    }
}
