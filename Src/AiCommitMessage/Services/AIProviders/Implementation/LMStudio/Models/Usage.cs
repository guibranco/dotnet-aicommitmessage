using System.Text.Json.Serialization;

namespace AiCommitMessage.Services.AIProviders.Implementation.LMStudio.Models;

public class Usage
{
    [JsonPropertyName("prompt_tokens")]
    public long PromptTokens { get; set; }

    [JsonPropertyName("completion_tokens")]
    public long CompletionTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public long TotalTokens { get; set; }
}