// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AntDesign;

if (!CultureInfo.CurrentCulture.Name.IsIn("en-US", "zh-CN"))
{
    CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
    CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
}

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAntDesignDocs();
await builder.Build().RunAsync();
