using AntDesign.ProLayout;
using AntDesign.TestApp.Maui.Services;
using AntDesign.TestApp.Maui.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AntDesign.TestApp.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
    {
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});
        builder.Services.AddScoped<JsonFileReader>();
        //set up the appsetting json file as embeded resource
        using var stream = FileSystem.OpenAppPackageFileAsync("wwwroot/appsettings.json").Result;
        var config = new ConfigurationBuilder()
        .AddJsonStream(stream)
        .Build();
        builder.Configuration.AddConfiguration(config);
        builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
        builder.Services.AddAntDesign();

        //must specify the host base address as the mauibuilder do not have hostenvironment extension
        builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri("http://127.0.0.1") });
        builder.Services.Configure<ProSettings>(builder.Configuration.GetSection("ProSettings"));
        builder.Services.AddScoped<IChartService, ChartService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IProfileService, ProfileService>();

        return builder.Build();
	}
}
