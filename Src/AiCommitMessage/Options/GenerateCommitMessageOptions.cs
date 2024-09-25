using CommandLine;

namespace AiCommitMessage.Options;

/// <summary>
/// Class GenerateMessageOptions.
/// </summary>
[Verb("generate-message", HelpText = "Generate a commit message based on staged changes.")]
public class GenerateCommitMessageOptions
{
    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>The message.</value>
    [Option('m', "message", Required = true, HelpText = "The current commit message.")]
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the branch.
    /// </summary>
    /// <value>The branch.</value>
    [Option('b', "branch", Required = true, HelpText = "The current branch name.")]
    public string Branch { get; set; }

    /// <summary>
    /// Gets or sets the difference.
    /// </summary>
    /// <value>The difference.</value>
    [Option('d', "diff", Required = true, HelpText = "The staged changes.")]
    public string Diff { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether debug is enabled or not.
    /// </summary>
    /// <value><c>true</c> if debug is enabled; otherwise, <c>false</c>.</value>
    [Option('D', "debug", Required = false, HelpText = "Debug mode.")]
    public bool Debug { get; set; }
}
