using System.Diagnostics.CodeAnalysis;
using AiCommitMessage.Options;
using AiCommitMessage.Services;
using CommandLine;

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
        Parser
            .Default.ParseArguments<InstallHookOptions, GenerateMessageOptions, SetSettingsOptions>(
                args
            )
            .WithParsed(Run)
            .WithNotParsed(HandleErrors);
        ;
    }

    private static void Run(object options)
    {
        switch (options)
        {
            case InstallHookOptions installHookOptions:
                new InstallHookService().InstallHook();
                break;
            case GenerateMessageOptions generateMessageOptions:
                var generatedMessage = new GenerateCommitMessageService().GenerateCommitMessage(
                    generateMessageOptions.Diff
                );
                Output.InfoLine(generatedMessage);
                break;
            case SetSettingsOptions setSettingsOptions:
                new SettingsService().SetSettings(setSettingsOptions);
                break;
            default:
                Output.ErrorLine("Invalid command-line arguments.");
        }
    }

    private static void HandleErrors(IEnumerable<Error> obj)
    {
        Output.ErrorLine("Invalid command-line arguments.");
    }
}
