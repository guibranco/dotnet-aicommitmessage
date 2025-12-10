using System.Text.RegularExpressions;

namespace AiCommitMessage.Utility;

/// <summary>
/// Class GitVersionUtility.
/// </summary>
public static class GitVersionUtility
{
    /// <summary>
    /// The semver pattern.
    /// </summary>
    private const string SemverPattern =
        @"\+semver:\s?(breaking|major|feature|minor|fix|patch|none|skip)";

    /// <summary>
    /// Extracts the git version bump command.
    /// </summary>
    /// <param name="originalMessage">The original message.</param>
    /// <returns>System.String.</returns>
    public static string ExtractGitVersionBumpCommand(string originalMessage)
    {
        var match = Regex.Match(
            originalMessage,
            SemverPattern,
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
            TimeSpan.FromSeconds(5)
        );

        return match.Success ? match.Groups[0].Value : string.Empty;
    }
}