using AiCommitMessage.Utility;
using FluentAssertions;

namespace AiCommitMessage.Tests.Utility;

public class BranchNameUtilityTests
{
    [Theory]
    [InlineData("feature/123-duplicated-schedule-in-payment-lock", "123")]
    [InlineData("bugfix/456-fix-login-bug", "456")]
    [InlineData("feature/issue123-payment-lock-bug", "123")]
    [InlineData("hotfix/789-fix-urgent-crash", "789")]
    [InlineData("feature/ISSUE987-some-feature", "987")]
    [InlineData("enhancement/ISSUE-654-enhance-performance", "654")]
    [InlineData("release/321", "321")]
    [InlineData("task/ISSUE-1111-complete-task", "1111")]
    public void ExtractIssueNumber_ShouldReturnCorrectIssueNumber(
        string branchName,
        string expectedIssueNumber
    )
    {
        // Act
        var result = BranchNameUtility.ExtractIssueNumber(branchName);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Be(expectedIssueNumber);
    }

    [Theory]
    /// <summary>
    /// Tests the ExtractJiraTicket method to ensure it correctly extracts the Jira ticket ID from various branch name formats.
    /// </summary>
    /// <param name="branchName">The branch name from which the Jira ticket ID will be extracted.</param>
    /// <param name="expectedTicket">The expected Jira ticket ID that should be returned by the ExtractJiraTicket method.</param>
    /// <remarks>
    /// This unit test uses the xUnit framework to validate the functionality of the ExtractJiraTicket method in the BranchNameUtility class.
    /// It checks multiple branch name formats, including those with prefixes like "feature/", "bugfix/", and "hotfix/", as well as variations in casing and formatting.
    /// The test asserts that the result is not empty and matches the expected ticket ID, ensuring that the method behaves as intended across different scenarios.
    /// </remarks>
    [InlineData("chore/no-issue-number")]
    public void ExtractIssueNumber_ShouldReturnEmptyString(string branchName)
    {
        // Act
        var result = BranchNameUtility.ExtractIssueNumber(branchName);

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("feature/XPTO-1234-some-branch-name_with_description", "XPTO-1234")]
    [InlineData("feature/XPTO1234-some-branch-name_with_description", "XPTO-1234")]
    [InlineData("XPTO1234-some-branch-name_with_description", "XPTO-1234")]
    [InlineData("XPTO-1234-some-branch-name_with_description", "XPTO-1234")]
    [InlineData("bugfix/XPTO-1234--some-branch-name_with_description", "XPTO-1234")]
    [InlineData("hotfix/xpto-1234-some-branch-name", "XPTO-1234")]
    [InlineData("release/XPTO1234", "XPTO-1234")]
    [InlineData("XPTO-1234", "XPTO-1234")]
    [InlineData("xpto1234", "XPTO-1234")]
    public void ExtractJiraTicket_ShouldReturnExpectedResult(
        string branchName,
        string expectedTicket
    )
    {
        // Act
        var result = BranchNameUtility.ExtractJiraTicket(branchName);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Be(expectedTicket);
    /// <summary>
    /// Tests the ExtractJiraTicket method to ensure it returns an empty string for branch names that do not contain a Jira ticket.
    /// </summary>
    /// <param name="branchName">The branch name to be tested, which is expected to not contain a Jira ticket.</param>
    /// <remarks>
    /// This unit test uses the xUnit framework to verify the behavior of the ExtractJiraTicket method from the BranchNameUtility class.
    /// The test is designed to check that when a branch name does not include a Jira ticket identifier, the method correctly returns an empty string.
    /// The InlineData attribute is used to provide the test method with specific input values for testing.
    /// In this case, the input "chore/no-ticket-branch" is used, which is expected to yield an empty result.
    /// </remarks>
    }

    [Theory]
    [InlineData("chore/no-ticket-branch")]
    public void ExtractJiraTicket_ShouldReturnEmptyString(string branchName)
    {
        // Act
        var result = BranchNameUtility.ExtractJiraTicket(branchName);

        // Assert
        result.Should().BeEmpty();
    }
}
