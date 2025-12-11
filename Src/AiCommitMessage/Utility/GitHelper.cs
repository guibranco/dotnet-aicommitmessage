using System.Diagnostics;

namespace AiCommitMessage.Utility;

/// <summary>
/// Class GitHelper.
/// </summary>
public static class GitHelper
{
    /// <summary>
    /// Gets the name of the branch.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string GetBranchName()
    {
        return ExecuteGitCommand("rev-parse --abbrev-ref HEAD");
    }

    /// <summary>
    /// Gets the git difference.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string GetGitDiff()
    {
        return ExecuteGitCommand("diff --staged");
    }

    /// <summary>
    /// Executes the git command.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>System.String.</returns>
    private static string ExecuteGitCommand(string arguments)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();
        var result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return result.Trim();
    }
}
