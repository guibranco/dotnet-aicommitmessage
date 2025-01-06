using AiCommitMessage.Options;
using AiCommitMessage.Utility;
using Spectre.Console;

namespace AiCommitMessage.Services;

public static class SettingsService
{
    
    // write method to set the settings depending on OS
    public static void SetVariableForUsersDependingOnOs(string key, string value)
    {
        var os = Environment.OSVersion.Platform;
        if (os == PlatformID.Win32S || os == PlatformID.Win32Windows || os == PlatformID.Win32NT || os == PlatformID.WinCE)
        {
            Environment.SetEnvironmentVariable(
                key,
                value,
                EnvironmentVariableTarget.User
            );
            return;
        }
        Environment.SetEnvironmentVariable(
            key,
            value
        );
    }
    
    public static string GetVariableForUsersDependingOnOs(string key)
    {
        var os = Environment.OSVersion.Platform;
        if (os == PlatformID.Win32S || os == PlatformID.Win32Windows || os == PlatformID.Win32NT || os == PlatformID.WinCE)
        {
            var value = Environment.GetEnvironmentVariable(
                key,
                EnvironmentVariableTarget.User
            );
            return value;
        }
       return  Environment.GetEnvironmentVariable(
            key
        );
    }
    
    
    /// <summary>
    /// Sets the environment variables for the OpenAI API key and URL based on the provided settings.
    /// </summary>
    /// <param name="setSettingsOptions">An object containing the settings options, including the API key and URL.</param>
    /// <remarks>
    /// This method configures the environment variables required for connecting to the OpenAI API.
    /// It sets the "OPENAI_API_KEY" variable to the value specified in <paramref name="setSettingsOptions.Key"/>.
    /// If the URL provided in <paramref name="setSettingsOptions.Url"/> is null or whitespace, the method will not set the "OPEN_API_URL" variable.
    /// Otherwise, it sets the "OPEN_API_URL" variable to the specified URL.
    /// These environment variables are set at the user level, making them accessible to applications running under the user's context.
    /// </remarks>
    public static void SetSettings(SetSettingsOptions setSettingsOptions)
    {
        if(!string.IsNullOrEmpty(setSettingsOptions.Provider))
        {
            // AI_PROVIDER
            SetVariableForUsersDependingOnOs(
                "AI_PROVIDER",
                setSettingsOptions.Provider.ToString()
            );
            
            // AI_PROVIDER_URL
            SetVariableForUsersDependingOnOs(
                "AI_PROVIDER_URL",
                setSettingsOptions.Url
            );
            
            // AI_MODEL
            SetVariableForUsersDependingOnOs(
                "AI_MODEL",
                setSettingsOptions.Model
            );
            
            return;
        }
        if (!string.IsNullOrWhiteSpace(setSettingsOptions.Model))
        {
            Environment.SetEnvironmentVariable(
                "AI_MODEL",
                setSettingsOptions.Model
            );
        }

        var model = EnvironmentLoader.LoadModelName();

        if (model.Equals("gpt-4o-mini", StringComparison.OrdinalIgnoreCase))
        {
            EnvironmentLoader.SetEnvironmentVariableIfProvided(
                "OPENAI_API_KEY",
                setSettingsOptions.Key,
                EnvironmentLoader.LoadOpenAiApiKey()
            );

            EnvironmentLoader.SetEnvironmentVariableIfProvided(
                "OPENAI_API_URL",
                setSettingsOptions.Url,
                EnvironmentLoader.LoadOpenAiApiUrl()
            );
        }
        else if (model.Equals("llama-3-1-405B-Instruct", StringComparison.OrdinalIgnoreCase))
        {
            EnvironmentLoader.SetEnvironmentVariableIfProvided(
                "LLAMA_API_KEY",
                setSettingsOptions.Key,
                EnvironmentLoader.LoadLlamaApiKey()
            );

            EnvironmentLoader.SetEnvironmentVariableIfProvided(
                "LLAMA_API_URL",
                setSettingsOptions.Url,
                EnvironmentLoader.LoadLlamaApiUrl()
            );
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]{model} is not supported.[/]");
            return;
        }

        AnsiConsole.MarkupLine($"[green]Successfully switched to {model}[/]");
    }
}
