using System.Text.RegularExpressions;

namespace AiCommitMessage.Utilities
{
    public static class JiraTicketExtractor
    {
        /// <summary>
        /// Extracts the JIRA ticket number from a given branch name.
        /// </summary>
        /// <param name="branchName">The branch name to extract the JIRA ticket from.</param>
        /// <returns>The extracted JIRA ticket number in uppercase, or an empty string if not found.</returns>
        public static string ExtractJiraTicket(string branchName)
        {
            // Regular expression to match the JIRA ticket pattern (with or without a hyphen)
            string pattern = @"(?i)([A-Z]+)-?(\d+)";

            // Extracting the JIRA ticket number
            Match match = Regex.Match(branchName, pattern);

            if (!match.Success)
            {
                return string.Empty;
            }

            // Construct the JIRA ticket number, ensuring the hyphen is present
            string projectKey = match.Groups[1].Value.ToUpper(); // Extract the project key in uppercase
            string issueNumber = match.Groups[2].Value; // Extract the issue number
            // Combine the project key and issue number with a hyphen
            string jiraTicket = $"{projectKey}-{issueNumber}";
            return jiraTicket;
        }
    }
}