namespace a2cs;

class Program {
    static void Main(string[] args) {
        Console.WriteLine(Output.Info.Welcome);
        Console.WriteLine(Output.Info.Help);

        var command = new Command();
        command.InputLoop();

        Console.WriteLine(Output.Info.Exit);
    }
}
