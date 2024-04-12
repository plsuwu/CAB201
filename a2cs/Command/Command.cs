#pragma warning disable CS8602

namespace a2cs;

/// <summary>
/// Class for interactive input loop handling and command parsing objects
/// </summary>
class Command {

    public string? input;

    // interface event loop
    public void InputLoop() {
        Console.WriteLine(Output.Welcome);
        Console.WriteLine(Output.Help);

        // loop while handling input
        while (true) {
            Console.WriteLine(Output.Prompt);
            input = Console.ReadLine().Trim();

            // guard
            if (input == "exit") {
                Console.WriteLine(Output.Exit);
                break;
            }

            // split into lowercase array of args delimited by space char
            string[] args = input.ToLower().Split(" ");
        }
    }
}
