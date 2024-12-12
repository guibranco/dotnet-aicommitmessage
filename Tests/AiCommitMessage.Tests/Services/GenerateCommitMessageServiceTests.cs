using System;
using System.ClientModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using AiCommitMessage.Options;
using AiCommitMessage.Services;
using AiCommitMessage.Utility;
using FluentAssertions;
using NSubstitute;
using OpenAI;
using OpenAI.Chat;
using Xunit;

namespace AiCommitMessage.Tests;

public class GenerateCommitMessageServiceTests
{
    private readonly ChatClient _mockChatClient;
    private readonly GenerateCommitMessageService _service;

    public GenerateCommitMessageServiceTests()
    {
        _mockChatClient = Substitute.For<ChatClient>(null, Substitute.For<ApiKeyCredential>(), Substitute.For<OpenAIClientOptions>());
        _service = new GenerateCommitMessageService();
    }

    [Fact]
    public void GenerateCommitMessage_Should_ThrowException_When_BothBranchAndDiffAreEmpty()
    {
        // Arrange
        var options = new GenerateCommitMessageOptions
        {
            Branch = string.Empty,
            Diff = string.Empty,
            Message = "Test message"
        };

        // Act
        Action act = () => _service.GenerateCommitMessage(options);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Unable to generate commit message: Both branch and diff are empty.");
    }

    [Fact]
    public void GenerateCommitMessage_Should_ReturnMessage_When_MergeConflictResolutionDetected()
    {
        // Arrange
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/test",
            Diff = "Some diff",
            Message = "Merge branch 'feature/test' into main"
        };

        // Act
        var result = _service.GenerateCommitMessage(options);

        // Assert
        result.Should().Be("Merge branch 'feature/test' into main");
    }

    // [Fact]
    // public void GenerateCommitMessage_Should_IncludeBranchAndDiff_When_Provided()
    // {
    //     // Arrange
    //     var options = new GenerateCommitMessageOptions
    //     {
    //         Branch = "feature/test",
    //         Diff = "Added new feature",
    //         Message = "Initial commit"
    //     };

    //     _mockChatClient.CompleteChat(Arg.Any<SystemChatMessage>(), Arg.Any<UserChatMessage>())
    //         .Returns(new ChatCompletionResult
    //         {
    //             Value = new ChatCompletion
    //             {
    //                 Content = new[] { new ChatMessage { Text = "Generated commit message" } }
    //             }
    //         });

    //     // Act
    //     var result = _service.GenerateCommitMessage(options);

    //     // Assert
    //     result.Should().Contain("Branch: feature/test");
    //     result.Should().Contain("Original message: Initial commit");
    //     result.Should().Contain("Git Diff: Added new feature");
    // }

    // [Fact]
    // public void GenerateCommitMessage_Should_DebugOutputToFile_When_DebugIsEnabled()
    // {
    //     // Arrange
    //     var options = new GenerateCommitMessageOptions
    //     {
    //         Branch = "feature/test",
    //         Diff = "Some diff",
    //         Message = "Initial commit",
    //         Debug = true
    //     };

    //     var chatCompletionResult = new ChatCompletionResult
    //     {
    //         Value = new ChatCompletion
    //         {
    //             Content = new[] { new ChatMessage { Text = "Generated commit message" } }
    //         }
    //     };

    //     _mockChatClient.CompleteChat(Arg.Any<SystemChatMessage>(), Arg.Any<UserChatMessage>())
    //         .Returns(chatCompletionResult);

    //     // Act
    //     var result = _service.GenerateCommitMessage(options);

    //     // Assert
    //     result.Should().Be("Generated commit message");
    //     var debugFileContent = File.ReadAllText("debug.json");
    //     debugFileContent.Should().Be(JsonSerializer.Serialize(chatCompletionResult));
    // }

    [Fact]
    public void GetGitProvider_Should_ReturnCorrectProvider_When_OriginUrlMatches()
    {
        // Arrange
        var process = Substitute.For<Process>();
        process.StandardOutput.ReadToEnd().Returns("https://github.com/example/repo.git");

        // Act
        var result = typeof(GenerateCommitMessageService)
            .GetMethod("GetGitProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            .Invoke(null, null);

        // Assert
        result.Should().Be(GitProvider.GitHub);
    }
}
