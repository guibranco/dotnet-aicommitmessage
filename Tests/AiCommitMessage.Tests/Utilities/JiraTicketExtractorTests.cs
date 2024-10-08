using AiCommitMessage.Utilities;
using Xunit;

namespace AiCommitMessage.Tests.Utilities
{
    public class JiraTicketExtractorTests
    {
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
        [InlineData("no-ticket-branch", "")]
        public void ExtractJiraTicket_ShouldReturnExpectedResult(string branchName, string expectedTicket)
        {
            // Act
            string result = JiraTicketExtractor.ExtractJiraTicket(branchName);

            // Assert
            Assert.Equal(expectedTicket, result);
        }
    }
}