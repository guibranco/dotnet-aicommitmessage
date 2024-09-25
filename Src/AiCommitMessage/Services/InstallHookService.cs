using System.Diagnostics;
using System.Reflection;

namespace AiCommitMessage.Services;

/// <summary>
/// Class InstallHookService.
/// </summary>
internal class InstallHookService
{
    /// <summary>
    /// Installs the hook.
    /// </summary>
    public void InstallHook()
    {
        var directory = GetHooksDirectory();
        var hookPath = Path.Combine(directory, "prepare-commit-msg");
        if (File.Exists(hookPath))
        {
            Output.ErrorLine("The prepare-commit-msg hook already exists.");
            return;
        }

        ExtractEmbeddedResource(directory, "AiCommitMessage", "prepare-commit-msg");
        MakeExecutable(hookPath);
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
