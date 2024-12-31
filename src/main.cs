using System.Net;
using System.Net.Sockets;

// Uncomment this line to pass the first stage
while (true)
{
    Console.Write("$ ");

    // Wait for user input
    string command = Console.ReadLine();
    string[] commandParse = command.Split(' ');
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

        default:
            Console.WriteLine($"{commandParse[0]}: command not found");
            break;
    }
}