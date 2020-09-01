using System;
using Newtonsoft.Json;
using System.Text.Json;

namespace D3SK.NetCore.Common.Utilities
{
    public static class JsonHelper
    {
        public static bool UseNewtonsoftJson = true;

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
            return UseNewtonsoftJson
                ? SerializeNewtonsoft(value)
                : System.Text.Json.JsonSerializer.Serialize(value, DefaultSerializeOptions);
        }

        public static object Deserialize(string value, Type type = null)
        {
            type ??= typeof(object);
            return UseNewtonsoftJson
                ? DeserializeNewtonsoft(value)
                : System.Text.Json.JsonSerializer.Deserialize(value, type, DefaultDeserializeOptions);
        }

        public static T Deserialize<T>(string value)
        {
            return UseNewtonsoftJson
                ? DeserializeNewtonsoft<T>(value)
                : System.Text.Json.JsonSerializer.Deserialize<T>(value, DefaultDeserializeOptions);
        }

        private static string SerializeNewtonsoft(object value)
        {
            return JsonConvert.SerializeObject(value,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new NewtonsoftPrivateSetterContractResolver()
                });
        }

        private static object DeserializeNewtonsoft(string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        private static T DeserializeNewtonsoft<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}