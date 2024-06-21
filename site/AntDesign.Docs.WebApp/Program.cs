// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.Docs.WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AntDesign.Docs.Wasm._Imports).Assembly, typeof(AntDesign.Docs.App).Assembly);

app.Run();
