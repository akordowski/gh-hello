# gh-hello - A GitHub CLI Extension Template

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Build status](https://github.com/akordowski/gh-hello/workflows/CI/badge.svg)](../../actions?query=workflow%3ACI)

This is a template repository for creating a [GitHub CLI](https://github.com/cli/cli#installation) extension based on **dotnet/C#**. It is based on the [gh-gei](https://github.com/github/gh-gei) project. It contains a [workflow](.github/workflows/CI.yml) for build, test and publish the extension out of the box.

## Test the `gh-hello` extension

```
gh extension install akordowski/gh-hello
gh hello --help
gh hello --version
gh hello hello --name John
gh extension remove hello
```

## Creating new GitHub CLI extension

- Clone this repository, delete the `.git` folder, init a new Git repository
- Create a new GitHub repository with the name of the extension, e.g. `gh-EXTENSION_NAME`
- Create a new personal access token (classic), apply the `repo` and `workflow` scopes
- Add a new GitHub Actions repository secret `PUBLISH_PAT`
- Clear the `LATEST-VERSION.txt` and `RELEASENOTES.md` files
- Delete the `releasenotes` folder
- Adjust the following files and all `Hello` project references based on your extension name
  - `.github/workflows/CI.yml`
  - `.vscode/launch.json`
  - `.vscode/settings.json`
  - `.vscode/tasks.json`
  - `src` & `tests` projects
  - `gh-hello.sln`
  - `justfile`
  - `publish.ps1`
  - `README.md`
- Create a new extension
- Add new release notes to the `RELEASENOTES.md` file, e.g. `Initial release`
- Commit all changes
- Tag the commit, e.g. `v0.0.1`
- Push the commit with the tag to the repository
- The `CI` workflow is triggered, builds, test and publish a new release of the GitHub CLI extension