namespace a2cs;

// i haven't made my way around to XML annotations + i have a little refactoring to do anyway;
// so the file structure/program flow isnt set in STONE, but should indicate a thought process
// of some description.
//
// i am unsure if i will have the energy to fully implement the logic in `Command/Path.cs`
// before 12 tonight -its a little messy anyway - but the other commands are functioning how they
// should (according to gradescope).

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
