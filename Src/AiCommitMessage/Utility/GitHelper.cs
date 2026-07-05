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
    /// Determines whether the commit currently being prepared is the true initial commit,
    /// i.e. HEAD has no parent because no commits exist yet on the current ref.
    /// </summary>
    /// <remarks>
    /// This is true for a brand new repository before its first commit, and for a freshly
    /// created orphan branch (<c>git checkout --orphan</c>) before its first commit. It is
    /// false for the first commit made on a feature branch that was created from a ref that
    /// already has history, since HEAD there already points to an existing parent commit.
    /// </remarks>
    /// <returns><c>true</c> if there is no parent commit; otherwise, <c>false</c>.</returns>
    public static bool IsInitialCommit()
    {
        return IsInitialCommit(null);
    }

    /// <summary>
    /// Determines whether the commit currently being prepared is the true initial commit,
    /// running the underlying git command in the specified working directory.
    /// </summary>
    /// <param name="workingDirectory">The repository directory to check, or <c>null</c> to use the current directory.</param>
    /// <returns><c>true</c> if there is no parent commit; otherwise, <c>false</c>.</returns>
    internal static bool IsInitialCommit(string workingDirectory)
    {
        return string.IsNullOrWhiteSpace(
            ExecuteGitCommand("rev-parse --verify HEAD", workingDirectory)
        );
    }

    /// <summary>
    /// Executes the git command.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <param name="workingDirectory">The directory to run the command in, or <c>null</c> to use the current directory.</param>
    /// <returns>System.String.</returns>
    private static string ExecuteGitCommand(string arguments, string workingDirectory = null)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments,
            WorkingDirectory = workingDirectory ?? string.Empty,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();
        var result = process.StandardOutput.ReadToEnd();
        process.StandardError.ReadToEnd();
        process.WaitForExit();
        return result.Trim();
    }
}
