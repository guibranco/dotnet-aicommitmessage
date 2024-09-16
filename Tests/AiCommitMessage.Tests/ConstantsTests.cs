using FluentAssertions;

namespace AiCommitMessage.Tests;

public class ConstantsTests
{
    [Fact]
    public void Test1()
    {
        // Arrange
        const string expected = "Hello World!";

        // Act
        var actual = Constants.SystemMessage;

        // Assert
        actual.Should().Be(expected);
    }
}
