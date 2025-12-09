using CommandLine;

namespace AiCommitMessage.Options;


/// <summary>
/// Class SetEnvironmentVariableOptions.
/// </summary>
[Verb("set-env", HelpText = "Set the Environment Variable.")]
public class SetEnvironmentVariableOptions
{
    /// <summary>
    /// Gets or sets the variable.
    /// </summary>
    /// <value>The variable.</value>
    [Value(0, Required = true, HelpText = "The environment variable in VAR_NAME=value format.")]
    public string Variable { get; set; }

    /// <summary>
    /// Gets or sets the target.
    /// </summary>
    /// <value>The target.</value>
    [Option('t', "target", Required = false, Default = "User", HelpText = "The environment variable target (User or Machine). Default is User.")]
    public string Target { get; set; }
}
