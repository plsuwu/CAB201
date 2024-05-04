namespace a2cs;

class Program {
    private const string __WELCOME = "Welcome to the Threat-o-tron 9000 Obstacle Avoidance System.";
    private const string __EXIT =
            "Thank you for using the Threat-o-tron 9000.";

    static void Main(string[] args) {
        Console.WriteLine(__WELCOME);
        Console.WriteLine(Help.__HELP);

        var command = new Command();
        command.InputLoop();

        Console.WriteLine(__EXIT);
    }
}
