# Commit Message Processor

This project includes a utility to process commit messages and preserve semantic versioning commands.

## Features

- Detects and preserves semantic versioning commands in commit messages.
- Supports the following commands:
  - `+semver: breaking`
  - `+semver: major`
  - `+semver: feature`
  - `+semver: minor`
  - `+semver: fix`
  - `+semver: patch`
  - `+semver: none`
  - `+semver: skip`
