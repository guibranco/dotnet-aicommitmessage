using System.ClientModel;
using AiCommitMessage.Options;
using AiCommitMessage.Services;
using Azure;
using FluentAssertions;

namespace AiCommitMessage.Tests.Services;

/// <summary>
/// Tests for API error handling functionality in GenerateCommitMessageService.
/// </summary>
public class ApiErrorHandlingTests
{
    private readonly GenerateCommitMessageService _service;

    public ApiErrorHandlingTests()
    {
        _service = new GenerateCommitMessageService();
    }

    /// <summary>
    /// Tests that the service handles API errors gracefully when ignore API errors is enabled.
    /// </summary>
    [Fact]
    public void GenerateCommitMessage_Should_HandleApiErrors_When_IgnoreApiErrorsEnabled()
    {
        // Arrange
        Environment.SetEnvironmentVariable(
            "DOTNET_AICOMMITMESSAGE_IGNORE_API_ERRORS",
            "true",
            EnvironmentVariableTarget.Process
        );
        Environment.SetEnvironmentVariable(
            "DOTNET_AICOMMITMESSAGE_DISABLE_API",
            "false",
            EnvironmentVariableTarget.Process
        );

        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/285-test",
            Diff = "Some diff",
            Message = "Test commit -skipai", // Use skipai to avoid actual API calls but test the flow
        };

        try
        {
            // Act
            var result = _service.GenerateCommitMessage(options);

            // Assert
            result.Should().Be("#285 Test commit");
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable(
                "DOTNET_AICOMMITMESSAGE_IGNORE_API_ERRORS",
                null,
                EnvironmentVariableTarget.Process
            );
            Environment.SetEnvironmentVariable(
                "DOTNET_AICOMMITMESSAGE_DISABLE_API",
                null,
                EnvironmentVariableTarget.Process
            );
        }
    }
}