using System.Text.Json.Serialization;

namespace AiCommitMessage.Services.AIProviders.Implementation.LMStudio.Models;

public class LmStudioModelsResponse
{
    [JsonPropertyName("data")]
    public Datum[] Data { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; }
}