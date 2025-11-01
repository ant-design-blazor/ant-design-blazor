// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace AntDesign.TableModels.JsonConverters
{
    /// <summary>
    /// Converter for ITableSortModel that handles polymorphic serialization with type preservation.
    /// Uses reflection to reconstruct generic types, which is not trim-safe.
    /// </summary>
    public class TableSortModelConverter : JsonConverter<ITableSortModel>
    {
        private const string TypePropertyName = "$type";

#if NET5_0_OR_GREATER
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "This converter is designed for scenarios where dynamic type reconstruction is required")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "This converter is designed for scenarios where dynamic type reconstruction is required")]
#endif
        public override ITableSortModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            // Try to get the type discriminator
            string fieldTypeName = null;
            if (root.TryGetProperty(TypePropertyName, out var fieldTypeElement))
            {
                fieldTypeName = fieldTypeElement.GetString();
            }

            // Read the basic properties
            var columnIndex = root.GetProperty("ColumnIndex").GetInt32();
            var priority = root.GetProperty("Priority").GetInt32();
            var fieldName = root.GetProperty("FieldName").GetString();
            var sortDirection = Enum.Parse<SortDirection>(root.GetProperty("SortDirection").GetString());

            // Reconstruct with the original generic type if available
            if (!string.IsNullOrEmpty(fieldTypeName))
            {
                var fieldType = Type.GetType(fieldTypeName);
                if (fieldType != null)
                {
                    var sortModelType = typeof(SortModel<>).MakeGenericType(fieldType);
                    return (ITableSortModel)Activator.CreateInstance(sortModelType, columnIndex, priority, fieldName, sortDirection);
                }
            }

            // Fallback to object type
            return new SortModel<object>(columnIndex, priority, fieldName, sortDirection);
        }

#if NET5_0_OR_GREATER
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "This converter is designed for scenarios where dynamic type reconstruction is required")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "This converter is designed for scenarios where dynamic type reconstruction is required")]
#endif
        public override void Write(Utf8JsonWriter writer, ITableSortModel value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Write type discriminator to preserve generic type information
            var valueType = value.GetType();
            if (valueType.IsGenericType)
            {
                var fieldType = valueType.GetGenericArguments()[0];
                writer.WriteString(TypePropertyName, fieldType.AssemblyQualifiedName);
            }

            // Write all properties
            writer.WriteNumber("ColumnIndex", value.ColumnIndex);
            writer.WriteNumber("Priority", value.Priority);
            writer.WriteString("FieldName", value.FieldName);
            writer.WriteString("SortDirection", value.SortDirection.ToString());

            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// Converter for ITableFilterModel that handles polymorphic serialization with type preservation.
    /// Uses reflection to reconstruct generic types, which is not trim-safe.
    /// </summary>
    public class TableFilterModelConverter : JsonConverter<ITableFilterModel>
    {
        private const string TypePropertyName = "$type";
#if NET5_0_OR_GREATER
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "This converter is designed for scenarios where dynamic type reconstruction is required")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "This converter is designed for scenarios where dynamic type reconstruction is required")]
#endif
        public override ITableFilterModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            // Try to get the type discriminator
            string fieldTypeName = null;
            if (root.TryGetProperty(TypePropertyName, out var fieldTypeElement))
            {
                fieldTypeName = fieldTypeElement.GetString();
            }

            // Read the basic properties
            var columnIndex = root.GetProperty("ColumnIndex").GetInt32();
            var fieldName = root.GetProperty("FieldName").GetString();

            // Read optional array properties
            string[] selectedValues = null;
            if (root.TryGetProperty("SelectedValues", out var selectedValuesElement) && selectedValuesElement.ValueKind == JsonValueKind.Array)
            {
                var valuesList = new List<string>();
                foreach (var item in selectedValuesElement.EnumerateArray())
                {
                    valuesList.Add(item.GetString());
                }
                selectedValues = valuesList.ToArray();
            }

            TableFilter[] filters = null;
            if (root.TryGetProperty("Filters", out var filtersElement) && filtersElement.ValueKind == JsonValueKind.Array)
            {
                var filtersList = new List<TableFilter>();
                foreach (var filterItem in filtersElement.EnumerateArray())
                {
                    var text = filterItem.GetProperty("Text").GetString();
                    var value = filterItem.GetProperty("Value").GetString();
                    var selected = filterItem.TryGetProperty("Selected", out var selectedProp) && selectedProp.GetBoolean();
                    filtersList.Add(new TableFilter { Text = text, Value = value, Selected = selected });
                }
                filters = filtersList.ToArray();
            }

            var filterType = TableFilterType.List;
            if (root.TryGetProperty("FilterType", out var filterTypeElement))
            {
                filterType = Enum.Parse<TableFilterType>(filterTypeElement.GetString());
            }

            // Reconstruct with the original generic type if available
            if (!string.IsNullOrEmpty(fieldTypeName))
            {
                var fieldType = Type.GetType(fieldTypeName);
                if (fieldType != null)
                {
                    var filterModelType = typeof(FilterModel<>).MakeGenericType(fieldType);
                    return (ITableFilterModel)Activator.CreateInstance(filterModelType, columnIndex, fieldName, selectedValues, filters, filterType);
                }
            }

            // Fallback to object type
            return new FilterModel<object>(columnIndex, fieldName, selectedValues, filters, filterType);
        }

#if NET5_0_OR_GREATER
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "This converter is designed for scenarios where dynamic type reconstruction is required")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "This converter is designed for scenarios where dynamic type reconstruction is required")]
#endif
        public override void Write(Utf8JsonWriter writer, ITableFilterModel value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Write type discriminator to preserve generic type information
            var valueType = value.GetType();
            if (valueType.IsGenericType)
            {
                var fieldType = valueType.GetGenericArguments()[0];
                writer.WriteString(TypePropertyName, fieldType.AssemblyQualifiedName);
            }

            // Write all properties
            writer.WriteNumber("ColumnIndex", value.ColumnIndex);
            writer.WriteString("FieldName", value.FieldName);

            if (value.SelectedValues != null)
            {
                writer.WritePropertyName("SelectedValues");
                writer.WriteStartArray();
                foreach (var item in value.SelectedValues)
                {
                    writer.WriteStringValue(item);
                }
                writer.WriteEndArray();
            }

            if (value.Filters != null)
            {
                writer.WritePropertyName("Filters");
                writer.WriteStartArray();
                foreach (var filter in value.Filters)
                {
                    writer.WriteStartObject();
                    writer.WriteString("Text", filter.Text);
                    if (filter.Value != null)
                    {
                        writer.WriteString("Value", filter.Value.ToString());
                    }
                    writer.WriteBoolean("Selected", filter.Selected);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }

            // Write FilterType if it's a FilterModel<T> (has this property)
            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(FilterModel<>))
            {
                var filterTypeProp = valueType.GetProperty("FilterType");
                if (filterTypeProp != null)
                {
                    var filterTypeValue = filterTypeProp.GetValue(value);
                    writer.WriteString("FilterType", filterTypeValue?.ToString() ?? "List");
                }
            }

            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// Converter for List of ITableSortModel that handles polymorphic serialization.
    /// Uses reflection to reconstruct generic types, which is not trim-safe.
    /// </summary>
    public class TableSortModelListConverter : JsonConverter<IList<ITableSortModel>>
    {
#if NET5_0_OR_GREATER
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "This converter uses TableSortModelConverter which performs dynamic type reconstruction")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "This converter uses TableSortModelConverter which performs dynamic type reconstruction")]
#endif
        public override IList<ITableSortModel> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected StartArray token");
            }

            var list = new List<ITableSortModel>();
            var elementConverter = new TableSortModelConverter();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                var item = elementConverter.Read(ref reader, typeof(ITableSortModel), options);
                list.Add(item);
            }

            return list;
        }

#if NET5_0_OR_GREATER
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "This converter uses TableSortModelConverter which performs dynamic type reconstruction")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "This converter uses TableSortModelConverter which performs dynamic type reconstruction")]
#endif
        public override void Write(Utf8JsonWriter writer, IList<ITableSortModel> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var elementConverter = new TableSortModelConverter();

            foreach (var item in value)
            {
                elementConverter.Write(writer, item, options);
            }

            writer.WriteEndArray();
        }
    }

    /// <summary>
    /// Converter for IList of ITableFilterModel that handles polymorphic serialization.
    /// Uses reflection to reconstruct generic types, which is not trim-safe.
    /// </summary>
    public class TableFilterModelListConverter : JsonConverter<IList<ITableFilterModel>>
    {
#if NET5_0_OR_GREATER
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "This converter uses TableFilterModelConverter which performs dynamic type reconstruction")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "This converter uses TableFilterModelConverter which performs dynamic type reconstruction")]
#endif
        public override IList<ITableFilterModel> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected StartArray token");
            }

            var list = new List<ITableFilterModel>();
            var elementConverter = new TableFilterModelConverter();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                var item = elementConverter.Read(ref reader, typeof(ITableFilterModel), options);
                list.Add(item);
            }

            return list;
        }

#if NET5_0_OR_GREATER
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "This converter uses TableFilterModelConverter which performs dynamic type reconstruction")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "This converter uses TableFilterModelConverter which performs dynamic type reconstruction")]
#endif
        public override void Write(Utf8JsonWriter writer, IList<ITableFilterModel> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var elementConverter = new TableFilterModelConverter();

            foreach (var item in value)
            {
                elementConverter.Write(writer, item, options);
            }

            writer.WriteEndArray();
        }
    }
}
