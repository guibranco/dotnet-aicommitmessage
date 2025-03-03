namespace AiCommitMessage.Services.AIProviders.Core;

public interface IAiServiceProvider
{
    public string GenerateAnswer(string prompt);
}