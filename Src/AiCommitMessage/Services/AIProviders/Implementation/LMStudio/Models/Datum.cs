using System.Text.Json.Serialization;

namespace AiCommitMessage.Services.AIProviders.Implementation.LMStudio.Models;

public class Datum
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("owned_by")]
    public string OwnedBy { get; set; }
}