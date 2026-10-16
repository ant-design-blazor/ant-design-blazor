// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;

namespace AntDesign
{
    public static class AdvancedFilterStateSerializer
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public static string Serialize(FilterConditionNode rootNode)
        {
            return JsonSerializer.Serialize(ToModel(rootNode), _jsonSerializerOptions);
        }

        public static FilterConditionNode Deserialize<TItem>(string json, IReadOnlyList<FilterFieldDescriptor> fields = null)
        {
            return Deserialize(json, fields ?? FilterFieldResolver.Resolve<TItem>());
        }

        public static FilterConditionNode Deserialize(string json, IReadOnlyList<FilterFieldDescriptor> fields)
        {
            if (string.IsNullOrWhiteSpace(json))
                return FilterConditionNode.CreateGroup();

            var model = JsonSerializer.Deserialize<AdvancedFilterConditionModel>(json, _jsonSerializerOptions);
            var fieldMap = fields?.ToDictionary(field => field.PropertyName) ?? new Dictionary<string, FilterFieldDescriptor>();
            return FromModel(model, fieldMap) ?? FilterConditionNode.CreateGroup();
        }

        public static FilterConditionNode Clone(FilterConditionNode rootNode, IReadOnlyList<FilterFieldDescriptor> fields)
        {
            return Deserialize(Serialize(rootNode), fields);
        }

        public static Expression<Func<TItem, bool>> BuildExpression<TItem>(FilterConditionNode rootNode, IReadOnlyList<FilterFieldDescriptor> fields)
        {
            var fieldMap = fields?.ToDictionary(field => field.PropertyName) ?? new Dictionary<string, FilterFieldDescriptor>();
            return AdvancedFilterExpressionBuilder.Build<TItem>(rootNode, fieldMap);
        }

        private static AdvancedFilterConditionModel ToModel(FilterConditionNode node)
        {
            if (node == null)
                return null;

            return new AdvancedFilterConditionModel
            {
                NodeType = node.NodeType,
                PropertyName = node.PropertyName,
                CompareOperator = node.CompareOperator,
                Value = ToJsonElement(node.Value),
                LogicalOperator = node.LogicalOperator,
                Children = node.Children?.Select(ToModel).Where(child => child != null).ToList() ?? new(),
            };
        }

        private static JsonElement? ToJsonElement(object value)
        {
            if (value == null)
                return null;

            return JsonSerializer.SerializeToElement(value, value.GetType(), _jsonSerializerOptions);
        }

        private static FilterConditionNode FromModel(AdvancedFilterConditionModel model, IReadOnlyDictionary<string, FilterFieldDescriptor> fields)
        {
            if (model == null)
                return null;

            return new FilterConditionNode
            {
                NodeType = model.NodeType,
                PropertyName = model.PropertyName,
                CompareOperator = model.CompareOperator,
                Value = ConvertValue(model.Value, model, fields),
                LogicalOperator = model.LogicalOperator,
                Children = model.Children?.Select(child => FromModel(child, fields)).Where(child => child != null).ToList() ?? new(),
            };
        }

        private static object ConvertValue(JsonElement? value, AdvancedFilterConditionModel model, IReadOnlyDictionary<string, FilterFieldDescriptor> fields)
        {
            if (!value.HasValue || value.Value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
                return null;

            if (string.IsNullOrEmpty(model.PropertyName) || !fields.TryGetValue(model.PropertyName, out var field))
                return ReadJsonValue(value.Value);

            if (model.CompareOperator == TableFilterCompareOperator.Between && value.Value.ValueKind == JsonValueKind.Array)
                return ConvertRangeArray(value.Value, field.PropertyType);

            if (value.Value.ValueKind == JsonValueKind.Array)
                return value.Value.EnumerateArray().Select(item => ConvertElement(item, field.PropertyType)).ToArray();

            return ConvertElement(value.Value, field.PropertyType);
        }

        private static Array ConvertArray(JsonElement jsonElement, Type targetType)
        {
            var elements = jsonElement.EnumerateArray().ToArray();
            var array = Array.CreateInstance(targetType, elements.Length);
            for (var i = 0; i < elements.Length; i++)
            {
                array.SetValue(ConvertElement(elements[i], targetType), i);
            }

            return array;
        }

        private static Array ConvertRangeArray(JsonElement jsonElement, Type targetType)
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            var rangeItemType = underlyingType.IsDateType() && Nullable.GetUnderlyingType(targetType) == null
                ? typeof(Nullable<>).MakeGenericType(underlyingType)
                : targetType;

            return ConvertArray(jsonElement, rangeItemType);
        }

        private static object ConvertElement(JsonElement jsonElement, Type targetType)
        {
            if (jsonElement.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
                return null;

            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (underlyingType == typeof(string))
                return jsonElement.GetString();

            if (underlyingType == typeof(bool))
                return jsonElement.GetBoolean();

            if (underlyingType == typeof(Guid))
                return jsonElement.ValueKind == JsonValueKind.String ? Guid.Parse(jsonElement.GetString()) : jsonElement.GetGuid();

            if (underlyingType == typeof(DateTime))
                return jsonElement.ValueKind == JsonValueKind.String ? DateTime.Parse(jsonElement.GetString(), CultureInfo.InvariantCulture) : jsonElement.GetDateTime();

#if NET6_0_OR_GREATER
            if (underlyingType == typeof(DateOnly))
                return DateOnly.Parse(jsonElement.GetString(), CultureInfo.InvariantCulture);

            if (underlyingType == typeof(TimeOnly))
                return TimeOnly.Parse(jsonElement.GetString(), CultureInfo.InvariantCulture);
#endif

            if (underlyingType.IsEnum)
                return jsonElement.ValueKind == JsonValueKind.String
                    ? Enum.Parse(underlyingType, jsonElement.GetString())
                    : Enum.ToObject(underlyingType, jsonElement.GetInt32());

            return JsonSerializer.Deserialize(jsonElement.GetRawText(), underlyingType, _jsonSerializerOptions);
        }

        private static object ReadJsonValue(JsonElement jsonElement)
        {
            return jsonElement.ValueKind switch
            {
                JsonValueKind.String => jsonElement.GetString(),
                JsonValueKind.Number => jsonElement.TryGetInt64(out var longValue) ? longValue : jsonElement.GetDecimal(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Array => jsonElement.EnumerateArray().Select(ReadJsonValue).ToArray(),
                _ => jsonElement.Clone(),
            };
        }
    }
}
