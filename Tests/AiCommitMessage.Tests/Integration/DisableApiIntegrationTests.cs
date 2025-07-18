using AiCommitMessage.Options;
using AiCommitMessage.Services;
using FluentAssertions;

namespace AiCommitMessage.Tests.Integration;

public class DisableApiIntegrationTests
{
    /// <summary>
    /// Integration test to verify the complete workflow when API is disabled.
    /// </summary>
    [Fact]
    public void CompleteWorkflow_Should_WorkCorrectly_When_ApiDisabled()
    {
        // Arrange
        Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", "true", EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("OPENAI_API_KEY", null, EnvironmentVariableTarget.Process);
        
        var service = new GenerateCommitMessageService();
        var options = new GenerateCommitMessageOptions
        {
            Branch = "feature/246-add-disable-api-option",
            Diff = "Added new environment variable support",
            Message = "Add option to disable API calls",
        };

        try
        {
            // Act
            var result = service.GenerateCommitMessage(options);

            // Assert
            result.Should().Be("#246 Add option to disable API calls");
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", null, EnvironmentVariableTarget.Process);
        }
    }

    /// <summary>
    /// Integration test to verify settings service works when API is disabled.
    /// </summary>
    [Fact]
    public void SettingsService_Should_WorkCorrectly_When_ApiDisabled()
    {
        // Arrange
        Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", "true", EnvironmentVariableTarget.Process);
        
        var options = new SetSettingsOptions
        {
            Model = "gpt-4o-mini"
        };

        try
        {
            // Act & Assert - Should not throw exception
            var act = () => SettingsService.SetSettings(options);
            act.Should().NotThrow();
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", null, EnvironmentVariableTarget.Process);
        }
    }
}