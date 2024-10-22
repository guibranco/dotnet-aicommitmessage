using Spectre.Console;

namespace AiCommitMessage.Utility;

public static class Output
{
    public static void InfoLine(string message) => InfoLine(message, Array.Empty<object>());

    /// <summary>
    /// Writes a formatted message to the console using markup.
    /// </summary>
    /// <param name="message">The message template that contains placeholders for formatting.</param>
    /// <param name="args">An array of objects to format and insert into the message template.</param>
    /// <remarks>
    /// This method utilizes the AnsiConsole class to output a formatted string to the console.
    /// The <paramref name="message"/> can include markup syntax for styling, such as colors and bold text.
    /// The <paramref name="args"/> parameter allows for a variable number of arguments, which are formatted
    /// according to the placeholders defined in the <paramref name="message"/>.
    /// This is particularly useful for creating dynamic and visually appealing console output.
    /// </remarks>
    public static void InfoLine(string message, params object[] args) =>
        AnsiConsole.MarkupLineInterpolated($"{string.Format(message, args)}");

    public static void ErrorLine(string message) => ErrorLine(message, Array.Empty<object>());

    /// <summary>
    /// Outputs an error message to the console with red formatting.
    /// </summary>
    /// <param name="message">The message template that contains placeholders for formatting.</param>
    /// <param name="args">An array of objects to format the message with.</param>
    /// <remarks>
    /// This method uses the AnsiConsole to display a formatted error message in red color.
    /// The message can include placeholders that will be replaced by the corresponding values from the <paramref name="args"/> array.
    /// This is useful for logging error messages in a visually distinct manner, making them stand out in the console output.
    /// </remarks>
    public static void ErrorLine(string message, params object[] args) =>
        AnsiConsole.MarkupLineInterpolated($"[red]{string.Format(message, args)}[/]");

    public static void TraceLine(string message) => TraceLine(message, Array.Empty<object>());

    /// <summary>
    /// Writes a formatted message to the console with a grey color.
    /// </summary>
    /// <param name="message">The message format string that contains placeholders for the arguments.</param>
    /// <param name="args">An array of objects to format and insert into the message.</param>
    /// <remarks>
    /// This method utilizes the AnsiConsole to output a message that is formatted according to the specified
    /// format string. The message is displayed in grey color, making it visually distinct from other console output.
    /// The method supports a variable number of arguments, allowing for flexible message formatting.
    /// It uses string interpolation to combine the message and its arguments before sending it to the console.
    /// </remarks>
    public static void TraceLine(string message, params object[] args) =>
        AnsiConsole.MarkupLineInterpolated($"[grey]{string.Format(message, args)}[/]");

    public static void WarningLine(string message) => WarningLine(message, Array.Empty<object>());

    /// <summary>
    /// Outputs a warning message to the console with yellow formatting.
    /// </summary>
    /// <param name="message">The message template that contains placeholders for formatting.</param>
    /// <param name="args">An array of objects to format the message template.</param>
    /// <remarks>
    /// This method uses the AnsiConsole to display a formatted warning message in yellow color.
    /// The message can include placeholders that will be replaced by the corresponding values from the <paramref name="args"/> array.
    /// The formatted message is displayed as an interpolated string, allowing for dynamic content to be included in the output.
    /// This is useful for providing feedback or alerts to users in a visually distinct manner.
    /// </remarks>
    public static void WarningLine(string message, params object[] args) =>
        AnsiConsole.MarkupLineInterpolated($"[yellow]{string.Format(message, args)}[/]");
}
