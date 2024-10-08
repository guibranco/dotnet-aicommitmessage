using System.Text.RegularExpressions;

namespace AiCommitMessage.Utility
{
    public static class BranchNameUtility
    {
        /// <summary>
        /// Extracts the GitHub issue number from a branch name.
        /// </summary>
        /// <param name="branchName">The branch name to extract the issue number from.</param>
        /// <returns>The issue number as a string, or a message if not found.</returns>
        public static string ExtractIssueNumber(string branchName)
        {
            // Regular expression to capture GitHub issue number (just digits)
            string pattern = @"(?i)issue?-?(\d+)";
            
            // Extracting the issue number
            Match match = Regex.Match(branchName, pattern);

            if (match.Success)
            {
                // Extract the issue number (Group 1)
                return match.Groups[1].Value;
            }
            return "No issue number found.";
        }
    }
}