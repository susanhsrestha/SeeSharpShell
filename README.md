[![progress-banner](https://backend.codecrafters.io/progress/shell/9727396d-8c2d-4315-a442-0fb397ec7185)](https://app.codecrafters.io/users/codecrafters-bot?r=2qF)
[![CI](https://github.com/susanhsrestha/SeeSharpShell/actions/workflows/ci.yml/badge.svg)](https://github.com/susanhsrestha/SeeSharpShell/actions/workflows/ci.yml)
[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://codespaces.new/susanhsrestha/SeeSharpShell)

# SeeSharpShell

A simple POSIX-compliant shell implementation in C#. This project is part of the ["Build Your Own Shell" Challenge](https://app.codecrafters.io/courses/shell/overview).

## Features

### Built-in Commands

| Command | Description |
|---------|-------------|
| `exit [code]` | Exit the shell with optional exit code (default: 0) |
| `echo [args]` | Display a line of text |
| `type <cmd>` | Display information about command type |
| `pwd` | Print the current working directory |
| `cd [dir]` | Change the current directory (supports `~` for home, `-` for previous directory) |
| `help` | Display help message with available commands |
| `clear` | Clear the terminal screen |
| `history` | Display command history |
| `env [name]` | Display environment variables (all or specific) |

### Additional Features

- **Command History**: All executed commands are stored in memory and can be viewed with the `history` command
- **Quoted Strings**: Supports both single and double quoted strings for arguments
- **PATH Resolution**: External commands are resolved from the `PATH` environment variable
- **Cross-platform**: Works on both Windows and Unix-like systems
- **Previous Directory**: Use `cd -` to return to the previous directory

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Running the Shell

```sh
./your_program.sh
```

Or directly with dotnet:

```sh
dotnet run --project codecrafters-shell.csproj
```

### Building

```sh
dotnet build --configuration Release
```

## Example Usage

```
$ help
SeeSharpShell - A simple POSIX-compliant shell

Built-in commands:
  exit [code]    Exit the shell with optional exit code (default: 0)
  echo [args]    Display a line of text
  type <cmd>     Display information about command type
  pwd            Print the current working directory
  cd [dir]       Change the current directory
  help           Display this help message
  clear          Clear the terminal screen
  history        Display command history
  env [name]     Display environment variables

External commands from PATH are also available.

$ echo 'Hello, World!'
Hello, World!

$ pwd
/home/user

$ cd /tmp
$ pwd
/tmp

$ cd -
/home/user

$ history
  1  help
  2  echo 'Hello, World!'
  3  pwd
  4  cd /tmp
  5  pwd
  6  cd -
  7  history

$ exit 0
```

## CodeCrafters Challenge

This project is a solution to the CodeCrafters "Build Your Own Shell" challenge. The challenge involves building a POSIX-compliant shell that can:

- Interpret shell commands
- Run external programs
- Implement built-in commands like cd, pwd, echo, and more

**Note**: If you're viewing this repo on GitHub, head over to [codecrafters.io](https://codecrafters.io) to try the challenge.

## CI/CD Pipeline

This repository includes a GitHub Actions workflow that runs on every push and pull request:

- **Build**: Compiles the project on Ubuntu, Windows, and macOS
- **Test**: Runs automated tests for all built-in shell commands
- **Status Badge**: Shows current build status at the top of this README

You can view the live test results in the Actions tab of this repository during PRs.

## Interactive Testing with GitHub Codespaces

Want to test the shell interactively during a PR? Use **GitHub Codespaces** to spin up a cloud-based development environment:

### How to Use

1. **Open a Codespace**: Click the "Open in GitHub Codespaces" badge at the top of this README, or:
   - Go to the repository or PR
   - Click the green "Code" button
   - Select "Codespaces" tab
   - Click "Create codespace on [branch]"

2. **Wait for Setup**: The environment will automatically:
   - Install .NET 9.0
   - Build the shell
   - Show you how to run it

3. **Run the Shell**: In the terminal, type:
   ```sh
   ./your_program.sh
   ```

4. **Test Interactively**: Type commands like `help`, `echo Hello`, `pwd`, etc.

5. **Stop When Done**: Simply close the browser tab. The Codespace will automatically stop after inactivity and can be deleted from your [Codespaces dashboard](https://github.com/codespaces).

### Benefits

- **No local setup required** - Everything runs in the cloud
- **Pre-configured environment** - .NET 9.0 and VS Code extensions ready
- **Isolated testing** - Each PR can have its own Codespace
- **Automatic cleanup** - Codespaces auto-stop after 30 minutes of inactivity
