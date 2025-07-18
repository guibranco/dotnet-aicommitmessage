using AiCommitMessage.Utility;
using FluentAssertions;

namespace AiCommitMessage.Tests.Utility;

public class EnvironmentLoaderTests
{
    /// <summary>
    /// Tests that IsApiDisabled returns false when the environment variable is not set.
    /// </summary>
    [Fact]
    public void IsApiDisabled_Should_ReturnFalse_When_EnvironmentVariableNotSet()
    {
        // Arrange
        Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", null, EnvironmentVariableTarget.Process);

        // Act
        var result = EnvironmentLoader.IsApiDisabled();

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that IsApiDisabled returns true when the environment variable is set to "true".
    /// </summary>
    [Fact]
    public void IsApiDisabled_Should_ReturnTrue_When_EnvironmentVariableIsTrue()
    {
        // Arrange
        Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", "true", EnvironmentVariableTarget.Process);

        try
        {
            // Act
            var result = EnvironmentLoader.IsApiDisabled();

            // Assert
            result.Should().BeTrue();
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", null, EnvironmentVariableTarget.Process);
        }
    }

    /// <summary>
    /// Tests that IsApiDisabled returns false when the environment variable is set to "false".
    /// </summary>
    [Fact]
    public void IsApiDisabled_Should_ReturnFalse_When_EnvironmentVariableIsFalse()
    {
        // Arrange
        Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", "false", EnvironmentVariableTarget.Process);

        try
        {
            // Act
            var result = EnvironmentLoader.IsApiDisabled();

            // Assert
            result.Should().BeFalse();
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", null, EnvironmentVariableTarget.Process);

    /// <summary>
    /// Tests that LoadOpenAiApiKey returns empty string when API is disabled, even without API key set.
    /// </summary>
    [Fact]
    public void LoadOpenAiApiKey_Should_ReturnEmptyString_When_ApiDisabledAndNoKeySet()
    {
        // Arrange
        Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", "true", EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("OPENAI_API_KEY", null, EnvironmentVariableTarget.Process);

        try
        {
            // Act
            var result = EnvironmentLoader.LoadOpenAiApiKey();

            // Assert
            result.Should().BeEmpty();
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("DOTNET_AICOMMITMESSAGE_DISABLE_API", null, EnvironmentVariableTarget.Process);
        }
    }
}
