using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Diagnostics;

string[] builtinCommands = { "exit", "echo", "type", "pwd" };
// Uncomment this line to pass the first stage
var getPathVariables = Environment.GetEnvironmentVariable("PATH");
var delimiter = ":";
// check if the system is windows and set the delimiter to ';'
if (Environment.OSVersion.Platform == PlatformID.Win32NT)
    delimiter = ";";
var pathVariables = getPathVariables.Split(':');

bool run = true;

while (run)
{
    Console.Write("$ ");

    // Wait for user input
    string command = Console.ReadLine();
    string[] commandParse = command.Split(' ');
    CommandType commandType = GetCommandType(commandParse[0]);
    if (commandType == CommandType.BuiltinCommand)
    {
        ExecuteBuiltinCommands(commandParse);
    }
    else if (commandType == CommandType.ExternalCommand)
    {
        ExecuteExternalCommands(commandParse);
    }
    else
    {
        Console.WriteLine($"{command}: not found");
    }
}

void ExecuteExternalCommands(string[] commandParse)
{
    var exec = GetExecutible(commandParse[0]);
    var Process = new Process();
    Process.StartInfo.FileName = exec;
    Process.StartInfo.Arguments = string.Join(" ", commandParse[1..]);
    Process.Start();
    Process.WaitForExit();
    return;
}

void ExecuteBuiltinCommands(string[] commandParse)
{
    switch (commandParse[0])
    {
        case "exit":
            if (commandParse.Length > 1 && commandParse[1] == "0")
            {
                run = false;
                return;
            }
            Console.WriteLine($"{commandParse[0]}: command needs more argument to exit");
            break;

        case "echo":
            if (commandParse.Length > 1)
                Console.WriteLine(string.Join(" ", commandParse[1..]));
            else
                Console.WriteLine($"{commandParse[0]}: command needs more argument to execute");
            break;

        case "type":
            if (commandParse.Length > 1)
            {
                var CommandType = GetCommandType(commandParse[1]);
                switch (CommandType)
                {
                    case CommandType.BuiltinCommand:
                        Console.WriteLine($"{commandParse[1]} is a shell builtin");
                        break;
                    case CommandType.ExternalCommand:
                        Console.WriteLine($"{commandParse[1]} is {GetExecutible(commandParse[1])}");
                        break;
                    case CommandType.NotFound:
                        Console.WriteLine($"{commandParse[1]}: not found");
                        break;
                }
                break;
            }
            Console.WriteLine($"{commandParse[0]}: command needs more argument to execute");
            break;
    }
}

CommandType GetCommandType(string command)
{
    if (builtinCommands.Contains(command))
        return CommandType.BuiltinCommand;
    else if (GetExecutible(command) != null)
        return CommandType.ExternalCommand;
    else return CommandType.NotFound;
}

string? GetExecutible(string command)
{
    foreach (var path in pathVariables)
    {
        string fullPath = Path.Combine(path, command);
        if (File.Exists(fullPath))
            return fullPath;
    }
    return null;
}

enum CommandType { NotFound, BuiltinCommand, ExternalCommand }