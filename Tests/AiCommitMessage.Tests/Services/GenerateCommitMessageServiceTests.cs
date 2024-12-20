using System.Text.RegularExpressions;
using AiCommitMessage.Options;
using AiCommitMessage.Services;
using AiCommitMessage.Utility;
using FluentAssertions;

namespace AiCommitMessage.Tests.Services;

public class GenerateCommitMessageServiceTests
{
    //private readonly ChatClient _mockChatClient;
    private readonly GenerateCommitMessageService _service;

    public GenerateCommitMessageServiceTests()
    {
        Environment.SetEnvironmentVariable("OPENAI_API_KEY", "test");
        //_mockChatClient = Substitute.For<ChatClient>(
        //    "model-name",
        //    new ApiKeyCredential("key"),
        //    Substitute.For<OpenAIClientOptions>()
        //);
        _service = new GenerateCommitMessageService();
    }

    //[Fact]
    //public void GenerateCommitMessage_Should_ThrowException_When_BothBranchAndDiffAreEmpty()
    //{
    //    // Arrange
    //    var options = new GenerateCommitMessageOptions
    //    {
    //        Branch = string.Empty,
    //        Diff = string.Empty,
    //        Message = "Test message",
    //    };

    //    // Act
    //    Action act = () => _service.GenerateCommitMessage(options);

    //    // Assert
    //    act.Should()
    //        .Throw<InvalidOperationException>()
    //        .WithMessage("Unable to generate commit message: Both branch and diff are empty.");
    //}

    [Fact]
    public void GenerateCommitMessage_Should_ReturnMessage_When_MergeConflictResolutionDetected()
    {
        // Arrange
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/test",
            Diff = "Some diff",
            Message = "Merge branch 'feature/test' into main",
        };

        // Act
        var result = _service.GenerateCommitMessage(options);

        // Assert
        result.Should().Be("Merge branch 'feature/test' into main");
    }

    //[Fact]
    //public void GenerateCommitMessage_Should_IncludeBranchAndDiff_When_Provided()
    //{
    //    // Arrange
    //    var options = new GenerateCommitMessageOptions
    //    {
    //        Branch = "feature/test",
    //        Diff = "Added new feature",
    //        Message = "Initial commit"
    //    };

    //    _mockChatClient.CompleteChat(Arg.Any<SystemChatMessage>(), Arg.Any<UserChatMessage>())
    //        .Returns(new ChatCompletionResult
    //        {
    //            Value = new ChatCompletion
    //            {
    //                Content = new[] { new ChatMessage { Text = "Generated commit message" } }
    //            }
    //        });

    //    // Act
    //    var result = _service.GenerateCommitMessage(options);

    //    // Assert
    //    result.Should().Contain("Branch: feature/test");
    //    result.Should().Contain("Original message: Initial commit");
    //    result.Should().Contain("Git Diff: Added new feature");
    //}

    //[Fact]
    //public void GenerateCommitMessage_Should_DebugOutputToFile_When_DebugIsEnabled()
    //{
    //    // Arrange
    //    var options = new GenerateCommitMessageOptions
    //    {
    //        Branch = "feature/test",
    //        Diff = "Some diff",
    //        Message = "Initial commit",
    //        Debug = true
    //    };

    //    var chatCompletionResult = new ChatCompletionResult
    //    {
    //        Value = new ChatCompletion
    //        {
    //            Content = new[] { new ChatMessage { Text = "Generated commit message" } }
    //        }
    //    };

    //    _mockChatClient.CompleteChat(Arg.Any<SystemChatMessage>(), Arg.Any<UserChatMessage>())
    //        .Returns(chatCompletionResult);

    //    // Act
    //    var result = _service.GenerateCommitMessage(options);

    //    // Assert
    //    result.Should().Be("Generated commit message");
    //    var debugFileContent = File.ReadAllText("debug.json");
    //    debugFileContent.Should().Be(JsonSerializer.Serialize(chatCompletionResult));
    //}

    [Fact]
    public void GenerateCommitMessage_WithLlamaModel_Should_MatchExpectedPattern()
    {
        // Arrange
        Environment.SetEnvironmentVariable("AI_MODEL", "llama-3-1-405B-Instruct");
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/llama",
            Diff = "Add llama-specific functionality",
            Message = "Initial llama commit"
        };

        // Act
        var result = _service.GenerateCommitMessage(options);

        // Assert
        result.Should().MatchRegex("(?i)(?=.*add)(?=.*llama)");
    }
    [Fact]
    public void GenerateCommitMessage_WithGPTModel_Should_MatchExpectedPattern()
    {
        // Arrange
        Environment.SetEnvironmentVariable("AI_MODEL", "gpt-4o-mini", EnvironmentVariableTarget.User);

        var service = new GenerateCommitMessageService();
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/gpt",
            Diff = "Add GPT-specific improvements",
            Message = "Initial GPT commit"
        };

        // Act
        var result = service.GenerateCommitMessage(options);

        // Assert
        result.Should().MatchRegex("(?i)(?=.*add)(?=.*gpt)");
    }
}
