using AiCommitMessage.Options;

namespace AiCommitMessage.Services;

public class SettingsService
{
    public void SetSettings(SetSettingsOptions setSettingsOptions)
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
