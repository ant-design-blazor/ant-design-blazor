using Blazor.Polyfill.Server;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton(sp =>
{
    var httpContext = sp.GetService<IHttpContextAccessor>()?.HttpContext;
    if (httpContext != null)
    {
        var request = httpContext.Request;
        var host = request.Host.ToUriComponent();
        var scheme = request.Scheme;
        var baseAddress = $"{scheme}://{host}";
        return new HttpClient() { BaseAddress = new Uri(baseAddress) };
    }
    else
    {
        return new HttpClient() { BaseAddress = new Uri("http://0.0.0.0:8181") };
    }
});

builder.Services.AddAntDesignDocs();
#if NET5_0_OR_GREATER
builder.Services.AddBlazorPolyfill();
#endif
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// disable UseHttpsRedirection to support open site in gitpod workspace, since there is a problem about https endpoint in Gitpod
// app.UseHttpsRedirection();
#if NET5_0
app.UseBlazorPolyfill();
#endif

#if NET6_0
app.UseBlazorPolyfill();
#endif

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
