using System.Diagnostics;
using AiCommitMessage.Options;
using AiCommitMessage.Utility;

namespace AiCommitMessage.Services;

/// <summary>
/// Class InstallHookService.
/// </summary>
internal class InstallHookService
{
    /// <summary>
    /// Installs a Git hook for preparing commit messages.
    /// </summary>
    /// <param name="options">The options for installing the hook, including the path and override settings.</param>
    /// <remarks>
    /// This method installs a "prepare-commit-msg" hook in the specified directory. If the provided path is null or whitespace,
    /// it defaults to the hooks directory within the Git repository root. If a hook already exists at the specified path and
    /// the override option is set to false, an error message is displayed, and the installation is aborted. If the hook does
    /// not exist or if the override option is true, the method extracts an embedded resource for the hook and places it in
    /// the specified directory. Additionally, if the system supports changing file permissions, it makes the hook executable.
    /// </remarks>
    public void InstallHook(InstallHookOptions options)
    {
        var directory = options.Path;
        if (string.IsNullOrWhiteSpace(options.Path))
        {
            directory = Path.Combine(GetGitRepositoryRootLevel(), GetHooksDirectory());
            EnsureDirectoryExists(directory);
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
    /// Retrieves the root level of the current Git repository.
    /// </summary>
    /// <remarks>
    /// This method executes a Git command to determine the top-level directory of the current repository.
    /// It uses the `git rev-parse --show-toplevel` command, which outputs the absolute path of the root directory of the Git repository.
    /// The method sets up a process to run the command, redirects the standard output to capture the result, and waits for the process to complete.
    /// The output is then trimmed to remove any leading or trailing whitespace before being returned.
    /// This method assumes that Git is installed and available in the system's PATH.
    /// </remarks>
    /// <returns>The absolute path of the root directory of the current Git repository.</returns>
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
        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();
        var rootLevel = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return rootLevel.Trim();
    }

    /// <summary>
    /// Retrieves the directory path for Git hooks.
    /// </summary>
    /// <returns>
    /// The path to the Git hooks directory. If the hooks path is not set in the Git configuration,
    /// it defaults to the ".git/hooks" directory within the current repository.
    /// </returns>
    /// <remarks>
    /// This method executes a Git command to fetch the configured hooks path using the command:
    /// `git config core.hooksPath`. It sets up a <see cref="ProcessStartInfo"/> to run the command
    /// without creating a new window and redirects the standard output to capture the result.
    /// After executing the command, it checks if the output is empty. If it is, the method returns
    /// a default path of ".git/hooks". Otherwise, it returns the trimmed output from the command,
    /// which represents the custom hooks directory set in the Git configuration.
    /// </remarks>
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

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();
        var hooksPath = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        return string.IsNullOrEmpty(hooksPath) ? Path.Combine(".git", "hooks") : hooksPath.Trim();
    }

    /// <summary>
    /// Modifies the file permissions of the specified file to make it executable.
    /// </summary>
    /// <param name="filePath">The path to the file that needs to be made executable.</param>
    /// <remarks>
    /// This method uses the Unix command 'chmod' to change the file permissions of the specified <paramref name="filePath"/>.
    /// It sets the executable bit for the file, allowing it to be run as a program.
    /// The method creates a new process to execute the command and waits for it to complete before returning.
    /// Note that this method is intended for use in environments where the 'chmod' command is available, such as Unix-like operating systems.
    /// </remarks>
    private static void MakeExecutable(string filePath)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "chmod",
            Arguments = $"+x {filePath}",
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();
        process.WaitForExit();
    }

    /// <summary>
    /// Checks if the 'chmod' command is available on the system.
    /// </summary>
    /// <returns>
    /// Returns true if the 'chmod' command is available (i.e., it executes successfully),
    /// otherwise returns false.
    /// </returns>
    /// <remarks>
    /// This method attempts to start a process that runs the 'chmod --version' command
    /// to check for its availability. It sets up the process with specific start information,
    /// including redirecting standard output and error, and ensuring that it does not use
    /// the shell to execute the command. The method waits for the process to exit and checks
    /// the exit code to determine if the command was successful. If any exception occurs during
    /// this process, it catches the exception and returns false, indicating that 'chmod' is not available.
    /// </remarks>
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
    /// Extracts an embedded resource from the assembly and saves it to the specified output directory.
    /// </summary>
    /// <param name="outputDir">The directory where the extracted resource will be saved.</param>
    /// <param name="resourceLocation">The location of the embedded resource within the assembly.</param>
    /// <param name="file">The name of the file to be created from the embedded resource.</param>
    /// <remarks>
    /// This method retrieves a stream for the specified embedded resource using its location and file name.
    /// It then creates a new file in the specified output directory and writes the contents of the resource stream to this file byte by byte.
    /// The method ensures that the file is created with the correct content from the embedded resource, allowing for easy access to resources packaged within the assembly.
    /// Note that this method assumes that the resource exists and does not handle cases where the resource may not be found.
    /// </remarks>
    private static void ExtractEmbeddedResource(
        string outputDir,
        string resourceLocation,
        string file
    )
    {
        EnsureDirectoryExists(outputDir);
        using var stream = typeof(InstallHookService).Assembly.GetManifestResourceStream(
            resourceLocation + "." + file
        );
        using var fileStream = new FileStream(Path.Combine(outputDir, file), FileMode.Create);
        if (stream != null)
        {
            for (var i = 0; i < stream.Length; i++)
            {
                fileStream.WriteByte((byte)stream.ReadByte());
            }
        }

        fileStream.Close();
    }

    /// <summary>
    /// Ensures that the specified directory exists by creating it if it does not.
    /// </summary>
    /// <param name="path">The path of the directory to ensure exists.</param>
    /// <remarks>
    /// This method checks the directory name extracted from the provided <paramref name="path"/>.
    /// If the directory name is not null and has a length greater than zero, it attempts to create the directory
    /// using the <see cref="Directory.CreateDirectory"/> method. This is useful for ensuring that a directory
    /// is available before performing file operations that require it.
    /// </remarks>
    private static void EnsureDirectoryExists(string path)
    {
        var directoryName = Path.GetDirectoryName(path);
        if (directoryName is { Length: > 0 })
        {
            Directory.CreateDirectory(directoryName);
        }
    }
}
