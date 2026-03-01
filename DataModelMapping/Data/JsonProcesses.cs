using System.Text.Json;
using System.Text.Json.Serialization;
using FluentResults;

namespace DataModelMapping.Data;

public class JsonProcesses<T> :IJsonProcesses<T>
{
    public async Task<Result<T>> DeserializeModelAsync(string json, CancellationToken cancellationToken = default)
    {
        T? model;
        try
        {
            var option = getJsonSerializerOptions();
            using var stream = new MemoryStream(
                System.Text.Encoding.UTF8.GetBytes(json)
            );
            model =  await JsonSerializer.DeserializeAsync<T>(stream, option, cancellationToken);
            if (model == null)
                return Result.Fail($"Deserialized object is null. Name: {nameof(T)}");
            
            return model;
        }
        catch (JsonException ex)
        {
            return Result.Fail($"Invalid JSON: {ex.Message}");
        }
    }

    public async Task<Result<string>> SerializeModelAsync(T model, CancellationToken cancellationToken = default)
    {
        try
        {
            var option = getJsonSerializerOptions();
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, model, option, cancellationToken);
            stream.Position = 0;
            return await new StreamReader(stream).ReadToEndAsync(cancellationToken);
        }
        catch (JsonException ex)
        {
            return Result.Fail($"JSON could not be created: {ex.Message}");
        }
    }

    private JsonSerializerOptions getJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        ReadCommentHandling = JsonCommentHandling.Disallow,
                        AllowTrailingCommas = false
                    };

        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false));

        return options;

    }
}