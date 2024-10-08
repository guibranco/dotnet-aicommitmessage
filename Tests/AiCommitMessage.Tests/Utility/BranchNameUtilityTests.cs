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
    [InlineData("chore/no-issue-number", "No issue number found.")]
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
}
