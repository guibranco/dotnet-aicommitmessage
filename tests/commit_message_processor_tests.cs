using System;
using Xunit;

public class CommitMessageProcessorTests
{
    [Theory]
    [InlineData(
        "+semver: minor Initial commit with some features",
        "Initial commit with some features +semver: minor"
    )]
    [InlineData("Fixing bug +semver: patch", "Fixing bug +semver: patch")]
    [InlineData(
        "+semver: major Breaking changes introduced",
        "Breaking changes introduced +semver: major"
    )]
    [InlineData("Refactoring code +semver: none", "Refactoring code +semver: none")]
    [InlineData("No version bump here", "No version bump here")]
    public void TestProcessCommitMessage(string originalMessage, string expectedProcessedMessage)
    {
        string processedMessage = CommitMessageProcessor.ProcessCommitMessage(originalMessage);
        Assert.Equal(expectedProcessedMessage, processedMessage);
    }

    [Fact]
    public void TestProcessCommitMessageWithMultipleSemverCommands()
    {
        string originalMessage = "+semver: minor Initial commit +semver: patch with some features";
        string expectedProcessedMessage = "Initial commit with some features +semver: minor";
        string processedMessage = CommitMessageProcessor.ProcessCommitMessage(originalMessage);
        Assert.Equal(expectedProcessedMessage, processedMessage);
    }

    [Fact]
    public void TestProcessCommitMessageWithNoSemverCommand()
    {
        string originalMessage = "Regular commit message without semver command";
        string expectedProcessedMessage = "Regular commit message without semver command";
        string processedMessage = CommitMessageProcessor.ProcessCommitMessage(originalMessage);
        Assert.Equal(expectedProcessedMessage, processedMessage);
    }

    [Fact]
    public void TestProcessCommitMessageWithWhitespaceOnly()
    {
        string originalMessage = "   ";
        string expectedProcessedMessage = "";
        string processedMessage = CommitMessageProcessor.ProcessCommitMessage(originalMessage);
        Assert.Equal(expectedProcessedMessage, processedMessage);
    }
}

// Note: This is a basic set of tests. In a real-world scenario,
// you would expand these tests to cover more edge cases and scenarios.
