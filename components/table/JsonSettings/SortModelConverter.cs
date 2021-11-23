// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using AntDesign.TableModels;
using Newtonsoft.Json;

namespace AntDesign.JsonSettings
{
    public class SortModelConverter : JsonConverter
    {
        public override bool CanRead => base.CanRead;

        public override bool CanWrite => base.CanWrite;

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ITableSortModel));
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, typeof(SortModel<string>));
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            serializer.Serialize(writer, (SortModel<string>)value, typeof(SortModel<string>));
        }
    }
}
