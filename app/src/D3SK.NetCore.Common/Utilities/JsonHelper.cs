using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace D3SK.NetCore.Common.Utilities
{
    public static class JsonHelper
    {
        public static JsonSerializerOptions DefaultSerializeOptions => new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static JsonSerializerOptions DefaultDeserializeOptions => new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreReadOnlyProperties = false
        };

        public static string Serialize(object value)
        {
            return JsonSerializer.Serialize(value, DefaultSerializeOptions);
        }

        public static T Deserialize<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value, DefaultDeserializeOptions);
        }
    }
}
