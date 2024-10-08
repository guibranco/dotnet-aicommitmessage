using System.Text;

namespace AiCommitMessage.Utility;

/// <summary>
/// Class EnvironmentLoader.
/// </summary>
public static class EnvironmentLoader
{
    /// <summary>
    /// Loads the OpenAI model.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string LoadOpenAiModel() => GetEnvironmentVariable("OPENAI_MODEL", "gpt-4o-mini");

    /// <summary>
    /// Loads the OpenAI API URL.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string LoadOpenAiApiUrl() =>
        GetEnvironmentVariable("OPENAI_API_URL", "https://api.openai.com/v1");

    /// <summary>
    /// Loads the OpenAI API key.
    /// </summary>
    /// <returns>System.String.</returns>
    /// <exception cref="InvalidOperationException">Please set the OPENAI_API_KEY environment variable.</exception>
    public static string LoadOpenAiApiKey()
    {
        var encryptStr = GetEnvironmentVariable("OPENAI_KEY_ENCRYPTED", "false");
        var encrypt = bool.Parse(encryptStr);

        var key = GetEnvironmentVariable("OPENAI_API_KEY", string.Empty);

        if (key == string.Empty)
        {
            throw new InvalidOperationException(
                "Please set the OPENAI_API_KEY environment variable."
            );
        }

        return encrypt ? Decrypt(key) : key;
    }

    /// <summary>
    /// Loads the optional emoji.
    /// </summary>
    /// <returns><c>true</c> if should include emoji in the commit message, <c>false</c> otherwise.</returns>
    public static bool LoadOptionalEmoji() =>
        bool.Parse(GetEnvironmentVariable("OPENAI_EMOJI", "true"));

    /// <summary>
    /// Decrypts the specified encrypted text.
    /// </summary>
    /// <param name="encryptedText">The encrypted text.</param>
    /// <returns>System.String.</returns>
    private static string Decrypt(string encryptedText)
    {
        // Placeholder for decryption logic
        // Implement your decryption logic here
        return Encoding.UTF8.GetString(Convert.FromBase64String(encryptedText));
    }

    /// <summary>
    /// Gets the environment variable.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>System.String.</returns>
    private static string GetEnvironmentVariable(string name, string defaultValue)
    {
        var value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);

        if (!string.IsNullOrWhiteSpace(value))
            return value;

        value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);

        return !string.IsNullOrWhiteSpace(value) ? value : defaultValue;
    }
}
