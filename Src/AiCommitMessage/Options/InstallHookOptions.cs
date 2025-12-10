using CommandLine;

namespace AiCommitMessage.Options;

/// <summary>
/// Class InstallHookOptions.
/// </summary>
[Verb("install-hook", HelpText = "Install the GIT hook for generating commit messages.")]
public class InstallHookOptions
{
    /// <summary>
    /// Gets or sets the path.
    /// </summary>
    /// <value>The path.</value>
    [Option('p', "path", Required = false, HelpText = "The GIT hooks directory to override.")]
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the hook file should be overriden.
    /// </summary>
    /// <value><c>true</c> to override; otherwise, <c>false</c>.</value>
    [Option('o', "override", Required = false, HelpText = "Override the existing hook.")]
    public bool Override { get; set; }
}