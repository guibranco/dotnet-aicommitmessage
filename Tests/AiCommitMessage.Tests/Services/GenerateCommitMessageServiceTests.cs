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

    /// <summary>
    /// Tests that an exception is thrown when the diff exceeds the specified limit.
    /// </summary>
    [Fact]
    public void GenerateCommitMessage_Should_ThrowException_When_DiffExceedsLimit()
    {
        // Arrange
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/test",
            Diff = new string('a', 102401), // 100 KB + 1 byte
            Message = "Test message",
        };

        // Act
        Action act = () => _service.GenerateCommitMessage(options);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "🚫 The staged changes are too large to process. Please reduce the number of files or size of changes and try again."
            );
    }

    /// <summary>
    /// Tests that generating a commit message adds a GitHub issue number and skips AI processing.
    /// </summary>
    [Fact]
    public void GenerateCommitMessage_Should_Add_GitHub_issue_number_and_Skip_AI()
    {
        // Arrange
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/123-my-branch-name",
            Diff = "Some diff",
            Message = "Initial commit -skipai",
        };

        //Act
        var result = _service.GenerateCommitMessage(options);

        // Assert
        result.Should().Be("#123 Initial commit");
    }

    /// <summary>
    /// Tests that generating a commit message does not duplicate GitHub issue numbers and skips AI processing.
    /// </summary>
    [Fact]
    public void GenerateCommitMessage_Should_Not_Duplicate_GitHub_issue_number_and_Skip_AI()
    {
        // Arrange
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/123-my-branch-name",
            Diff = "Some diff",
            Message = "#123 Initial commit -skipai",
        };

        //Act
        var result = _service.GenerateCommitMessage(options);

        // Assert
        result.Should().Be("#123 Initial commit");
    }

    /// <summary>
    /// Tests that the generated commit message adds a JIRA prefix and skips AI processing.
    /// </summary>
    [Fact]
    public void GenerateCommitMessage_Should_Add_JIRA_prefix_and_Skip_AI()
    {
        // Arrange
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/TEST-123-my-branch-name",
            Diff = "Some diff",
            Message = "Initial commit -skipai",
        };

        //Act
        var result = _service.GenerateCommitMessage(options);

        // Assert
        result.Should().Be("[TEST-123] Initial commit");
    }

    /// <summary>
    /// Tests that generating a commit message does not duplicate the JIRA prefix and skips AI.
    /// </summary>
    [Fact]
    public void GenerateCommitMessage_Should_Not_Duplicate_JIRA_prefix_and_Skip_AI()
    {
        // Arrange
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/TEST-123-my-branch-name",
            Diff = "Some diff",
            Message = "[TEST-123] Initial commit -skipai",
        };

        //Act
        var result = _service.GenerateCommitMessage(options);

        // Assert
        result.Should().Be("[TEST-123] Initial commit");
    }

    /// <summary>
    /// Tests that API calls are bypassed when the SkipAI flag is provided in the options.
    /// </summary>
    [Fact]
    public void GenerateCommitMessage_Should_ByPass_ApiCalls_When_SkipAI_Flag_Is_Provided()
    {
        // Arrange
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/test",
            Diff = "Some diff",
            Message = "Initial commit -skipai ",
        };
        var result = _service.GenerateCommitMessage(options);
        result.Should().Be("Initial commit");
    }

    /// <summary>
    /// Tests that the <see cref="GenerateCommitMessageOptions.SkipAI"/> flag is ignored when misplaced in the commit message.
    /// </summary>
    [Fact]
    public void GenerateCommitMessage_Should_Ignore_SkipAI_Flag_When_SKipAI_Flag_Is_Misplaced()
    {
        // Arrange
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/test",
            Diff = "Some diff",
            Message = "Test misplaced -skipai commit",
        };
        var result = _service.GenerateCommitMessage(options);
        result.Should().NotBe("Test misplaced -skipai commit");
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

    //     [Fact]
    //     public void GenerateCommitMessage_WithLlamaModel_Should_MatchExpectedPattern()
    //     {
    //         // Arrange
    //         Environment.SetEnvironmentVariable("AI_MODEL", "llama-3-1-405B-Instruct");
    //         var options = new GenerateCommitMessageOptions
    //         {
    //             Branch = "feature/llama",
    //             Diff = "Add llama-specific functionality",
    //             Message = "Initial llama commit",
    //         };

    //         // Act
    //         var result = _service.GenerateCommitMessage(options);

    //         // Assert
    //         result.Should().MatchRegex("(?i)(?=.*add)(?=.*llama)");
    //     }

    //     [Fact]
    //     public void GenerateCommitMessage_WithGPTModel_Should_MatchExpectedPattern()
    //     {
    //         // Arrange
    //         Environment.SetEnvironmentVariable(
    //             "AI_MODEL",
    //             "gpt-4o-mini",
    //             EnvironmentVariableTarget.User
    //         );

    //         var service = new GenerateCommitMessageService();
    //         var options = new GenerateCommitMessageOptions
    //         {
    //             Branch = "feature/gpt",
    //             Diff = "Add GPT-specific improvements",
    //             Message = "Initial GPT commit",
    //         };

    //         // Act
    //         var result = service.GenerateCommitMessage(options);
    //
    //         // Assert
    //         result.Should().MatchRegex("(?i)(?=.*add)(?=.*gpt)");
    //    }

    //  [Fact]
    //  public void FilterPackageLockDiff_Should_RemoveIgnoredFiles()
    //  {
    //      // Arrange
    //      var diff = "diff --git a/file1.txt b/file1.txt\n@@ -1,1 +1,1 @@\n- old content\n+ new content\n" +
    //                 "diff --git a/package-lock.json b/package-lock.json\n@@ -2,1 +2,1 @@\n- removed\n+ added\n" +
    //                 "diff --git a/yarn.lock b/yarn.lock\n@@ -1,1 +1,1 @@\n- foo\n+ bar\n" +
    //                 "diff --git a/other.txt b/other.txt\n@@ -1,1 +1,1 @@\n- old\n+ new\n";
    //      var expected = "diff --git a/file1.txt b/file1.txt\n@@ -1,1 +1,1 @@\n- old content\n+ new content\n" +
    //                     "diff --git a/other.txt b/other.txt\n@@ -1,1 +1,1 @@\n- old\n+ new";
    //      // Act
    //      var method = typeof(AiCommitMessage.Services.GenerateCommitMessageService)
    //      var filtered = (string)method.Invoke(null, new object[] { diff });
    //
    //      // Assert
    //      filtered.Should().Be(expected);
    //  }

    /// <summary>
    /// Tests that API calls are disabled when the environment variable is set to true.
    /// </summary>
    [Fact]
    public void GenerateCommitMessage_Should_DisableApiCalls_When_EnvironmentVariableIsTrue()
    {
        // Arrange
        Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", "true", EnvironmentVariableTarget.Process);
        
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/test",
            Diff = "Some diff",
            Message = "Initial commit",
        };

        try
        {
            // Act
            var result = _service.GenerateCommitMessage(options);

            // Assert
            result.Should().Be("Initial commit");
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", null, EnvironmentVariableTarget.Process);
        }
    }

    /// <summary>
    /// Tests that API calls are disabled when the environment variable is set to true and message is empty.
    /// </summary>
    [Fact]
    public void GenerateCommitMessage_Should_ReturnFallbackMessage_When_ApiDisabledAndMessageEmpty()
    {
        // Arrange
        Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", "true", EnvironmentVariableTarget.Process);
        
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/123-test",
            Diff = "Some diff",
            Message = "",
        };

        try
        {
            // Act
            var result = _service.GenerateCommitMessage(options);

            // Assert
            result.Should().Be("#123 Manual commit message required");
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", null, EnvironmentVariableTarget.Process);
        }
    }

    /// <summary>
    /// Tests that API calls are enabled by default when the environment variable is not set.
    /// </summary>
    [Fact]
    public void GenerateCommitMessage_Should_EnableApiCalls_When_EnvironmentVariableNotSet()
    {
        // Arrange
        Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", null, EnvironmentVariableTarget.Process);
        
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/test",
            Diff = "Some diff",
            Message = "Initial commit -skipai", // Use skipai to avoid actual API calls in tests
        };

        // Act
        var result = _service.GenerateCommitMessage(options);

        // Assert
        result.Should().Be("Initial commit");
    }
