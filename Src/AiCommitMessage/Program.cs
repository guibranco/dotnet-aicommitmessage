﻿using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using OpenAI.Chat;
using Spectre.Console;

namespace AiCommitMessage;

[ExcludeFromCodeCoverage]
internal class Program
{
    /// <summary>
    /// The entry point of the application that generates a commit message based on staged Git changes.
    /// </summary>
    /// <param name="args">An array of command-line arguments passed to the application.</param>
    /// <remarks>
    /// This method checks if any command-line arguments are provided. If no arguments are given, it retrieves the version information of the application
    /// from the assembly attributes and displays it along with usage instructions for generating a commit message.
    /// If arguments are present, it concatenates them into a single string and passes it to the OpenAI method for further processing.
    /// This allows users to generate meaningful commit messages based on their staged changes in Git.
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
            AnsiConsole.WriteLine("dotnet-aicommitmessage generate-message $(git diff --staged)");
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
    /// This method retrieves the OpenAI API key from the environment variables. If the API key is not set, it prompts the user to set the
    /// "OPENAI_API_KEY" environment variable and exits the application. Once the key is obtained, it initializes a ChatClient with
    /// the specified model ("gpt-4o-mini") and sends a chat completion request using the provided message. The response from the
    /// chat model is then displayed to the user. This method relies on the AnsiConsole for output and requires proper configuration
    /// of the environment variable for successful execution.
    /// </remarks>
    static void OpenAI(string message)
    {
        var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrEmpty(key))
        {
            AnsiConsole.WriteLine("Please set the OPENAI_API_KEY environment variable.");
            Environment.Exit(1);
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
