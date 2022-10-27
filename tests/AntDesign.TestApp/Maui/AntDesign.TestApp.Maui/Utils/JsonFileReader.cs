// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AntDesign.TestApp.Maui
{
    public class JsonFileReader
    {
        public async Task<T> ReadJsonAsync<T>(string path)
        {
            var jsonPath = "wwwroot/" + path;
            //set up the appsetting json file as embeded resource
            using var stream = await FileSystem.OpenAppPackageFileAsync(jsonPath);
            var str = await new StreamReader(stream).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<T>(str);
            return data;
        }
    }
}
