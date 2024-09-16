using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using OpenAI.Chat;
using Spectre.Console;

namespace AiCommitMessage;

[ExcludeFromCodeCoverage]
internal class Program
{
    /// <summary>
    /// The entry point of the AiCommitMessage application.
    /// </summary>
    /// <param name="args">An array of command-line arguments passed to the application.</param>
    /// <remarks>
    /// This method checks if any command-line arguments were provided. If no arguments are given, it retrieves the version information
    /// from the assembly and displays it along with usage instructions for the application. The usage message indicates how to
    /// run the application with a commit message. If arguments are present, it concatenates them into a single message string
    /// and calls the OpenAI method to process the message.
    /// </remarks>
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            var versionString = Assembly
                .GetEntryAssembly()
                ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;

            AnsiConsole.WriteLine($"AiCommitMessage v{versionString}");
            AnsiConsole.WriteLine("-------------");
            AnsiConsole.WriteLine("\nUsage:");
            AnsiConsole.WriteLine("  AiCommitMessage <message>");
            return;
        }

        var message = string.Join(' ', args);
        OpenAI(message);
    }

    /// <summary>
    /// Sends a message to the OpenAI chat model and displays the response.
    /// </summary>
    /// <param name="message">The message to be sent to the OpenAI chat model.</param>
    /// <remarks>
    /// This method initializes a ChatClient with the specified model name and an API key retrieved from the environment variables.
    /// It then sends a system message along with the user-provided message to the chat model and retrieves the chat completion response.
    /// Finally, it prints the content of the response to the console using AnsiConsole.
    /// This method does not return any value but outputs the response directly to the console.
    /// </remarks>
    static void OpenAI(string message)
    {
        var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrEmpty(key))
        {
            AnsiConsole.WriteLine("Please set the OPENAI_API_KEY environment variable.");
            return;
        }
        var client = new ChatClient("gpt-4o-mini", key);

        var chatCompletion = client.CompleteChat(
            new SystemChatMessage(Constants.SystemMessage),
            new UserChatMessage(message)
        );

        AnsiConsole.WriteLine(chatCompletion.Value.Content[0].Text);
    }
}
