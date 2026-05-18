using AiCommitMessage.Options;
using AiCommitMessage.Services;
using FluentAssertions;

namespace AiCommitMessage.Tests.Services;

public class EnvironmentVariableServiceTests
{
    /// <summary>
    /// Tests that setting a User-level environment variable works correctly.
    /// </summary>
    [Fact]
    public void SetEnvironmentVariable_Should_SetUserVariable_When_TargetIsUser()
    {
        var options = new SetEnvironmentVariableOptions
        {
            Variable = "TEST_USER_VAR=test_value",
            Target = "User",
        };

        try
        {
            EnvironmentVariableService.SetEnvironmentVariable(options);

            var result = Environment.GetEnvironmentVariable(
                "TEST_USER_VAR",
                EnvironmentVariableTarget.User
            );
            result.Should().Be("test_value");
        }
        finally
        {
            Environment.SetEnvironmentVariable(
                "TEST_USER_VAR",
                null,
                EnvironmentVariableTarget.User
            );
        }
    }

    /// <summary>
    /// Tests that setting a User-level environment variable works when target is not specified (default).
    /// </summary>
    [Fact]
    public void SetEnvironmentVariable_Should_SetUserVariable_When_TargetIsNotSpecified()
    {
        var options = new SetEnvironmentVariableOptions
        {
            Variable = "TEST_DEFAULT_VAR=default_value",
            Target = "User",
        };

        try
        {
            EnvironmentVariableService.SetEnvironmentVariable(options);

            var result = Environment.GetEnvironmentVariable(
                "TEST_DEFAULT_VAR",
                EnvironmentVariableTarget.User
            );
            result.Should().Be("default_value");
        }
        finally
        {
            Environment.SetEnvironmentVariable(
                "TEST_DEFAULT_VAR",
                null,
                EnvironmentVariableTarget.User
            );
        }
    }

    /// <summary>
    /// Tests that an error is returned when variable format is invalid (missing =).
    /// </summary>
    [Fact]
    public void SetEnvironmentVariable_Should_SetExitCode_When_FormatIsInvalid()
    {
        var options = new SetEnvironmentVariableOptions
        {
            Variable = "INVALID_FORMAT",
            Target = "User",
        };

        var originalExitCode = Environment.ExitCode;

        try
        {
            EnvironmentVariableService.SetEnvironmentVariable(options);

            Environment.ExitCode.Should().Be(1);
        }
        finally
        {
            Environment.ExitCode = originalExitCode;
        }
    }

    /// <summary>
    /// Tests that an error is returned when variable is null or empty.
    /// </summary>
    [Fact]
    public void SetEnvironmentVariable_Should_SetExitCode_When_VariableIsNull()
    {
        var options = new SetEnvironmentVariableOptions { Variable = null, Target = "User" };

        var originalExitCode = Environment.ExitCode;

        try
        {
            EnvironmentVariableService.SetEnvironmentVariable(options);

            Environment.ExitCode.Should().Be(1);
        }
        finally
        {
            Environment.ExitCode = originalExitCode;
        }
    }

    /// <summary>
    /// Tests that an error is returned when variable name is empty.
    /// </summary>
    [Fact]
    public void SetEnvironmentVariable_Should_SetExitCode_When_VariableNameIsEmpty()
    {
        var options = new SetEnvironmentVariableOptions { Variable = "=value", Target = "User" };

        var originalExitCode = Environment.ExitCode;

        try
        {
            EnvironmentVariableService.SetEnvironmentVariable(options);

            Environment.ExitCode.Should().Be(1);
        }
        finally
        {
            Environment.ExitCode = originalExitCode;
        }
    }

    /// <summary>
    /// Tests that an error is returned when target is invalid.
    /// </summary>
    [Fact]
    public void SetEnvironmentVariable_Should_SetExitCode_When_TargetIsInvalid()
    {
        var options = new SetEnvironmentVariableOptions
        {
            Variable = "TEST_VAR=value",
            Target = "InvalidTarget",
        };

        var originalExitCode = Environment.ExitCode;

        try
        {
            EnvironmentVariableService.SetEnvironmentVariable(options);

            Environment.ExitCode.Should().Be(1);
        }
        finally
        {
            Environment.ExitCode = originalExitCode;
        }
    }

    /// <summary>
    /// Tests that target is case-insensitive.
    /// </summary>
    [Theory]
    [InlineData("user")]
    [InlineData("USER")]
    [InlineData("User")]
    [InlineData("uSeR")]
    public void SetEnvironmentVariable_Should_AcceptCaseInsensitiveTarget(string target)
    {
        var options = new SetEnvironmentVariableOptions
        {
            Variable = "TEST_CASE_VAR=case_value",
            Target = target,
        };

        try
        {
            EnvironmentVariableService.SetEnvironmentVariable(options);

            var result = Environment.GetEnvironmentVariable(
                "TEST_CASE_VAR",
                EnvironmentVariableTarget.User
            );
            result.Should().Be("case_value");
        }
        finally
        {
            Environment.SetEnvironmentVariable(
                "TEST_CASE_VAR",
                null,
                EnvironmentVariableTarget.User
            );
        }
    }
}
