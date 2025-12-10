namespace AiCommitMessage.Utility;

/// <summary>
/// Represents the different Git providers.
/// </summary>
public enum GitProvider
{
    /// <summary>
    /// The unidentified.
    /// </summary>
    Unidentified = 0,

    /// <summary>
    /// The Azure devops.
    /// </summary>
    AzureDevOps,

    /// <summary>
    /// The BitBucket.
    /// </summary>
    Bitbucket,

    /// <summary>
    /// The GitHub.
    /// </summary>
    GitHub,

    /// <summary>
    /// The GitLab.
    /// </summary>
    GitLab,
}