using System.ClientModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using AiCommitMessage.Options;
using AiCommitMessage.Utility;
using Azure;
using Azure.AI.Inference;
using OpenAI;
using OpenAI.Chat;

namespace AiCommitMessage.Services;

/// <summary>
/// Service for generating commit messages based on staged changes and contextual information.
/// </summary>
public class GenerateCommitMessageService
{
    /// <summary>
    /// Regular expression to detect merge conflict resolution messages.
    /// </summary>
    private static readonly Regex MergeConflictPattern = new(
        @"^Merge branch '.*' into .*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase
    );

    /// <summary>
    /// Checks whether the given commit message is a merge conflict resolution message.
    /// </summary>
    /// <param name="message">The commit message to evaluate.</param>
    /// <returns><c>true</c> if the message indicates a merge conflict resolution; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This helper method uses a predefined regular expression to match patterns commonly seen in merge conflict resolutions.
    /// </remarks>
    private static bool IsMergeConflictResolution(string message) =>
        MergeConflictPattern.IsMatch(message);

    /// <summary>
    /// Generates a commit message based on the provided options and the OpenAI API.
    /// </summary>
    /// <param name="options">An instance of <see cref="GenerateCommitMessageOptions"/> containing the branch name, original message, and git diff.</param>
    /// <returns>A string containing the generated commit message from the API.</returns>
    /// <remarks>
    /// This method retrieves API details (URL and key) from environment variables, constructs a message including the branch name,
    /// original commit message, and git diff, and sends it to the respective API for processing. It also handles debugging, saving
    /// API responses to a JSON file if debugging is enabled. If the commit message is a merge conflict resolution, it is returned as-is.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown if both the branch and diff are empty, as meaningful commit generation is not possible.</exception>
    public string GenerateCommitMessage(GenerateCommitMessageOptions options)
    {
        var branch = string.IsNullOrEmpty(options.Branch)
            ? GitHelper.GetBranchName()
            : options.Branch;
        var diff = string.IsNullOrEmpty(options.Diff) ? GitHelper.GetGitDiff() : options.Diff;
        diff = FilterPackageLockDiff(diff);
        var message = options.Message;

        if (IsMergeConflictResolution(message))
        {
            return message;
        }

        if (Encoding.UTF8.GetByteCount(diff) > 10240)
        {
            throw new InvalidOperationException(
                "🚫 The staged changes are too large to process. Please reduce the number of files or size of changes and try again."
            );
        }

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

        var model = EnvironmentLoader.LoadModelName();
        return GenerateWithModel(model, formattedMessage, branch, message, options.Debug);
    }
    private static string FilterPackageLockDiff(string diff)
    {
        if (string.IsNullOrEmpty(diff))
            return diff;

        var ignoredPatterns = new[] {
            "package-lock.json",
            "yarn.lock",
            "pnpm-lock.yaml",
            ".csproj.lock",
            "composer.lock",
            "Gemfile.lock"
        };

        var result = new StringBuilder();
        bool skipBlock = false;
        var lines = diff.Split('\n');
        foreach (var line in lines)
        {
            if (line.StartsWith("diff --git"))
            {
                skipBlock = false;
                var parts = line.Split(' ');
                if (parts.Length >= 4)
                {
                    var pathB = parts[3].StartsWith("b/") ? parts[3].Substring(2) : parts[3];
                    foreach (var pattern in ignoredPatterns)
                    {
                        if (pathB.EndsWith(pattern))
                        {
                            skipBlock = true;
                            break;
                        }
                    }
                }
                if (!skipBlock)
                    result.Append(line + "\n");
            }
            else if (!skipBlock)
            {
                result.Append(line + "\n");
            }
        }
        return result.ToString().TrimEnd('\n');
    }

    private static string GenerateWithModel(
        string model,
        string formattedMessage,
        string branch,
        string message,
        bool debug
    )
    {
        string text;

        if (model.Equals("llama-3-1-405B-Instruct", StringComparison.OrdinalIgnoreCase))
        {
            text = GenerateUsingAzureAi(formattedMessage);
        }
        else if (model.Equals("gpt-4o-mini", StringComparison.OrdinalIgnoreCase))
        {
            text = GenerateUsingOpenAi(formattedMessage);
        }
        else
        {
            throw new NotSupportedException($"Model '{model}' is not supported.");
        }

        text = ProcessGeneratedMessage(text, branch, message);

        if (!debug)
        {
            return text;
        }

        SaveDebugInfo(text);

        return text;
    }

    /// <summary>
    /// Generates a commit message using the Azure AI API.
    /// </summary>
    /// <param name="formattedMessage">The formatted message to be sent to the Azure AI API.</param>
    /// <returns>The generated commit message.</returns>
    private static string GenerateUsingAzureAi(string formattedMessage)
    {
        string text;
        var endpoint = new Uri(EnvironmentLoader.LoadLlamaApiUrl());
        var credential = new AzureKeyCredential(EnvironmentLoader.LoadLlamaApiKey());

        var client = new ChatCompletionsClient(
            endpoint,
            credential,
            new AzureAIInferenceClientOptions()
        );

        var requestOptions = new ChatCompletionsOptions
        {
            Messages =
            {
                new ChatRequestSystemMessage(Constants.SystemMessage),
                new ChatRequestUserMessage(formattedMessage),
            },
            Temperature = 1.0f,
            NucleusSamplingFactor = 1.0f,
            MaxTokens = 1000,
            Model = "Meta-Llama-3.1-405B-Instruct",
        };

        var response = client.Complete(requestOptions);
        text = response.Value.Content;
        return text;
    }

    /// <summary>
    /// Generates a commit message using the OpenAI API.
    /// </summary>
    /// <param name="formattedMessage">The formatted message to be sent to the OpenAI API.</param>
    /// <returns>The generated commit message.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the OpenAI API is unavailable.</exception>
    private static string GenerateUsingOpenAi(string formattedMessage)
    {
        string text;
        try
        {
            var apiUrl = EnvironmentLoader.LoadOpenAiApiUrl();
            var apiKey = EnvironmentLoader.LoadOpenAiApiKey();

            var client = new ChatClient(
                "gpt-4o-mini",
                new ApiKeyCredential(apiKey),
                new OpenAIClientOptions { Endpoint = new Uri(apiUrl) }
            );

            var chatCompletion = client.CompleteChat(
                new SystemChatMessage(Constants.SystemMessage),
                new UserChatMessage(formattedMessage)
            );

            text = chatCompletion.Value.Content[0].Text;
        }
        catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
        {
            throw new InvalidOperationException(
                "⚠️ OpenAI API is currently unavailable. Please try again later."
            );
        }

        return text;
    }

    /// <summary>
    /// Service for generating commit messages using AI models.
    /// </summary>
    /// <remarks>
    /// This service provides functionality to generate commit messages based on provided options and the OpenAI API.
    /// It includes methods to detect merge conflict resolution messages, generate commit messages using different AI models,
    /// and process the generated messages to include additional information such as issue numbers or version bump commands.
    /// </remarks>
    private static string ProcessGeneratedMessage(string text, string branch, string message)
    {
        if (text.Length >= 7 && text[..7] == "type - ")
        {
            text = text[7..];
        }

        var provider = GetGitProvider();
        if (provider == GitProvider.GitHub)
        {
            var issueNumber = BranchNameUtility.ExtractIssueNumber(branch);
            if (!string.IsNullOrWhiteSpace(issueNumber))
            {
                text = $"#{issueNumber} {text}";
            }
        }
        else
        {
            var jiraTicketNumber = BranchNameUtility.ExtractJiraTicket(branch);
            if (!string.IsNullOrWhiteSpace(jiraTicketNumber))
            {
                text = $"[{jiraTicketNumber}] {text}";
            }
        }

        var gitVersionCommand = GitVersionUtility.ExtractGitVersionBumpCommand(message);
        if (!string.IsNullOrEmpty(gitVersionCommand))
        {
            text = $"{text} {gitVersionCommand}";
        }

        return text;
    }

    /// <summary>
    /// Saves the provided debug information to a JSON file named "debug.json".
    /// </summary>
    /// <param name="text">The debug information to be saved.</param>
    private static void SaveDebugInfo(string text)
    {
    }
    private static string FilterPackageLockDiff(string diff)
    {
        if (string.IsNullOrEmpty(diff))
            return diff;

        var ignoredPatterns = new[]
        {
            "package-lock.json",
            "yarn.lock",
            "pnpm-lock.yaml",
            ".csproj.lock",
            "composer.lock",
            "Gemfile.lock"
        };

        var result = new StringBuilder();
        bool skipBlock = false;
        var lines = diff.Split('\n');
        foreach (var line in lines)
        {
            if (line.StartsWith("diff --git"))
            {
                skipBlock = false;
                var parts = line.Split(' ');
                if (parts.Length >= 4)
                {
                    var pathB = parts[3].StartsWith("b/") ? parts[3].Substring(2) : parts[3];
                    if (ignoredPatterns.Any(pattern => pathB.EndsWith(pattern)))
                    {
                        skipBlock = true;
                    }
                }
            }
            if (!skipBlock)
            {
                result.AppendLine(line);

    /// <summary>
    /// Retrieves the current Git provider based on the remote origin URL.
    /// </summary>
    /// <returns>A <see cref="GitProvider"/> enumeration value representing the detected Git provider.</returns>
    /// <remarks>
    /// This method uses the <c>git config --get remote.origin.url</c> command to determine the Git provider based on substrings in the URL.
    /// If no known provider is identified, it defaults to <see cref="GitProvider.Unidentified"/>.
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
        using var process = new Process { StartInfo = processStartInfo };
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
