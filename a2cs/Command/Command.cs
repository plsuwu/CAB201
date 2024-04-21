#pragma warning disable CS8602

namespace a2cs;

/// <summary>
/// Main loop for handling user input:
/// Parses a command into an array of arguments,
/// Calls the relevant command's handler using pattern matching, passing the array of arguments as a
/// parameter.
/// </summary>
public class Command {

    public interface ICommand {
        string Handler(string[] args);
    }

    public class Error : ICommand {
        public string Handler(string[] args) {
            return $"Invalid option: {args[0]}.";
        }
    }

    public string? input;

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
            // Console.WriteLine("in: [{0}]", string.Join(", ", args));

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
