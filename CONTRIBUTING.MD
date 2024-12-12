# Contributing to dotnet-aicommitmessage

Thank you for your interest in contributing to the `dotnet-aicommitmessage` project! Your contributions are invaluable to making this tool even better for developers around the world. This guide will help you get started with contributing to the project.

## Getting Started 🚀

1. **Fork the repository**: Visit the [dotnet-aicommitmessage GitHub page](https://github.com/guibranco/dotnet-aicommitmessage) and click the `Fork` button.
2. **Clone your fork**: Clone your forked repository to your local machine:
   ```bash
   git clone https://github.com/<your-username>/dotnet-aicommitmessage.git
   ```
3. **Install dependencies**: Make sure you have the required .NET SDK installed (targeting .NET 8 or higher). You can check your installed version with:
   ```bash
   dotnet --version
   ```
   If needed, download the latest .NET SDK from [Microsoft's .NET Downloads](https://dotnet.microsoft.com/download).
4. **Create a branch**: Create a new branch for your feature or bug fix:
   ```bash
   git checkout -b <feature-or-bugfix-name>
   ```

## Development Guidelines 🛠️

### Code Style ✍️

- Follow standard .NET coding conventions. Refer to the [Microsoft .NET Coding Guidelines](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/style-rules) for more details.
- Use meaningful variable and method names.
- Ensure your code is clean, readable, and well-documented.

### Compatibility 💻

- The tool targets **.NET 8 and higher**. While it might technically run on lower versions, this is not officially supported or tested. If you are adding features or fixing bugs, ensure the tool retains compatibility with lower versions if possible.

### Testing ✅

- The project uses [xUnit](https://xunit.net/) for unit testing. Add or update tests for any new features or bug fixes.
- Make sure all tests pass before submitting your changes.
- Run the test suite locally using:
  ```bash
  dotnet test
  ```

### OpenAI Integration 🤖

- The tool uses the OpenAI API to generate commit messages based on staged content, branch names, and commit messages.
- If your changes impact how the tool interacts with the OpenAI API, ensure the functionality is well-tested.

### Documentation 📖

- Update the documentation if your changes affect the tool’s behavior or usage.
- Documentation includes inline comments, `README.md`, or any related markdown files.

### NuGet Package 🎵

- Ensure that `dotnet-aicommitmessage` remains compatible with NuGet publishing standards.
- Update the `csproj` file metadata if required.

## Submitting Your Contribution 📨

1. **Commit your changes**: Write meaningful and descriptive commit messages.
   ```
   [FEATURE] Add support for staging area summary
   ```
2. **Push your branch**: Push your branch to your forked repository:
   ```bash
   git push origin <feature-or-bugfix-name>
   ```
3. **Create a Pull Request (PR)**: Go to the original [dotnet-aicommitmessage repository](https://github.com/guibranco/dotnet-aicommitmessage) and open a pull request.

### Pull Request Checklist ✅

[ ] Code follows the .NET coding conventions.
[ ] All xUnit tests pass locally.
[ ] Code changes are covered with tests.
[ ] Documentation has been updated (if required).
[ ] PR description clearly explains the purpose and changes.

## Reporting Issues 🐞

If you encounter a bug or have a feature request, please open an issue in the [GitHub Issues](https://github.com/guibranco/dotnet-aicommitmessage/issues) section. Provide as much detail as possible:

- Steps to reproduce (for bugs).
- Use cases and rationale (for features).
- Environment details (e.g., OS, .NET SDK version).

## Community and Support 🤝

If you have questions or need assistance, feel free to open a discussion in the repository. We’re here to help!

Thank you for contributing to `dotnet-aicommitmessage`! Together, we can create a powerful and intelligent tool for developers.

