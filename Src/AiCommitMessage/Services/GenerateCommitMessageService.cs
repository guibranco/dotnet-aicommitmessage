using System.ClientModel;
using System.Text.Json;
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
    /// Generates a commit message based on the provided options and the OpenAI API.
    /// </summary>
    /// <param name="options">An instance of <see cref="GenerateCommitMessageOptions"/> containing the branch name, original message, and git diff.</param>
    /// <returns>A string containing the generated commit message from the OpenAI API.</returns>
    /// <remarks>
    /// This method retrieves the OpenAI API URL and API key from the environment variables. If the URL is not set, it defaults to "https://api.openai.com/v1".
    /// If the API key is not provided, it returns a message prompting the user to set the <c>OPENAI_API_KEY</c> environment variable.
    /// It constructs a message that includes the branch name, original commit message, and git diff, which is then sent to the OpenAI API using a chat client.
    /// If debugging is enabled, it serializes the chat completion response to JSON and writes it to a file named "debug.json".
    /// Finally, it returns the generated commit message from the chat completion response.
    /// </remarks>
    public string GenerateCommitMessage(GenerateCommitMessageOptions options)
    {
        var model = Environment.GetEnvironmentVariable(
            "OPENAI_MODEL",
            EnvironmentVariableTarget.User
        );

        if (string.IsNullOrWhiteSpace(model))
        {
            model = "gpt-4o-mini";
        }

        var url = Environment.GetEnvironmentVariable(
            "OPENAI_API_URL",
            EnvironmentVariableTarget.User
        );

        if (string.IsNullOrEmpty(url))
        {
            url = "https://api.openai.com/v1";
        }

        var key = Environment.GetEnvironmentVariable(
            "OPENAI_API_KEY",
            EnvironmentVariableTarget.User
        );

        if (string.IsNullOrEmpty(key))
        {
            return "Please set the OPENAI_API_KEY environment variable.";
        }

        var client = new ChatClient(
            model,
            new ApiKeyCredential(key),
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

        if (!options.Debug)
        {
            return chatCompletion.Value.Content[0].Text;
        }

        var json = JsonSerializer.Serialize(chatCompletion);
        File.WriteAllText("debug.json", json);

        return chatCompletion.Value.Content[0].Text;
    }
}
