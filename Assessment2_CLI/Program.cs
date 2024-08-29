namespace a2cs;

/// <summary>
/// passes IO to and from the command handler
/// </summary>
class Program {
    private const string __WELCOME =
        "Welcome to the Threat-o-tron 9000 Obstacle Avoidance System.";
    private const string __EXIT = "Thank you for using the Threat-o-tron 9000.";

    static void Main(string[] args) {
        Console.WriteLine(__WELCOME);
        Console.WriteLine(Help.__HELP);

        var command = new Command();
        command.InputLoop(); // loop until SIGINT or 'exit' command is issued

        Console.WriteLine(__EXIT);
    }
}
