using System.Text.Json.Serialization;

namespace AiCommitMessage.Services.AIProviders.Implementation;

public class LmCompletationResponseModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("choices")]
    public Choice[] Choices { get; set; }

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; }
}