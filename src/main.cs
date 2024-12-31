using System.Net;
using System.Net.Sockets;

// Uncomment this line to pass the first stage
while (true)
{
    Console.Write("$ ");

    // Wait for user input
    var command = Console.ReadLine();

    Console.WriteLine($"{command}: command not found");
}