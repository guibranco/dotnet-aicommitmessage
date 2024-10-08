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
    /// <summary>
    /// Tests the ExtractIssueNumber method to ensure it returns an empty string for a given branch name.
    /// </summary>
    /// <param name="branchName">The branch name from which to extract the issue number.</param>
    /// <remarks>
    /// This test case uses the xUnit testing framework to validate the behavior of the 
    /// <see cref="BranchNameUtility.ExtractIssueNumber"/> method. The test is designed to check 
    /// that when the input branch name does not contain an issue number (in this case, "chore/no-issue-number"), 
    /// the method correctly returns an empty string. This is important for ensuring that the method 
    /// behaves as expected when provided with branch names that do not follow the expected format.
    /// </remarks>
    }
    /// <summary>
    /// Tests the extraction of issue numbers from branch names.
    /// </summary>
    /// <param name="branchName">The name of the branch from which to extract the issue number.</param>
    /// <param name="expectedIssueNumber">The expected issue number that should be extracted from the branch name.</param>
    /// <remarks>
    /// This method uses the <see cref="BranchNameUtility.ExtractIssueNumber"/> function to retrieve the issue number from the provided branch name.
    /// It asserts that the result is not empty and matches the expected issue number.
    /// The test cases cover various formats of branch names, including features, bug fixes, hotfixes, enhancements, releases, and tasks.
    /// This ensures that the extraction logic is robust and can handle different naming conventions.
    /// </remarks>
}
