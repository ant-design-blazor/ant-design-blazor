using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AntDesign.TableModels.JsonConverters
{
    public class QueryModelJsonConverter : JsonConverterFactory
    {
        private const string TypePropertyName = "$type";

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(QueryModel) || 
                   (typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(QueryModel<>));
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type converterType;
            if (typeToConvert.IsGenericType)
            {
                Type elementType = typeToConvert.GetGenericArguments()[0];
                converterType = typeof(QueryModelConverterInner<>).MakeGenericType(elementType);
            }
            else
            {
                converterType = typeof(QueryModelConverterInner<object>);
            }
            
            return (JsonConverter)Activator.CreateInstance(converterType);
        }

        private class QueryModelConverterInner<T> : JsonConverter<QueryModel>
        {
            private Type ResolveType(string typeFullName)
            {
                try
                {
                    // First try direct type resolution
                    var type = Type.GetType(typeFullName);
                    if (type != null)
                        return type;

                    // If that fails, try parsing the type name and assembly name separately
                    var parts = typeFullName.Split(',');
                    if (parts.Length < 2)
                        return null;

                    var typeName = parts[0].Trim();
                    var assemblyName = parts[1].Trim();

                    // Try to find the type in the current assembly domain
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (assembly.GetName().Name == assemblyName.Split(',')[0])
                        {
                            type = assembly.GetType(typeName);
                            if (type != null)
                                return type;
                        }
                    }

                    return null;
                }
                catch
                {
                    return null;
                }
            }

            public override QueryModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException("Expected StartObject token");
                }

                int pageIndex = 0;
                int pageSize = 0;
                int startIndex = 0;
                List<ITableSortModel> sortModel = new();
                List<ITableFilterModel> filterModel = new();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        break;
                    }

                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException("Expected PropertyName token");
                    }

                    string propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName.ToLower())
                    {
                        case "pageindex":
                            pageIndex = reader.GetInt32();
                            break;
                        case "pagesize":
                            pageSize = reader.GetInt32();
                            break;
                        case "startindex":
                            startIndex = reader.GetInt32();
                            break;
                        case "sortmodel":
                            if (reader.TokenType != JsonTokenType.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for sortModel");
                            }

                            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            {
                                if (reader.TokenType != JsonTokenType.StartObject)
                                {
                                    throw new JsonException("Expected StartObject token for sort model item");
                                }

                                string typeFullName = null;
                                JsonDocument itemDoc = null;

                                try
                                {
                                    using (var jsonDoc = JsonDocument.ParseValue(ref reader))
                                    {
                                        var root = jsonDoc.RootElement;
                                        if (root.TryGetProperty(TypePropertyName, out var typeProperty))
                                        {
                                            typeFullName = typeProperty.GetString();
                                        }
                                        itemDoc = JsonDocument.Parse(root.GetRawText());
                                    }

                                    Type sortModelType;
                                    if (string.IsNullOrEmpty(typeFullName))
                                    {
                                        sortModelType = typeof(SortModel<>).MakeGenericType(typeof(T));
                                    }
                                    else
                                    {
                                        var type = ResolveType(typeFullName);
                                        if (type == null)
                                        {
                                            // If type resolution fails, try to extract the generic parameter type
                                            var genericTypeName = typeFullName.Split('`')[0];
                                            sortModelType = typeof(SortModel<>).MakeGenericType(typeof(T));
                                        }
                                        else
                                        {
                                            sortModelType = type.IsGenericTypeDefinition ? 
                                                type.MakeGenericType(typeof(T)) : type;
                                        }
                                    }

                                    var sortModelInstance = (ITableSortModel)JsonSerializer.Deserialize(itemDoc.RootElement.GetRawText(), sortModelType, options);
                                    sortModel.Add(sortModelInstance);
                                }
                                finally
                                {
                                    itemDoc?.Dispose();
                                }
                            }
                            break;
                        case "filtermodel":
                            if (reader.TokenType != JsonTokenType.StartArray)
                            {
                                throw new JsonException("Expected StartArray token for filterModel");
                            }

                            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            {
                                if (reader.TokenType != JsonTokenType.StartObject)
                                {
                                    throw new JsonException("Expected StartObject token for filter model item");
                                }

                                string typeFullName = null;
                                JsonDocument itemDoc = null;

                                try
                                {
                                    using (var jsonDoc = JsonDocument.ParseValue(ref reader))
                                    {
                                        var root = jsonDoc.RootElement;
                                        if (root.TryGetProperty(TypePropertyName, out var typeProperty))
                                        {
                                            typeFullName = typeProperty.GetString();
                                        }
                                        itemDoc = JsonDocument.Parse(root.GetRawText());
                                    }

                                    Type filterModelType;
                                    if (string.IsNullOrEmpty(typeFullName))
                                    {
                                        filterModelType = typeof(FilterModel<>).MakeGenericType(typeof(T));
                                    }
                                    else
                                    {
                                        var type = ResolveType(typeFullName);
                                        if (type == null)
                                        {
                                            // If type resolution fails, try to extract the generic parameter type
                                            var genericTypeName = typeFullName.Split('`')[0];
                                            filterModelType = typeof(FilterModel<>).MakeGenericType(typeof(T));
                                        }
                                        else
                                        {
                                            filterModelType = type.IsGenericTypeDefinition ? 
                                                type.MakeGenericType(typeof(T)) : type;
                                        }
                                    }

                                    var filterModelInstance = (ITableFilterModel)JsonSerializer.Deserialize(itemDoc.RootElement.GetRawText(), filterModelType, options);
                                    filterModel.Add(filterModelInstance);
                                }
                                finally
                                {
                                    itemDoc?.Dispose();
                                }
                            }
                            break;
                    }
                }

                Type queryModelType = typeof(QueryModel<>).MakeGenericType(typeof(T));
                var instance = (QueryModel)Activator.CreateInstance(queryModelType, pageIndex, pageSize, startIndex, sortModel, filterModel);
                return instance;
            }

            public override void Write(Utf8JsonWriter writer, QueryModel value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                writer.WriteNumber("pageIndex", value.PageIndex);
                writer.WriteNumber("pageSize", value.PageSize);
                writer.WriteNumber("startIndex", value.StartIndex);

                writer.WriteStartArray("sortModel");
                foreach (var sort in value.SortModel)
                {
                    var json = JsonSerializer.Serialize(sort, sort.GetType(), options);
                    using var doc = JsonDocument.Parse(json);
                    writer.WriteStartObject();
                    writer.WriteString(TypePropertyName, sort.GetType().AssemblyQualifiedName);
                    foreach (var property in doc.RootElement.EnumerateObject())
                    {
                        property.WriteTo(writer);
                    }
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();

                writer.WriteStartArray("filterModel");
                foreach (var filter in value.FilterModel)
                {
                    var json = JsonSerializer.Serialize(filter, filter.GetType(), options);
                    using var doc = JsonDocument.Parse(json);
                    writer.WriteStartObject();
                    writer.WriteString(TypePropertyName, filter.GetType().AssemblyQualifiedName);
                    foreach (var property in doc.RootElement.EnumerateObject())
                    {
                        property.WriteTo(writer);
                    }
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();

                writer.WriteEndObject();
            }
        }
    }
} 