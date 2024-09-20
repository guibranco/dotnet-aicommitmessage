# ![GIT Hooks + OpenAI - Generate GIT commit messages from Open AI](https://raw.githubusercontent.com/guibranco/dotnet-aicommitmessage/main/docs/images/splash.png)

🧠 🧰 This tool generates AI-powered commit messages via Git hooks, automating meaningful message suggestions from OpenAI and others to improve commit quality and efficiency.

[![GitHub last commit](https://img.shields.io/github/last-commit/guibranco/dotnet-aicommitmessage)](https://wakatime.com/badge/github/guibranco/dotnet-aicommitmessage)
[![GitHub license](https://img.shields.io/github/license/guibranco/dotnet-aicommitmessage)](https://wakatime.com/badge/github/guibranco/dotnet-aicommitmessage)
[![time tracker](https://wakatime.com/badge/github/guibranco/dotnet-aicommitmessage.svg)](https://wakatime.com/badge/github/guibranco/dotnet-aicommitmessage)

[![Build](https://github.com/guibranco/dotnet-aicommitmessage/actions/workflows/build.yml/badge.svg)](https://github.com/guibranco/dotnet-aicommitmessage/actions/workflows/build.yml)
[![Continuous Integration](https://github.com/guibranco/dotnet-aicommitmessage/actions/workflows/ci.yml/badge.svg)](https://github.com/guibranco/dotnet-aicommitmessage/actions/workflows/ci.yml)
[![Infisical secrets check](https://github.com/guibranco/dotnet-aicommitmessage/actions/workflows/infisical-secrets-check.yml/badge.svg)](https://github.com/guibranco/dotnet-aicommitmessage/actions/workflows/infisical-secrets-check.yml)
[![Linter check](https://github.com/guibranco/dotnet-aicommitmessage/actions/workflows/linter.yml/badge.svg)](https://github.com/guibranco/dotnet-aicommitmessage/actions/workflows/linter.yml)

---

> [!CAUTION]
> This is still in development/beta. It will be **GA** when a major release 1 becomes available.

## What this tool does

Generates a commit message based on the `git diff` result using the [OpenAI API](https://platform.openai.com/docs/overview).

---

## Requirements

- [OpenAI API key](https://platform.openai.com/api-keys).
- [.NET 8.0 (or higher) runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
- [GIT client](https://git-scm.com/downloads).

---

## Getting started

[![AICommitMessage NuGet Version](https://img.shields.io/nuget/v/AICommitMessage.svg?style=flat)](https://www.nuget.org/packages/AICommitMessage/)
[![AICommitMessage NuGet Downloads](https://img.shields.io/nuget/dt/AICommitMessage.svg?style=flat)](https://www.nuget.org/packages/AICommitMessage/)

This repository is available at [NuGet](https://www.nuget.org) under the name [AICommitMessage](https://www.nuget.org/packages/AICommitMessage/).

### Installation

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

| Command                    | Description                                                                                                             |
| -------------------------- | ----------------------------------------------------------------------------------------------------------------------- |
| `install-hook`             | Installs GIT hooks in the default `.git/hooks` directory or in the custom directory configured in GIT settings.         |
| `generate-message`         | Generates a commit message based on the current changes (`git diff` context).                                           |
| `set-openai-url`           | Configures a custom OpenAI API URL to generate commit messages.                                                         |
| `set-openai-key`           | Sets or updates the OpenAI API key required to access the OpenAI service.                                               |
| `set-cryptography-for-key` | Specifies whether the OpenAI API key should be stored in an encrypted format or as plain text in environment variables. |
