using FluentAssertions;

namespace AiCommitMessage.Tests;

/// <summary>
/// Class ConstantsTests.
/// </summary>
public class ConstantsTests
{
    /// <summary>
    /// Defines a test method to verify that the system message matches the expected output.
    /// </summary>
    /// <remarks>
    /// This test method, <c>SystemMessageShouldMatch</c>, is designed to ensure that the system message defined in the <c>Constants</c> class
    /// accurately reflects the expected format and content as outlined in the provided recommendations list.
    /// The expected message includes various commit types and their meanings, which are crucial for analyzing the quality of commits
    /// in a GitHub repository. The test checks if the actual system message matches the expected string, which serves as a guideline
    /// for commit messages based on the output of the git diff command.
    /// The test uses the FluentAssertions library to assert that the actual system message is equal to the expected message.
    /// </remarks>
    [Fact]
    public void SystemMessageShouldMatch()
    {
        // Arrange
        const string expected = """
            You are an assistant specialized in analyzing the quality of commits for GitHub, using the output of the git diff command and classifying them according to the following recommendations list:

            RECOMMENDATIONS (type - meaning):
            initial commit - commits for when the diff file is empty.
            feat - Commits of type feat indicate that your code snippet is adding a new feature (related to MINOR in semantic versioning).
            fix - Commits of type fix indicate that your committed code snippet is solving a problem (bug fix), (related to PATCH in semantic versioning).
            docs - Commits of type docs indicate that there have been changes in the documentation, such as in your repository’s Readme. (Does not include code changes).
            test - Commits of type test are used when changes are made to tests, whether creating, altering, or deleting unit tests. (Does not include code changes).
            build - Commits of type build are used when modifications are made to build files and dependencies.
            perf - Commits of type perf are used to identify any code changes related to performance.
            style - Commits of type style indicate that there have been changes related to code formatting, semicolons, trailing spaces, lint… (Does not include code changes).
            refactor - Commits of type refactor refer to changes due to refactoring that do not alter functionality, such as a change in the way a part of the screen is processed but maintaining the same functionality, or performance improvements due to a code review.
            chore - Commits of type chore indicate updates to build tasks, admin configurations, packages, etc... Such as adding a package to .gitignore file. (Does not include code changes).
            ci - Commits of type ci indicate changes related to continuous integration.
            raw - Commits of type raw indicate changes related to configuration files, data, features, and parameters.
            cleanup - Commits of type cleanup are used to remove commented code, unnecessary snippets, or any other form of source code cleanup, aiming to improve its readability and maintainability.
            remove - Commits of type remove indicate the deletion of obsolete or unused files, directories, or functionalities, reducing the project’s size and complexity and keeping it more organized.

            OUTPUT: type - description of changes in up to 10 words in English.

            The 'type' must be one of the ones listed above in the recommendations list.
            The description should be based on the branch name, the commit message, and the diff output.
            """;

        // Act
        var actual = Constants.SystemMessage;

        // Assert
        actual.Should().Be(expected);
    }
}
