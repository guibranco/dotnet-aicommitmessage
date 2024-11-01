using AiCommitMessage.Options;

namespace AiCommitMessage.Services;

public static class SettingsService
{
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
        Environment.SetEnvironmentVariable(
            "OPENAI_API_KEY",
            setSettingsOptions.Key,
            EnvironmentVariableTarget.User
        );

        if (string.IsNullOrWhiteSpace(setSettingsOptions.Url))
        {
            return;
        }

        Environment.SetEnvironmentVariable(
            "OPEN_API_URL",
            setSettingsOptions.Url,
            EnvironmentVariableTarget.User
        );
    }
}
