namespace a2cs;

// i haven't made my way around to XML annotations + i have a little refactoring to do anyway;
// so the file structure/program flow isnt set in STONE, but should indicate my general thought process
// to some degree.
//

class Program {
    private const string __WELCOME =
        "Welcome to the Threat-o-tron 9000 Obstacle Avoidance System.";
    private const string __EXIT = "Thank you for using the Threat-o-tron 9000.";

    static void Main(string[] args) {
        Console.WriteLine(__WELCOME);
        Console.WriteLine(Help.__HELP);

        var command = new Command();
        command.InputLoop();

        Console.WriteLine(__EXIT);
    }
}
