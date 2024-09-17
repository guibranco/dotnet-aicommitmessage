# ![GIT Hooks + OpenAI - Generate GIT commit messages from Open AI](docs/images/splash.png)

🧠 🧰 This tool generates AI-powered commit messages via Git hooks, automating meaningful message suggestions from OpenAI and others to improve commit quality and efficiency.

---

## What this tool do

Generates a commit message based on `git diff` result using the [OpenAI](https://platform.openai.com/docs/overview) API.

---

## Getting started

1. Install the tool globally (or per project/repositoy).
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

## Requirements

- [OpenAI API key](https://platform.openai.com/api-keys).
- [.NET 8.0 (or higher) runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
- [GIT client](https://git-scm.com/downloads).

---

## Commit message pattern

The training model for the AI used is designed using as reference these two guidelines:

- [Conventional Commits v1.0.0](https://www.conventionalcommits.org/en/v1.0.0/).
- [Padrões de Commits](https://github.com/tiagolofi/padroes-de-commits) (in Portuguese).
  
