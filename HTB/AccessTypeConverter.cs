using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using HTB;

public class AccessTypeConverter : JsonConverter<IAccessType>
{
    public override IAccessType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (var jsonDoc = JsonDocument.ParseValue(ref reader))
        {
            var jsonObject = jsonDoc.RootElement;

            if (jsonObject.TryGetProperty("Type", out var typeProp))
            {
                var type = typeProp.GetString();
                return type switch
                {
                    "Free" => JsonSerializer.Deserialize<Free>(jsonObject.GetRawText(), options),
                    "Paid" => JsonSerializer.Deserialize<Paid>(jsonObject.GetRawText(), options),
                    _ => throw new NotSupportedException($"Unsupported AccessType '{type}'")
                };
            }
            throw new JsonException("Invalid AccessType JSON.");
        }
    }

    public override void Write(Utf8JsonWriter writer, IAccessType value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}