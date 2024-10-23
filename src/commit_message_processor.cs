using System;
using System.Text.RegularExpressions;

public class CommitMessageProcessor
{
    private static readonly string semverPattern = @"\+semver:\s?(breaking|major|feature|minor|fix|patch|none|skip)";

    public static string ProcessCommitMessage(string originalMessage)
    {
        // Detect and extract the semver command
        var semverMatch = Regex.Match(originalMessage, semverPattern, RegexOptions.IgnoreCase);

        // Clean the commit message by removing the semver command
        string cleanedMessage = Regex.Replace(originalMessage, semverPattern, "").Trim();

        // Append the semver command at the end if it exists
        if (semverMatch.Success)
        {
            cleanedMessage += " " + semverMatch.Value;
        }

        return cleanedMessage;
    }

    public static void Main(string[] args)
    {
        // Example usage
        string originalMessage = "+semver: minor Initial commit with some features";
        string processedMessage = ProcessCommitMessage(originalMessage);
        Console.WriteLine("Original Message: " + originalMessage);
        Console.WriteLine("Processed Message: " + processedMessage);
    }
}

// Note: This is a basic implementation. In a real-world scenario, 
// you would integrate this into your commit handling workflow.
// Additionally, you would write unit tests to validate the behavior.
