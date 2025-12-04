using System.Text;

namespace AiCommitMessage.Utility;

/// <summary>
/// Provides utility methods for loading and managing environment variables used by the AI Commit Message application.
/// </summary>
/// <remarks>
/// This class handles loading configuration values from environment variables across different scopes (Process, User, Machine),
/// with support for encrypted API keys and default fallback values.
/// </remarks>
public static class EnvironmentLoader
{
    /// <summary>
    /// Loads the AI model configuration from the environment variables.
    /// </summary>
    /// <returns>The name of the AI model as a string. If the environment variable is not set, it defaults to "gpt-5.1".</returns>
    /// <remarks>
    /// This method retrieves the value of the environment variable "AI_MODEL".
    /// If the variable is not found, it returns a default value of "gpt-5.1".
    /// 
    /// Supported models:
    /// <list type="bullet">
    /// <item><description>OpenAI: gpt-5.1, gpt-5-mini, gpt-5-nano, gpt-4o-mini</description></item>
    /// <item><description>Azure AI: llama-3-1-405B-Instruct</description></item>
    /// </list>
    /// 
    /// This allows for flexibility in specifying which model to use without hardcoding it into the application.
    /// It is particularly useful in scenarios where different models may be used in different environments, such as development, testing, or production.
    /// </remarks>
    public static string LoadModelName() => GetEnvironmentVariable("AI_MODEL", "gpt-5.1");

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

    /// <summary>
    /// Loads the OpenAI API key from the environment variables.
    /// </summary>
    /// <returns>A string representing the OpenAI API key, decrypted if necessary.</returns>
    /// <remarks>
    /// This method retrieves the OpenAI API key from the "OPENAI_API_KEY" environment variable.
    /// If the corresponding "_IS_ENCRYPTED" flag is set to true, the key will be automatically decrypted.
    /// This is used for authenticating requests to the OpenAI API services.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown when the OPENAI_API_KEY environment variable is not set or is empty.</exception>
    /// <seealso cref="LoadEncryptedApiKey(string)"/>
    public static string LoadOpenAiApiKey() => LoadEncryptedApiKey("OPENAI_API_KEY");

    /// <summary>
    /// Loads the Llama API key from the environment variables.
    /// </summary>
    /// <returns>A string representing the Llama API key, decrypted if necessary.</returns>
    /// <remarks>
    /// This method retrieves the Llama API key from the "LLAMA_API_KEY" environment variable.
    /// If the corresponding "_IS_ENCRYPTED" flag is set to true, the key will be automatically decrypted.
    /// This is used for authenticating requests to Azure AI services using the Llama model.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown when the LLAMA_API_KEY environment variable is not set or is empty.</exception>
    /// <seealso cref="LoadEncryptedApiKey(string)"/>
    public static string LoadLlamaApiKey() => LoadEncryptedApiKey("LLAMA_API_KEY");

    /// <summary>
    /// Loads the Llama API URL from the environment variables.
    /// </summary>
    /// <returns>A string representing the Llama API URL, or an empty string if not set.</returns>
    /// <remarks>
    /// This method retrieves the Azure AI Llama endpoint URL by checking the "LLAMA_API_URL" environment variable.
    /// Unlike other URL loaders, this returns an empty string if the variable is not defined, as the Llama endpoint
    /// may not be configured in all environments.
    /// </remarks>
    public static string LoadLlamaApiUrl() => GetEnvironmentVariable("LLAMA_API_URL", string.Empty);

    /// <summary>
    /// Loads an API key from the environment, decrypting it if specified.
    /// </summary>
    /// <param name="keyName">The name of the environment variable containing the API key.</param>
    /// <returns>The API key as a string, decrypted if the encryption flag is set.</returns>
    /// <remarks>
    /// This method checks for both the API key itself and a corresponding "_IS_ENCRYPTED" environment variable.
    /// If encryption is enabled, the key is automatically decrypted using the <see cref="Decrypt(string)"/> method.
    /// This provides a secure way to store API keys in environment variables.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown when the specified API key environment variable is not set or is empty.</exception>
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
    /// <returns><c>true</c> if emojis should be included in commit messages; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method checks the "DOTNET_AICOMMITMESSAGE_USE_EMOJI" environment variable.
    /// Defaults to <c>true</c> if the variable is not set, enabling emoji usage by default.
    /// Set to "false" to disable emoji prefixes in generated commit messages.
    /// </remarks>
    public static bool LoadOptionalEmoji() =>
        bool.Parse(GetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_USE_EMOJI", "true"));

    /// <summary>
    /// Checks if API calls are disabled via environment variable.
    /// </summary>
    /// <returns><c>true</c> if API calls should be disabled; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method checks the "DOTNET_AICOMMITMESSAGE_DISABLE_API" environment variable.
    /// Defaults to <c>false</c> if the variable is not set, allowing API calls by default.
    /// When set to "true", the application will use fallback commit message generation instead of calling AI APIs.
    /// This is useful for offline development or testing scenarios.
    /// </remarks>
    public static bool IsApiDisabled() =>
        bool.Parse(GetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", "false"));

    /// <summary>
    /// Decrypts the specified encrypted text using Base64 decoding.
    /// </summary>
    /// <param name="encryptedText">The Base64-encoded encrypted text to decrypt.</param>
    /// <returns>A string representing the decrypted text in UTF-8 encoding.</returns>
    /// <remarks>
    /// This is a placeholder implementation that performs simple Base64 decoding.
    /// In production environments, this should be replaced with proper encryption/decryption logic
    /// using secure cryptographic algorithms such as AES or RSA.
    /// </remarks>
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
    /// <param name="defaultValue">The default value to return if the environment variable is not found at any level.</param>
    /// <returns>The value of the specified environment variable, or the provided default value if it is not set at any level.</returns>
    /// <remarks>
    /// This method searches for the environment variable in the following order:
    /// <list type="number">
    /// <item><description>Process level (current process only)</description></item>
    /// <item><description>User level (current user)</description></item>
    /// <item><description>Machine level (all users)</description></item>
    /// </list>
    /// The first non-empty value found is returned. If no value is found at any level, the default value is returned.
    /// This hierarchical search allows for flexible configuration across different scopes.
    /// </remarks>
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
    /// Sets an environment variable to a new value if provided; otherwise, preserves an existing value.
    /// </summary>
    /// <param name="variableName">The name of the environment variable to set.</param>
    /// <param name="newValue">The new value for the environment variable. If null or whitespace, this parameter is ignored.</param>
    /// <param name="existingValue">The existing value to preserve if no new value is provided. If null or whitespace, no action is taken.</param>
    /// <remarks>
    /// This method sets the environment variable at the User level (<see cref="EnvironmentVariableTarget.User"/>).
    /// It provides a convenient way to update environment variables while preserving existing values when no new value is specified.
    /// This is particularly useful in configuration update scenarios where you want to maintain existing settings unless explicitly changed.
    /// </remarks>
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
