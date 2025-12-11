using AiCommitMessage.Options;
using AiCommitMessage.Utility;

namespace AiCommitMessage.Services;

/// <summary>
/// Service for managing environment variables.
/// </summary>
public static class EnvironmentVariableService
{
    /// <summary>
    /// Outputs an error message and sets the exit code to 1.
    /// </summary>
    /// <param name="message">The error message to display.</param>
    private static void ErrorLine(string message)
    {
        Output.ErrorLine(message);
        Environment.ExitCode = 1;
    }

    /// <summary>
    /// Sets an environment variable based on the provided options.
    /// </summary>
    /// <param name="setEnvironmentVariableOptions">The options containing the variable name, value, and target scope.</param>
    /// <remarks>
    /// This method parses the variable string in the format "VAR_NAME=value" and sets the environment variable
    /// to the specified target scope (User or Machine). If no target is specified, it defaults to User scope.
    /// Setting environment variables with Machine scope requires administrator privileges on Windows.
    /// </remarks>
    /// <exception cref="System.Security.SecurityException">Thrown when attempting to set a Machine-level variable without sufficient permissions.</exception>
    /// <exception cref="ArgumentException">Thrown when the variable name contains invalid characters.</exception>
    public static void SetEnvironmentVariable(
        SetEnvironmentVariableOptions setEnvironmentVariableOptions
    )
    {
        if (string.IsNullOrWhiteSpace(setEnvironmentVariableOptions.Variable))
        {
            ErrorLine("Variable cannot be null or empty.");
            return;
        }

        var variable = setEnvironmentVariableOptions.Variable.Split('=', 2);
        if (variable.Length != 2)
        {
            ErrorLine("Invalid variable format. Please use the format: VAR_NAME=value");
            return;
        }

        var variableName = variable[0].Trim();
        var variableValue = variable[1].Trim();
        if (string.IsNullOrWhiteSpace(variableName))
        {
            ErrorLine("Variable name cannot be empty.");
            return;
        }

        EnvironmentVariableTarget envTarget;
        if (
            string.Equals(
                setEnvironmentVariableOptions.Target,
                "Machine",
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            envTarget = EnvironmentVariableTarget.Machine;
        }
        else if (
            string.Equals(
                setEnvironmentVariableOptions.Target,
                "User",
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            envTarget = EnvironmentVariableTarget.User;
        }
        else
        {
            ErrorLine(
                $"Invalid target '{setEnvironmentVariableOptions.Target}'. Please use 'User' or 'Machine'."
            );
            return;
        }

        try
        {
            Environment.SetEnvironmentVariable(variableName, variableValue, envTarget);

            Output.InfoLine(
                $"Environment variable '{variableName}' set to '{variableValue}' for {envTarget} scope."
            );
        }
        catch (System.Security.SecurityException)
        {
            ErrorLine(
                "Permission denied. Setting Machine-level environment variables requires administrator privileges."
            );
        }
        catch (ArgumentException ex)
        {
            ErrorLine($"Invalid argument: {ex.Message}");
        }
        catch (Exception ex)
        {
            ErrorLine($"An error occurred while setting the environment variable: {ex.Message}");
        }
    }
}
