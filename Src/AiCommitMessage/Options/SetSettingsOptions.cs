using CommandLine;

namespace AiCommitMessage.Options;

/// <summary>
/// Class SetSettingsOptions.
/// </summary>
[Verb("set-settings", HelpText = "Set the AI model settings.")]
public class SetSettingsOptions
{
    /// <summary>
    /// Gets or sets the URL.
    /// </summary>
    /// <value>The URL.</value>
    [Option('u', "url", Required = false, HelpText = "The model url.")]
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    /// <value>The key.</value>
    [Option('k', "key", Required = false, HelpText = "The model API key.")]
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the model.
    /// </summary>
    /// <value>The model.</value>
    [Option(
        'm',
        "model",
        Required = false,
        HelpText = "The model name (e.g., GPT-4o, GPT-5.1, Llama-3-1-405B-Instruct)."
    )]
    public string Model { get; set; }

    /// <summary>
    /// Gets or sets the target.
    /// </summary>
    /// <value>The target.</value>
    [Option('t', "target", Required = false, HelpText = "The environment target.")]
    public string Target { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [save encrypted].
    /// </summary>
    /// <value><c>true</c> if [save encrypted]; otherwise, <c>false</c>.</value>
    [Option('e', "encrypted", Required = false, HelpText = "Persist key encrypted or plain-text.")]
    public bool SaveEncrypted { get; set; }
}
