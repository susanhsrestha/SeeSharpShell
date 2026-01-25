using System.Diagnostics;

// Entry point
var shell = new Shell();
shell.Run();

/// <summary>
/// A simple POSIX-compliant shell implementation in C#
/// </summary>
public class Shell
{
    private static readonly string[] BuiltinCommands = { "exit", "echo", "type", "pwd", "cd", "help", "clear", "history", "env" };
    private readonly string[] _pathVariables;
    private readonly List<string> _commandHistory = new();
    private bool _running = true;

    public Shell()
    {
        var pathEnv = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        var delimiter = Environment.OSVersion.Platform == PlatformID.Win32NT ? ";" : ":";
        _pathVariables = pathEnv.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
    }

    public void Run()
    {
        while (_running)
        {
            Console.Write("$ ");

            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                continue;

            // Store command in history
            _commandHistory.Add(input);

            var commandParts = ParseCommand(input);
            if (commandParts.Length == 0)
                continue;

            var commandType = GetCommandType(commandParts[0]);

            switch (commandType)
            {
                case CommandType.BuiltinCommand:
                    ExecuteBuiltinCommand(commandParts);
                    break;
                case CommandType.ExternalCommand:
                    ExecuteExternalCommand(commandParts);
                    break;
                default:
                    Console.WriteLine($"{input}: not found");
                    break;
            }
        }
    }

    /// <summary>
    /// Parse command input handling quoted strings
    /// </summary>
    private static string[] ParseCommand(string input)
    {
        var parts = new List<string>();
        var current = string.Empty;
        var inSingleQuote = false;
        var inDoubleQuote = false;

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (c == '\'' && !inDoubleQuote)
            {
                inSingleQuote = !inSingleQuote;
            }
            else if (c == '"' && !inSingleQuote)
            {
                inDoubleQuote = !inDoubleQuote;
            }
            else if (c == ' ' && !inSingleQuote && !inDoubleQuote)
            {
                if (!string.IsNullOrEmpty(current))
                {
                    parts.Add(current);
                    current = string.Empty;
                }
            }
            else
            {
                current += c;
            }
        }

        if (!string.IsNullOrEmpty(current))
        {
            parts.Add(current);
        }

        return parts.ToArray();
    }

    private void ExecuteExternalCommand(string[] commandParts)
    {
        var execPath = GetExecutablePath(commandParts[0]);
        if (execPath == null)
        {
            Console.WriteLine($"{commandParts[0]}: command not found");
            return;
        }

        try
        {
            using var process = new Process();
            process.StartInfo.FileName = execPath;
            process.StartInfo.UseShellExecute = false;
            
            // Add arguments individually to handle spaces and special characters properly
            foreach (var arg in commandParts[1..])
            {
                process.StartInfo.ArgumentList.Add(arg);
            }
            
            process.Start();
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing {commandParts[0]}: {ex.Message}");
        }
    }

    private void ExecuteBuiltinCommand(string[] commandParts)
    {
        switch (commandParts[0])
        {
            case "exit":
                HandleExitCommand(commandParts);
                break;
            case "echo":
                HandleEchoCommand(commandParts);
                break;
            case "type":
                HandleTypeCommand(commandParts);
                break;
            case "pwd":
                HandlePwdCommand();
                break;
            case "cd":
                HandleCdCommand(commandParts);
                break;
            case "help":
                HandleHelpCommand();
                break;
            case "clear":
                HandleClearCommand();
                break;
            case "history":
                HandleHistoryCommand();
                break;
            case "env":
                HandleEnvCommand(commandParts);
                break;
        }
    }

    private void HandleExitCommand(string[] commandParts)
    {
        if (commandParts.Length > 1)
        {
            if (int.TryParse(commandParts[1], out int exitCode))
            {
                _running = false;
                Environment.ExitCode = exitCode;
                return;
            }
        }
        // Default exit with code 0
        _running = false;
        Environment.ExitCode = 0;
    }

    private static void HandleEchoCommand(string[] commandParts)
    {
        if (commandParts.Length > 1)
        {
            Console.WriteLine(string.Join(" ", commandParts[1..]));
        }
        else
        {
            Console.WriteLine();
        }
    }

    private void HandleTypeCommand(string[] commandParts)
    {
        if (commandParts.Length < 2)
        {
            Console.WriteLine("type: missing argument");
            return;
        }

        var targetCommand = commandParts[1];
        var targetCommandType = GetCommandType(targetCommand);

        switch (targetCommandType)
        {
            case CommandType.BuiltinCommand:
                Console.WriteLine($"{targetCommand} is a shell builtin");
                break;
            case CommandType.ExternalCommand:
                Console.WriteLine($"{targetCommand} is {GetExecutablePath(targetCommand)}");
                break;
            case CommandType.NotFound:
                Console.WriteLine($"{targetCommand}: not found");
                break;
        }
    }

    private static void HandlePwdCommand()
    {
        Console.WriteLine(Directory.GetCurrentDirectory());
    }

    private static void HandleCdCommand(string[] commandParts)
    {
        string targetPath;

        if (commandParts.Length < 2 || commandParts[1] == "~")
        {
            targetPath = Environment.GetEnvironmentVariable("HOME") 
                         ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
        else if (commandParts[1] == "-")
        {
            var oldPwd = Environment.GetEnvironmentVariable("OLDPWD");
            if (string.IsNullOrEmpty(oldPwd))
            {
                Console.WriteLine("cd: OLDPWD not set");
                return;
            }
            targetPath = oldPwd;
            Console.WriteLine(targetPath);
        }
        else
        {
            targetPath = commandParts[1];
        }

        try
        {
            var currentDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(targetPath);
            Environment.SetEnvironmentVariable("OLDPWD", currentDir);
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine($"cd: {targetPath}: No such file or directory");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"cd: {ex.Message}");
        }
    }

    private static void HandleHelpCommand()
    {
        Console.WriteLine("SeeSharpShell - A simple POSIX-compliant shell");
        Console.WriteLine();
        Console.WriteLine("Built-in commands:");
        Console.WriteLine("  exit [code]    Exit the shell with optional exit code (default: 0)");
        Console.WriteLine("  echo [args]    Display a line of text");
        Console.WriteLine("  type <cmd>     Display information about command type");
        Console.WriteLine("  pwd            Print the current working directory");
        Console.WriteLine("  cd [dir]       Change the current directory");
        Console.WriteLine("  help           Display this help message");
        Console.WriteLine("  clear          Clear the terminal screen");
        Console.WriteLine("  history        Display command history");
        Console.WriteLine("  env [name]     Display environment variables");
        Console.WriteLine();
        Console.WriteLine("External commands from PATH are also available.");
    }

    private static void HandleClearCommand()
    {
        Console.Clear();
    }

    private void HandleHistoryCommand()
    {
        for (int i = 0; i < _commandHistory.Count; i++)
        {
            Console.WriteLine($"  {i + 1}  {_commandHistory[i]}");
        }
    }

    private static void HandleEnvCommand(string[] commandParts)
    {
        if (commandParts.Length > 1)
        {
            // Display specific environment variable
            var value = Environment.GetEnvironmentVariable(commandParts[1]);
            if (value != null)
            {
                Console.WriteLine($"{commandParts[1]}={value}");
            }
            else
            {
                Console.WriteLine($"env: {commandParts[1]}: not set");
            }
        }
        else
        {
            // Display all environment variables
            var variables = Environment.GetEnvironmentVariables();
            foreach (string key in variables.Keys)
            {
                Console.WriteLine($"{key}={variables[key]}");
            }
        }
    }

    private CommandType GetCommandType(string command)
    {
        if (BuiltinCommands.Contains(command))
            return CommandType.BuiltinCommand;
        if (GetExecutablePath(command) != null)
            return CommandType.ExternalCommand;
        return CommandType.NotFound;
    }

    private string? GetExecutablePath(string command)
    {
        // Check if command is an absolute or relative path
        // Handle both Windows (\) and Unix (/) directory separators
        if (command.Contains(Path.DirectorySeparatorChar) || 
            command.Contains(Path.AltDirectorySeparatorChar))
        {
            return File.Exists(command) ? command : null;
        }

        // Search in PATH directories
        foreach (var path in _pathVariables)
        {
            var fullPath = Path.Combine(path, command);
            if (File.Exists(fullPath))
                return fullPath;
        }
        return null;
    }
}

public enum CommandType 
{ 
    NotFound, 
    BuiltinCommand, 
    ExternalCommand 
}