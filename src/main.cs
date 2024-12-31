using System.Net;
using System.Net.Sockets;
using System.Collections;

// Uncomment this line to pass the first stage
while (true)
{
    Console.Write("$ ");

    // Wait for user input
    string command = Console.ReadLine();
    string[] commandParse = command.Split(' ');
    string[] builtinCommands = { "exit", "echo", "type" };
    var getPathVariable = Environment.GetEnvironmentVariable("PATH");
    var pathVariables = getPathVariable.Split(':');

    switch (commandParse[0])
    {
        case "exit":
            if (commandParse.Length > 1 && commandParse[1] == "0")
                return;
            Console.WriteLine($"{commandParse[0]}: command needs more argument to exit");
            break;

        case "echo":
            if (commandParse.Length > 1)
                Console.WriteLine(string.Join(" ", commandParse[1..]));
            else
                Console.WriteLine($"{commandParse[0]}: command needs more argument to execute");
            break;

        case "type":
            // search for the command in the PATH
            if (commandParse.Length > 1)
            {
                bool found = false;
                foreach (var path in pathVariables)
                {
                    string fullPath = Path.Combine(path, commandParse[1]);
                    if (File.Exists(fullPath))
                    {
                        Console.WriteLine($"{commandParse[1]} is {fullPath}");
                        found = true;
                        break;
                    }
                }
            }
            break;

        default:
            Console.WriteLine($"{commandParse[0]}: command not found");
            break;
    }
}