using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Common.Utilities
{
    public class JsonObjectConverter : JsonConverter<object>
    {
        public override object Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt64(out var longValue))
                {
                    return longValue;
                }
                else
                {
                    if (reader.TryGetDecimal(out var decimalValue))
                    {
                        return decimalValue;
                    }
                }
            }
            else if (reader.TokenType == JsonTokenType.True)
            {
                return true;
            }
            else if (reader.TokenType == JsonTokenType.False)
            {
                return false;
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                if (reader.TryGetDateTimeOffset(out var dateValue))
                {
                    return dateValue;
                }

                return reader.GetString();
            }

            throw new JsonException($"Invalid type {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            if (value is string sVal)
            {
                writer.WriteStringValue(sVal);
            }
            else if (value is decimal dVal)
            {
                writer.WriteNumberValue(dVal);
            }
            else if (value is int iVal)
            {
                writer.WriteNumberValue(iVal);
            }
            else if (value is bool bVal)
            {
                writer.WriteBooleanValue(bVal);
            }
            else if (value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                throw new InvalidOperationException("Directly writing object not supported.");
            }
        }
    }
}
