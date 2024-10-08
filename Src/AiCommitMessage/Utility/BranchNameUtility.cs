using System.Text.RegularExpressions;

namespace AiCommitMessage.Utility;

public static class BranchNameUtility
{
    private const string Pattern = @"(?:issue)?[-/]?(\d+)";

    /// <summary>
    /// Extracts the GitHub issue number from a branch name.
    /// </summary>
    /// <param name="branchName">The branch name to extract the issue number from.</param>
    /// <returns>The issue number as a string, or a message if not found.</returns>
    public static string ExtractIssueNumber(string branchName)
    {
        var match = Regex.Match(
            branchName,
            Pattern,
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
            TimeSpan.FromSeconds(5)
        );

        return match.Success ? match.Groups[1].Value : string.Empty;
    }
}
