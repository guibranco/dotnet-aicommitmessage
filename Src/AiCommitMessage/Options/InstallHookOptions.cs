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
}
