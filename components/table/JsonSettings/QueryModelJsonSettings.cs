// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AntDesign.TableModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AntDesign.JsonSettings
{
    /// <summary>
    /// Class intended for serialization and deserialization of QueryModel
    /// Useful for pre-storing and loading QueryModel as a JSON object
    /// Predefined QueryModel can be used in ITable.ReloadData(QueryModel queryModel) method
    /// </summary>
    public class QueryModelJsonSettings<TItem>
    {
        private static JsonSerializerSettings SetDefaultSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new SortModelConverter());
            settings.Converters.Add(new FilterModelConverter());
            settings.ContractResolver = new JsonIgnoreResolver(new List<string> { "OnFilter" });
            return settings;
        }

        public static string SerializeObject(QueryModel queryModel)
        {
            return JsonConvert.SerializeObject(queryModel, SetDefaultSettings());
        }

        public static QueryModel<TItem> DeserializeObject(string queryModelJson)
        {
            return JsonConvert.DeserializeObject<QueryModel<TItem>>(queryModelJson, SetDefaultSettings());
        }
    }

}
