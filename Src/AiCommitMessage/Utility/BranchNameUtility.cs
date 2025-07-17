using System.Text.RegularExpressions;

namespace AiCommitMessage.Utility;

/// <summary>
/// Class BranchNameUtility.
/// </summary>
public static class BranchNameUtility
{
    /// <summary>
    /// The GitHub issue pattern - matches issue number at start or after type prefix.
    /// </summary>
    private const string GitHubIssuePattern = @"^(?:[a-zA-Z]+/)?(\d+)(?:-|$)";

    /// <summary>
    /// The Jira ticket pattern.
    /// </summary>
    private const string JiraTicketPattern = @"(?i)([A-Z]+)-?(\d+)";

    /// <summary>
    /// Extracts the GitHub issue number from a branch name.
    /// </summary>
    /// <param name="branchName">The branch name to extract the issue number from.</param>
    /// <returns>The issue number as a string, or a message if not found.</returns>
    public static string ExtractIssueNumber(string branchName)
    {
        var match = Regex.Match(
            branchName,
            GitHubIssuePattern,
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
            TimeSpan.FromSeconds(5)
        );

        return match.Success ? match.Groups[1].Value : string.Empty;
    }

    /// <summary>
    /// Extracts the JIRA ticket number from a given branch name.
    /// </summary>
    /// <param name="branchName">The branch name to extract the JIRA ticket from.</param>
    /// <returns>The extracted JIRA ticket number in uppercase, or an empty string if not found.</returns>
    public static string ExtractJiraTicket(string branchName)
    {
        var match = Regex.Match(
            branchName,
            JiraTicketPattern,
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
            TimeSpan.FromSeconds(5)
        );

        if (!match.Success)
        {
            return string.Empty;
        }

        var projectKey = match.Groups[1].Value.ToUpperInvariant();
        var issueNumber = match.Groups[2].Value;
        return $"{projectKey}-{issueNumber}";
    }
}
