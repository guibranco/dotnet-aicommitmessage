using System.Diagnostics.CodeAnalysis;
using AiCommitMessage.Options;
using AiCommitMessage.Services;
using AiCommitMessage.Utility;
using CommandLine;

namespace AiCommitMessage;

/// <summary>
/// Class Program.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class Program
{
    /// <summary>
    /// The entry point of the application that processes command-line arguments.
    /// </summary>
    /// <param name="args">An array of strings representing the command-line arguments passed to the application.</param>
    /// <remarks>
    /// This method utilizes the Parser class to parse command-line arguments into specific options types:
    /// <see cref="InstallHookOptions"/>, <see cref="GenerateCommitMessageOptions"/>, <see cref="SetSettingsOptions"/>, and
    /// <see cref="SetEnvironmentVariableOptions"/>.
    /// It calls the <c>Run</c> method if the parsing is successful, allowing the application to execute the intended functionality.
    /// If the parsing fails, it invokes the <c>HandleErrors</c> method to manage any errors that occurred during parsing.
    /// This structure allows for a clean and organized way to handle different command-line options and their corresponding actions.
    /// </remarks>
    private static void Main(string[] args) =>
        Parser
            .Default.ParseArguments<
                InstallHookOptions,
                GenerateCommitMessageOptions,
                SetSettingsOptions,
                SetEnvironmentVariableOptions
            >(args)
            .WithParsed(Run)
            .WithNotParsed(HandleErrors);

    /// <summary>
    /// Executes the appropriate action based on the provided options object.
    /// </summary>
    /// <param name="options">An object representing the command-line options to be processed.</param>
    /// <remarks>
    /// This method uses a switch statement to determine the type of action to perform based on the
    /// runtime type of the <paramref name="options"/> parameter. It handles three specific cases:
    ///
    /// 1. If the options are of type <see cref="InstallHookOptions"/>, it calls the
    ///    <see cref="InstallHookService.InstallHook"/> method to install a hook based on the provided options.
    ///
    /// 2. If the options are of type <see cref="GenerateCommitMessageOptions"/>, it generates a commit message
    ///    using the <see cref="GenerateCommitMessageService.GenerateCommitMessage"/> method and outputs the
    ///    generated message to the console.
    ///
    /// 3. If the options are of type <see cref="SetSettingsOptions"/>, it updates settings by invoking the
    ///    <see cref="SettingsService.SetSettings"/> method with the provided options.
    ///
    /// 4. If the options are of type <see cref="SetEnvironmentVariableOptions"/>, it sets an environment variable
    ///    by invoking the <see cref="EnvironmentVariableService.SetEnvironmentVariable"/> method with the provided options.
    ///
    /// If none of these types match, an error message indicating "Invalid command-line arguments." is outputted.
    /// This method is designed to facilitate command-line operations by routing the execution flow to the
    /// appropriate service based on user input.
    /// </remarks>
    private static void Run(object options)
    {
        switch (options)
        {
            case InstallHookOptions installHookOptions:
                InstallHookService.InstallHook(installHookOptions);
                break;
            case GenerateCommitMessageOptions generateMessageOptions:
                var generatedMessage = new GenerateCommitMessageService().GenerateCommitMessage(
                    generateMessageOptions
                );
                Output.InfoLine(generatedMessage);
                break;
            case SetSettingsOptions setSettingsOptions:
                SettingsService.SetSettings(setSettingsOptions);
                break;
            case SetEnvironmentVariableOptions setEnvironmentVariableOptions:
                EnvironmentVariableService.SetEnvironmentVariable(setEnvironmentVariableOptions);
                break;
            default:
                Output.ErrorLine("Invalid command-line arguments.");
                Environment.ExitCode = 1;
                break;
        }
    }

    /// <summary>
    /// Handles errors by outputting an error message to the console.
    /// </summary>
    /// <param name="errors">An enumerable collection of errors that occurred.</param>
    /// <remarks>
    /// This method is designed to be called when invalid command-line arguments are detected.
    /// It takes an enumerable collection of <paramref name="errors"/> which contains the details of the errors.
    /// The method outputs a generic error message indicating that the command-line arguments provided are invalid.
    /// This is useful for informing users about incorrect input and guiding them to provide valid arguments.
    /// </remarks>
    private static void HandleErrors(IEnumerable<Error> errors)
    {
        var listOfErrors = errors.ToList();
        if (listOfErrors.IsHelp() || listOfErrors.IsVersion())
        {
            Environment.ExitCode = 0;
            return;
        }

        Output.ErrorLine("Invalid command-line arguments.");
        Environment.ExitCode = 2;
    }
}
