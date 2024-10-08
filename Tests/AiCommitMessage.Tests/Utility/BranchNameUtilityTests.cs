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
