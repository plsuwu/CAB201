#pragma warning disable CS8602

namespace a2cs;

/// <summary>
/// Main runtime manager to parse commands and display related feedback to user input.
/// </summary>
/// <remarks>
/// Facilitates `stdio` access between any given command's logic and the `Main()` function
/// via the `<i>ICommand</i>` interface, which provides the `Handler()` function to funnel
/// IO to and from any new Command object that is created. Each Command object handles its own logic
/// internally, but will return via this interface as either an error or result string, which
/// can be used by <c>Command</c> as part of a `stdio` operation.
/// </remarks>
public class Command {

    public interface ICommand {
        string Handler(string[] args);
    }

    private class Error : ICommand {
        public string Handler(string[] args) {
            return $"Invalid option: {args[0]}.";
        }
    }

    private string? input;

    public void InputLoop() {
        do {
            Console.WriteLine(Output.Info.Prompt);
            input = Console.ReadLine().Trim();

            if (input == "exit") {
                break;
            }
            if (input == "help") {
                Console.WriteLine(Output.Info.Help);
                continue;
            }

            string[] args = input.ToLower().Split(" ");

            ICommand cmd = args[0] switch {
                "add" => new Add(),
                "check" => new Check(),
                "map" => new Map(),
                "path" => new Path(),
                _ => new Error(),
            };

            var parsed = cmd.Handler(args);

            if (parsed.Contains("\n")) {
                string[] lines = parsed.Split("\n");
                foreach (string line in lines) {
                    Console.WriteLine(line);
                }
            } else {
                Console.WriteLine(parsed);
            }

        } while (true);
    }
}
