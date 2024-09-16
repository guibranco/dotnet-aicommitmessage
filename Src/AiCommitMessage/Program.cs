using System.Reflection;
using OpenAI.Chat;
using Spectre.Console;

namespace AiCommitMessage;

internal class Program
{
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

    static void OpenAI(string message)
    {
        var client = new ChatClient(
            "gpt-4o-mini",
            Environment.GetEnvironmentVariable("OPENAI_API_KEY")
        );

        var chatCompletion = client.CompleteChat(
            new SystemChatMessage(Constants.SystemMessage),
            new UserChatMessage(message)
        );

        AnsiConsole.WriteLine(chatCompletion.Value.Content[0].Text);
    }
}
