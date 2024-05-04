#pragma warning disable CS8602
// haven't made my way around to XML annotations,
// also much tidying up to do,

namespace a2cs;

/// <summary>
/// Main runtime manager to parse commands and return associated feedback on
/// user input to stdout.
/// </summary>
public class Command {
    private const string __PROMPT = "Enter command:";

    public interface ICommand {
        string Handler(string[] args);
    }

    private class Error : ICommand {
        public string Handler(string[] args) {
            return $"Invalid option: {args[0]}.\nType 'help' to see a list of commands.";
        }
    }

    private string? input;

    public void InputLoop() {
        do {
            Console.WriteLine(__PROMPT);
            input = Console.ReadLine().Trim();

            if (input == "exit") {
                break;
            }
            if (input == "help") {
                Console.WriteLine(Help.__HELP);
                continue;
            }

            string[] args = input.ToLower().Split(" ");

            ICommand cmd = args[0] switch {
                "add" => new Add(), "check" => new Check(),
                "map" => new Map(), "path" => new Path(),
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
