using System.Diagnostics;
using System.Reflection;
using AiCommitMessage.Options;

namespace AiCommitMessage.Services;

/// <summary>
/// Class InstallHookService.
/// </summary>
internal class InstallHookService
{
    /// <summary>
    /// Installs the hook.
    /// </summary>
    /// <param name="options">The options.</param>
    public void InstallHook(InstallHookOptions options)
    {
        var directory = options.Path;
        if (string.IsNullOrWhiteSpace(options.Path))
        {
            directory = Path.Combine(GetGitRepositoryRootLevel(), GetHooksDirectory());
        }

        var hookPath = Path.Combine(directory, "prepare-commit-msg");
        if (File.Exists(hookPath) && options.Override == false)
        {
            Output.ErrorLine("The prepare-commit-msg hook already exists.");
            return;
        }

        ExtractEmbeddedResource(directory, "AiCommitMessage", "prepare-commit-msg");
        if (IsChmodAvailable())
        {
            MakeExecutable(hookPath);
        }
    }

    /// <summary>
    /// Gets the git repository root level.
    /// </summary>
    /// <returns>System.String.</returns>
    private static string GetGitRepositoryRootLevel()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = "rev-parse --show-toplevel",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        using var process = new Process { StartInfo = processStartInfo };
        process.Start();
        var rootLevel = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return rootLevel.Trim();
    }

    /// <summary>
    /// Gets the hooks directory.
    /// </summary>
    /// <returns>System.String.</returns>
    private static string GetHooksDirectory()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = "config core.hooksPath",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process { StartInfo = processStartInfo };
        process.Start();
        var hooksPath = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        return string.IsNullOrEmpty(hooksPath) ? Path.Combine(".git", "hooks") : hooksPath.Trim();
    }

    /// <summary>
    /// Makes the executable.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    private static void MakeExecutable(string filePath)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "chmod",
            Arguments = $"+x {filePath}",
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process { StartInfo = processStartInfo };
        process.Start();
        process.WaitForExit();
    }

    /// <summary>
    /// Determines whether [is chmod available].
    /// </summary>
    /// <returns><c>true</c> if [is chmod available]; otherwise, <c>false</c>.</returns>
    private static bool IsChmodAvailable()
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "chmod",
                Arguments = "--version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using var process = Process.Start(startInfo);
            process?.WaitForExit();
            return process is { ExitCode: 0 };
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Extracts the embedded resource.
    /// </summary>
    /// <param name="outputDir">The output dir.</param>
    /// <param name="resourceLocation">The resource location.</param>
    /// <param name="file">The file.</param>
    private static void ExtractEmbeddedResource(
        string outputDir,
        string resourceLocation,
        string file
    )
    {
        using var stream = typeof(InstallHookService).Assembly.GetManifestResourceStream(
            resourceLocation + "." + file
        );
        using var fileStream = new FileStream(Path.Combine(outputDir, file), FileMode.Create);
        for (var i = 0; i < stream.Length; i++)
        {
            fileStream.WriteByte((byte)stream.ReadByte());
        }

        fileStream.Close();
    }
}
