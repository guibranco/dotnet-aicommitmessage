using System.Text;

namespace AiCommitMessage.Utility;

/// <summary>
/// Class EnvironmentLoader.
/// </summary>
public static class EnvironmentLoader
{
    /// <summary>
    /// Loads the OpenAI model configuration from the environment variables.
    /// </summary>
    /// <returns>The name of the OpenAI model as a string. If the environment variable is not set, it defaults to "gpt-4o-mini".</returns>
    /// <remarks>
    /// This method retrieves the value of the environment variable "OPENAI_MODEL".
    /// If the variable is not found, it returns a default value of "gpt-4o-mini".
    /// This allows for flexibility in specifying which model to use without hardcoding it into the application.
    /// It is particularly useful in scenarios where different models may be used in different environments, such as development, testing, or production.
    /// </remarks>
    public static string LoadModelName() => GetEnvironmentVariable("AI_MODEL", "gpt-4o-mini");

    /// <summary>
    /// Loads the OpenAI API URL from the environment variables.
    /// </summary>
    /// <returns>A string representing the OpenAI API URL. If the environment variable is not set, it returns a default URL.</returns>
    /// <remarks>
    /// This method retrieves the OpenAI API URL by checking the environment variable named "OPENAI_API_URL".
    /// If this variable is not defined, it falls back to a default value of "https://api.openai.com/v1".
    /// This allows for flexibility in configuring the API endpoint without hardcoding it into the application,
    /// making it easier to manage different environments (e.g., development, testing, production).
    /// </remarks>
    public static string LoadOpenAiApiUrl() =>
        GetEnvironmentVariable("OPENAI_API_URL", "https://api.openai.com/v1");

    public static string LoadOpenAiApiKey() => LoadEncryptedApiKey("OPENAI_API_KEY");

    public static string LoadLlamaApiKey() => LoadEncryptedApiKey("LLAMA_API_KEY");

    /// <summary>
    /// Loads the Llama API URL from the environment variables.
    /// </summary>
    /// <returns>A string representing the Llama API URL.</returns>
    public static string LoadLlamaApiUrl() => GetEnvironmentVariable("LLAMA_API_URL", string.Empty);

    /// <summary>
    /// Loads an API key from the environment, decrypting it if specified.
    /// </summary>
    /// <param name="keyName">The name of the environment variable containing the API key.</param>
    private static string LoadEncryptedApiKey(string keyName)
    {
        var key = GetEnvironmentVariable(keyName, string.Empty);
        var isEncrypted =
            bool.TryParse(
                GetEnvironmentVariable($"{keyName}_IS_ENCRYPTED", "false"),
                out var parsed
            ) && parsed;

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException($"Please set the {keyName} environment variable.");
        }

        return isEncrypted ? Decrypt(key) : key;
    }

    /// <summary>
    /// Loads the optional emoji setting from the environment variables.
    /// </summary>
    /// <returns><c>true</c> if should include emoji in the commit message, <c>false</c> otherwise.</returns>
    public static bool LoadOptionalEmoji() =>
        bool.Parse(GetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_USE_EMOJI", "true"));

    /// <summary>
    /// Checks if API calls are disabled via environment variable.
    /// </summary>
    /// <returns><c>true</c> if API calls should be disabled, <c>false</c> otherwise.</returns>
    public static bool IsApiDisabled() =>
        bool.Parse(GetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", "false"));

    /// <summary>
    /// Decrypts the specified encrypted text.
    /// </summary>
    /// <param name="encryptedText">The encrypted text to decrypt.</param>
    /// <returns>A string representing the decrypted text.</returns>
    private static string Decrypt(string encryptedText)
    {
        // Placeholder for decryption logic
        // Implement your decryption logic here
        return Encoding.UTF8.GetString(Convert.FromBase64String(encryptedText));
    }

    /// <summary>
    /// Retrieves the value of an environment variable from the process, user, and machine levels in that order.
    /// </summary>
    /// <param name="name">The name of the environment variable to retrieve.</param>
    /// <param name="defaultValue">The default value to return if the environment variable is not found.</param>
    /// <returns>The value of the specified environment variable, or the provided default value if it is not set.</returns>
    private static string GetEnvironmentVariable(string name, string defaultValue)
    {
        var value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);

        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);

        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);

        return !string.IsNullOrWhiteSpace(value) ? value : defaultValue;
    }

    /// <summary>
    /// Sets an environment variable to a new value if provided; otherwise, uses an existing value.
    /// </summary>
    /// <param name="variableName">The name of the environment variable.</param>
    /// <param name="newValue">The new value for the environment variable.</param>
    /// <param name="existingValue">The existing value for the environment variable.</param>
    public static void SetEnvironmentVariableIfProvided(
        string variableName,
        string newValue,
        string existingValue
    )
    {
        if (!string.IsNullOrWhiteSpace(newValue))
        {
            Environment.SetEnvironmentVariable(
                variableName,
                newValue,
                EnvironmentVariableTarget.User
            );
        }
        else if (!string.IsNullOrWhiteSpace(existingValue))
        {
            Environment.SetEnvironmentVariable(
                variableName,
                existingValue,
                EnvironmentVariableTarget.User
            );
        }
    }
}
