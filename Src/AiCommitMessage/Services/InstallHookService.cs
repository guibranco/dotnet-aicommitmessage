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
        var hookPath = Path.Combine(Environment.CurrentDirectory, ".git/hooks/prepare-commit-msg");
        if (File.Exists(hookPath))
        {
            Output.ErrorLine("The prepare-commit-msg hook already exists.");
            return;
        }

        ExtractEmbeddedResource(".git/hooks", "AiCommitMessage", "prepare-commit-msg");
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
        using var stream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream(resourceLocation + "." + file);
        using var fileStream = new FileStream(Path.Combine(outputDir, file), FileMode.Create);
        for (var i = 0; i < stream.Length; i++)
        {
            fileStream.WriteByte((byte)stream.ReadByte());
        }

        fileStream.Close();
    }
}
