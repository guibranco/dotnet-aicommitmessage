using System.ClientModel;
using System.Diagnostics;
using System.Text.Json;
using AiCommitMessage.Options;
using AiCommitMessage.Utility;
using OpenAI;
using OpenAI.Chat;

/// <summary>
/// Class GitHelper.
/// </summary>
public class GitHelper
{
    // Retrieves the current branch name
    public static string GetBranchName()
    {
        return ExecuteGitCommand("rev-parse --abbrev-ref HEAD");
    }

    // Retrieves the staged diff
    public static string GetGitDiff()
    {
        return ExecuteGitCommand("diff --staged");
    }

    // Executes the provided GIT command and returns the result
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

        using var process = new Process { StartInfo = processStartInfo };
        process.Start();
        var result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return result.Trim();
    }
}
