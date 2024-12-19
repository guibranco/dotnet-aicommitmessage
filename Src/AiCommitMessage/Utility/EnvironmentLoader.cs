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

    /// <summary>
    /// Loads the OpenAI API key from the environment variables.
    /// </summary>
    /// <returns>A string representing the OpenAI API key.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the API key is not set in the environment variables.</exception>
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
    /// Loads the Llama API key from the environment variables.
    /// </summary>
    /// <returns>A string representing the Llama API key.</returns>
    public static string LoadLlamaApiKey() =>
        GetEnvironmentVariable("LLAMA_API_KEY", string.Empty);

    /// <summary>
    /// Loads the Llama API URL from the environment variables.
    /// </summary>
    /// <returns>A string representing the Llama API URL.</returns>
    public static string LoadLlamaApiUrl() =>
        GetEnvironmentVariable("LLAMA_API_URL", string.Empty);

    /// <summary>
    /// Loads the optional emoji setting from the environment variables.
    /// </summary>
    /// <returns><c>true</c> if should include emoji in the commit message, <c>false</c> otherwise.</returns>
    public static bool LoadOptionalEmoji() =>
        bool.Parse(GetEnvironmentVariable("OPENAI_EMOJI", "true"));

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
    /// Retrieves the value of an environment variable, searching first in the user environment and then in the machine environment.
    /// </summary>
    /// <param name="name">The name of the environment variable to retrieve.</param>
    /// <param name="defaultValue">The value to return if the environment variable is not found.</param>
    /// <returns>The value of the specified environment variable, or <paramref name="defaultValue"/> if the variable is not set.</returns>
    /// <remarks>
    /// This method first attempts to get the value of the specified environment variable from the user-level environment variables.
    /// If the variable is not found or its value is null or whitespace, it then checks the machine-level environment variables.
    /// If the variable is still not found, the method returns the provided <paramref name="defaultValue"/>.
    /// This allows for a fallback mechanism when dealing with environment variables, ensuring that a sensible default can be used.
    /// </remarks>
    private static string GetEnvironmentVariable(string name, string defaultValue)
    {
        var value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);

        if (!string.IsNullOrWhiteSpace(value))
            return value;

        value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);

        return !string.IsNullOrWhiteSpace(value) ? value : defaultValue;
    }

    public static void SetEnvironmentVariableIfProvided(
        string variableName,
        string newValue,
        string existingValue
    )
    {
        if (!string.IsNullOrWhiteSpace(newValue))
        {
            Environment.SetEnvironmentVariable(variableName, newValue, EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable(variableName, newValue, EnvironmentVariableTarget.Process);
        }
        else if (!string.IsNullOrWhiteSpace(existingValue))
        {
            Environment.SetEnvironmentVariable(variableName, existingValue, EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable(variableName, existingValue, EnvironmentVariableTarget.Process);
        }
    }
}
