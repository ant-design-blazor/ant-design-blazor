// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.Json;

namespace AntDesign.Core.Helpers
{
    public static class JsonElementHelper<TValue>
    {
        private static readonly Type _columnDataType;

        static JsonElementHelper()
        {
            _columnDataType = THelper.GetUnderlyingType<TValue>();
        }

        public static object GetValue(JsonElement jsonElement)
        {
            if (_columnDataType.IsEnum)
            {
                if (jsonElement.ValueKind == JsonValueKind.Number)
                    return Enum.Parse(THelper.GetUnderlyingType<TValue>(), jsonElement.GetInt32().ToString());
                else
                    return Enum.Parse(THelper.GetUnderlyingType<TValue>(), jsonElement.GetString());
            }
            if (_columnDataType == typeof(DateTime))
                return jsonElement.GetDateTime();
            else if (_columnDataType == typeof(byte))
                return jsonElement.GetByte();
            else if (_columnDataType == typeof(decimal))
                return jsonElement.GetDecimal();
            else if (_columnDataType == typeof(double))
                return jsonElement.GetDouble();
            else if (_columnDataType == typeof(short))
                return jsonElement.GetInt16();
            else if (_columnDataType == typeof(int))
                return jsonElement.GetInt32();
            else if (_columnDataType == typeof(long))
                return jsonElement.GetInt64();
            else if (_columnDataType == typeof(sbyte))
                return jsonElement.GetSByte();
            else if (_columnDataType == typeof(float))
                return jsonElement.GetSingle();
            else if (_columnDataType == typeof(ushort))
                return jsonElement.GetUInt16();
            else if (_columnDataType == typeof(uint))
                return jsonElement.GetUInt32();
            else if (_columnDataType == typeof(ulong))
                return jsonElement.GetUInt64();
            else if (_columnDataType == typeof(Guid))
                return jsonElement.GetGuid();
            else if (_columnDataType == typeof(bool))
                return jsonElement.ValueKind == JsonValueKind.True;
            else
                return jsonElement.GetString();
        }
    }
}
