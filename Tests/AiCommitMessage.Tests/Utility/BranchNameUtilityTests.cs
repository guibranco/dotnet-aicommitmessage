using NUnit.Framework;
using AiCommitMessage.Utility;

namespace AiCommitMessage.Tests.Utility
{
    [TestFixture]
    public class BranchNameUtilityTests
    {
        [TestCase("feature/123-duplicated-schedule-in-payment-lock", "123")]
        [TestCase("bugfix/456-fix-login-bug", "456")]
        [TestCase("feature/issue123-payment-lock-bug", "123")]
        [TestCase("hotfix/789-fix-urgent-crash", "789")]
        [TestCase("feature/ISSUE987-some-feature", "987")]
        [TestCase("enhancement/ISSUE-654-enhance-performance", "654")]
        [TestCase("release/321", "321")]
        [TestCase("task/ISSUE-1111-complete-task", "1111")]
        [TestCase("chore/no-issue-number", "No issue number found.")]
        public void ExtractIssueNumber_ShouldReturnCorrectIssueNumber(string branchName, string expectedIssueNumber)
        {
            // Act
            var result = BranchNameUtility.ExtractIssueNumber(branchName);

            // Assert
            Assert.AreEqual(expectedIssueNumber, result);
        }
    }
}
