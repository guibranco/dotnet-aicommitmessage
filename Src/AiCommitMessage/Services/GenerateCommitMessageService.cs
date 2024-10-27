using System.ClientModel;
using System.Diagnostics;
using System.Text.Json;
using AiCommitMessage.Options;
using AiCommitMessage.Utility;
using OpenAI;
using OpenAI.Chat;

namespace AiCommitMessage.Services;

/// <summary>
/// Class GenerateCommitMessageService.
/// </summary>
public class GenerateCommitMessageService
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
        // Use the provided branch or retrieve it from GIT if not supplied
        string branch = string.IsNullOrEmpty(options.Branch)
            ? GitHelper.GetBranchName()
            : options.Branch;

        // Use the provided diff or retrieve it from GIT if not supplied
        string diff = string.IsNullOrEmpty(options.Diff) ? GitHelper.GetGitDiff() : options.Diff;
        var model = EnvironmentLoader.LoadOpenAiModel();
        var url = EnvironmentLoader.LoadOpenAiApiUrl();
        var key = EnvironmentLoader.LoadOpenAiApiKey();

        var client = new ChatClient(
            model,
            new ApiKeyCredential(key),
            new OpenAIClientOptions { Endpoint = new Uri(url) }
        );

        // Use the provided message (this will come from the prepare-commit-msg hook)
        string message = options.Message; // No fallback to GIT, as commit message is passed in the hook

        if (string.IsNullOrEmpty(branch) && string.IsNullOrEmpty(diff))
        {
            throw new InvalidOperationException(
                "Unable to generate commit message: Both branch and diff are empty."
            );
        }

        var formattedMessage =
            "Branch: "
            + (string.IsNullOrEmpty(branch) ? "<unknown>" : branch)
            + "\n\n"
            + "Original message: "
            + message
            + "\n\n"
            + "Git Diff: "
            + (string.IsNullOrEmpty(diff) ? "<no changes>" : diff);

        var chatCompletion = client.CompleteChat(
            new SystemChatMessage(Constants.SystemMessage),
            new UserChatMessage(formattedMessage)
        );

        var text = chatCompletion.Value.Content[0].Text;

        if (text.Length >= 7 && text[..7] == "type - ")
        {
            text = text[7..];
        }

        var provider = GetGitProvider();
        if (provider == GitProvider.GitHub)
        {
            var issueNumber = BranchNameUtility.ExtractIssueNumber(options.Branch);
            if (!string.IsNullOrWhiteSpace(issueNumber))
            {
                text = $"#{issueNumber} {text}";
            }
        }
        else
        {
            var jiraTicketNumber = BranchNameUtility.ExtractJiraTicket(options.Branch);
            if (!string.IsNullOrWhiteSpace(jiraTicketNumber))
            {
                text = $"[{jiraTicketNumber}] {text}";
            }
        }

        if (!options.Debug)
        {
            return text;
        }

        var json = JsonSerializer.Serialize(chatCompletion);
        File.WriteAllText("debug.json", json);

        return text;
    }

    /// <summary>
    /// Retrieves the current Git provider based on the remote origin URL.
    /// </summary>
    /// <returns>A <see cref="GitProvider"/> enumeration value representing the detected Git provider.</returns>
    /// <remarks>
    /// This method executes a Git command to fetch the remote origin URL configured for the repository.
    /// It uses the <c>git config --get remote.origin.url</c> command to obtain the URL, which is then analyzed to determine the Git provider.
    /// The method checks for specific substrings in the URL to identify the provider:
    /// - If the URL contains "dev.azure.com", it returns <see cref="GitProvider.AzureDevOps"/>.
    /// - If the URL contains "bitbucket.org", it returns <see cref="GitProvider.Bitbucket"/>.
    /// - If the URL contains "github.com", it returns <see cref="GitProvider.GitHub"/>.
    /// - If the URL contains "gitlab.com", it returns <see cref="GitProvider.GitLab"/>.
    /// If none of these providers are identified, it returns <see cref="GitProvider.Unidentified"/>.
    /// This method is useful for determining the source control environment in which the code is hosted.
    /// </remarks>
    private static GitProvider GetGitProvider()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = "config --get remote.origin.url",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();
        var originUrl = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        if (originUrl.Contains("dev.azure.com"))
        {
            return GitProvider.AzureDevOps;
        }

        if (originUrl.Contains("bitbucket.org"))
        {
            return GitProvider.Bitbucket;
        }

        if (originUrl.Contains("github.com"))
        {
            return GitProvider.GitHub;
        }

        if (originUrl.Contains("gitlab.com"))
        {
            return GitProvider.GitLab;
        }

        return GitProvider.Unidentified;
    }
}
