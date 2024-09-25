using System.Text.Json;
using System.Text.Json.Serialization;
using AiCommitMessage.Options;
using OpenAI;
using OpenAI.Chat;

namespace AiCommitMessage.Services;

/// <summary>
/// Class GenerateCommitMessageService.
/// </summary>
internal class GenerateCommitMessageService
{
    /// <summary>
    /// Generates the commit message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>System.String.</returns>
    public string GenerateCommitMessage(GenerateCommitMessageOptions options)
    {
        var url = Environment.GetEnvironmentVariable(
            "OPEN_API_URL",
            EnvironmentVariableTarget.User
        );

        if (string.IsNullOrEmpty(url))
            url = "https://api.openai.com/v1";

        var key = Environment.GetEnvironmentVariable(
            "OPENAI_API_KEY",
            EnvironmentVariableTarget.User
        );

        if (string.IsNullOrEmpty(key))
            return "Please set the OPENAI_API_KEY environment variable.";

        var client = new ChatClient(
            "gpt-4o-mini",
            key,
            new OpenAIClientOptions { Endpoint = new Uri(url) }
        );

        var message =
            "Branch: "
            + options.Branch
            + "\n\n"
            + "Original message: "
            + options.Message
            + "\n\n"
            + "Git Diff: "
            + options.Diff;

        var chatCompletion = client.CompleteChat(
            new SystemChatMessage(Constants.SystemMessage),
            new UserChatMessage(message)
        );

        if (options.Debug)
        {
            var json = JsonSerializer.Serialize(chatCompletion);
            File.WriteAllText("debug.json", json);
        }

        return chatCompletion.Value.Content[0].Text;
    }
}
