using System.Diagnostics;
using AiCommitMessage.Utility;
using FluentAssertions;

namespace AiCommitMessage.Tests.Utility;

public class GitHelperTests : IDisposable
{
    private readonly string _repoPath;

    public GitHelperTests()
    {
        _repoPath = Path.Combine(Path.GetTempPath(), "aicm-git-tests-" + Guid.NewGuid());
        Directory.CreateDirectory(_repoPath);
        RunGit(_repoPath, "init -q");
        RunGit(_repoPath, "config user.email test@example.com");
        RunGit(_repoPath, "config user.name \"Test User\"");
    }

    public void Dispose()
    {
        try
        {
            ClearReadOnlyAttributes(_repoPath);
            Directory.Delete(_repoPath, true);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            // Best effort cleanup; git marks some object files read-only on Windows.
        }
    }

    private static void ClearReadOnlyAttributes(string path)
    {
        foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
        {
            File.SetAttributes(file, FileAttributes.Normal);
        }
    }

    [Fact]
    public void IsInitialCommit_Should_ReturnTrue_When_RepositoryHasNoCommitsYet()
    {
        // Act
        var result = GitHelper.IsInitialCommit(_repoPath);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsInitialCommit_Should_ReturnFalse_When_HeadAlreadyHasACommit()
    {
        // Arrange
        CommitFile(_repoPath, "file.txt", "content");

        // Act
        var result = GitHelper.IsInitialCommit(_repoPath);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsInitialCommit_Should_ReturnFalse_When_OnFeatureBranchWithExistingHistory()
    {
        // Arrange
        CommitFile(_repoPath, "file.txt", "content");
        RunGit(_repoPath, "checkout -q -b feature/test");

        // Act
        var result = GitHelper.IsInitialCommit(_repoPath);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsInitialCommit_Should_ReturnTrue_When_OnFreshOrphanBranchBeforeFirstCommit()
    {
        // Arrange
        CommitFile(_repoPath, "file.txt", "content");
        RunGit(_repoPath, "checkout -q --orphan fresh-branch");

        // Act
        var result = GitHelper.IsInitialCommit(_repoPath);

        // Assert
        result.Should().BeTrue();
    }

    private static void CommitFile(string repoPath, string fileName, string content)
    {
        File.WriteAllText(Path.Combine(repoPath, fileName), content);
        RunGit(repoPath, $"add {fileName}");
        RunGit(repoPath, "commit -q -m \"commit\"");
    }

    private static void RunGit(string repoPath, string arguments)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments,
            WorkingDirectory = repoPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();
        process.StandardOutput.ReadToEnd();
        process.StandardError.ReadToEnd();
        process.WaitForExit();
    }
}
