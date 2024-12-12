using CommandLine;

namespace AiCommitMessage.Options;

/// <summary>
/// Represents the options for generating a commit message based on staged changes.
/// </summary>
[Verb("generate-message", HelpText = "Generate a commit message based on staged changes.")]
public class GenerateCommitMessageOptions
{
    /// <summary>
    /// Gets or sets the initial commit message provided by the user.
    /// </summary>
    /// <value>The initial commit message used as input for generation.</value>
    /// <remarks>
    /// This message serves as the base for generating a refined or enhanced commit message.
    /// </remarks>
    [Option('m', "message", Required = true, HelpText = "The current commit message.")]
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the name of the current branch.
    /// </summary>
    /// <value>The name of the branch where changes are staged.</value>
    /// <remarks>
    /// Providing the branch name can help contextualize the commit message for workflows
    /// or tools that rely on branch-specific details.
    /// </remarks>
    [Option('b', "branch", Required = false, HelpText = "The current branch name.")]
    public string Branch { get; set; }

    /// <summary>
    /// Gets or sets the staged changes for the commit.
    /// </summary>
    /// <value>A string representation of the staged changes (e.g., diff output).</value>
    /// <remarks>
    /// Including the diff can provide more detailed context for generating a commit message
    /// tailored to the exact changes being staged.
    /// </remarks>
    [Option('d', "diff", Required = false, HelpText = "The staged changes.")]
    public string Diff { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether debug mode is enabled.
    /// </summary>
    /// <value><c>true</c> if debug mode is enabled; otherwise, <c>false</c>.</value>
    /// <remarks>
    /// Debug mode enables additional logging and diagnostic output to assist in troubleshooting.
    /// </remarks>
    [Option('D', "debug", Required = false, HelpText = "Debug mode.")]
    public bool Debug { get; set; }
}
