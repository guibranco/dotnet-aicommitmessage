using AiCommitMessage.Utility;
using FluentAssertions;

namespace AiCommitMessage.Tests.Utility;

public class GitVersionUtilityTests
{
    [Theory]
    [InlineData("+semver: minor Initial commit with some features", "+semver: minor")]
    [InlineData("Fixing bug +semver: patch", "+semver: patch")]
    [InlineData("+semver: major Breaking changes introduced", "+semver: major")]
    [InlineData("Refactoring code +semver: none", "+semver: none")]
    [InlineData("No version bump here", "")]
    public void TestProcessCommitMessage(string originalMessage, string expectedProcessedMessage)
    {
        // Arrange

        // Act
        var processedMessage = GitVersionUtility.ExtractGitVersionBumpCommand(originalMessage);

        // Assert
        processedMessage.Should().Be(expectedProcessedMessage);
    }

    [Fact]
    public void TestProcessCommitMessageWithMultipleSemverCommands()
    {
        // Arrange
        const string originalMessage =
            "+semver: minor Initial commit +semver: patch with some features";
        const string expectedProcessedMessage = "+semver: minor";

        // Act
        var processedMessage = GitVersionUtility.ExtractGitVersionBumpCommand(originalMessage);

        // Assert
        processedMessage.Should().Be(expectedProcessedMessage);
    }

    [Fact]
    public void TestProcessCommitMessageWithNoSemverCommand()
    {
        // Arrange
        const string originalMessage = "Regular commit message without semver command";

        // Act
        var processedMessage = GitVersionUtility.ExtractGitVersionBumpCommand(originalMessage);

        // Assert
        processedMessage.Should().BeEmpty();
    }

    [Fact]
    public void TestProcessCommitMessageWithWhitespaceOnly()
    {
        // Arrange
        const string originalMessage = "   ";

        // Act
        var processedMessage = GitVersionUtility.ExtractGitVersionBumpCommand(originalMessage);

        // Assert
        processedMessage.Should().BeEmpty();
    }
}
