using Blazor.Polyfill.Server;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient(sp => new HttpClient()
{
    DefaultRequestHeaders =
    {
        // Use to call the github API on server side
      {"User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.129 Safari/537.36 Edg/81.0.416.68"}
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
