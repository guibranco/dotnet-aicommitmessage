using System.Text.Json.Serialization;

namespace AiCommitMessage.Services.AIProviders.Implementation.LMStudio.Models;

public class Choice
{
    [JsonPropertyName("index")]
    public long Index { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("logprobs")]
    public object Logprobs { get; set; }

    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; }
}