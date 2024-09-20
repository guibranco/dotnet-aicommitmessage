# ![GIT Hooks + OpenAI - Generate GIT commit messages from Open AI](https://raw.githubusercontent.com/guibranco/dotnet-aicommitmessage/main/docs/images/splash.png)

🧠 🧰 This tool generates AI-powered commit messages via Git hooks, automating meaningful message suggestions from OpenAI and others to improve commit quality and efficiency.

---

## What this tool does

Generates a commit message based on the `git diff` result using the [OpenAI](https://platform.openai.com/docs/overview) API.

---

## Requirements

- [OpenAI API key](https://platform.openai.com/api-keys).
- [.NET 8.0 (or higher) runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
- [GIT client](https://git-scm.com/downloads).

---

## Getting started

1. Install the tool globally (or per project/repository).
2. Move to the project folder.
3. Install the Git hook on the default `hooks` directory.

```ps
dotnet tool install -g AiCommitMessage
cd my-project/
dotnet-aicommitmessage install-hook
git add .
git commit -m ""
```

Use `git log -1` to review the last commit details and find the automatically generated commit message.

---

## Commit message pattern

The training model for the AI used is designed using as reference these two guidelines:

- [Conventional Commits v1.0.0](https://www.conventionalcommits.org/en/v1.0.0/).
- [Padrões de Commits](https://github.com/tiagolofi/padroes-de-commits) (in Portuguese).

---

## Commands

This tool accepts an argument as the command to execute. Here is a list of available commands:

| Command                  | Description                                                                                                              |
|--------------------------|--------------------------------------------------------------------------------------------------------------------------|
| `install-hook`            | Installs GIT hooks in the default `.git/hooks` directory or in the custom directory configured in GIT settings.         |
| `generate-commit`         | Generates a commit message based on the current changes (`git diff` context).                                           |
| `set-openai-url`          | Configures a custom OpenAI API URL to generate commit messages.                                                         |
| `set-openai-key`          | Sets or updates the OpenAI API key required to access the OpenAI service.                                               |
| `set-cryptography-for-key`| Specifies whether the OpenAI API key should be stored in an encrypted format or as plain text in environment variables. |

