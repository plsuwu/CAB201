// #pragma warning disable CS8602  // Dereference of a possibly null reference
namespace a2cs;

/// <summary>
/// Main runtime manager class. Used to parse user commands into commands or errors and return associated feedback on
/// user input to stdout.
/// </summary>
public class Command {
    private const int       __ARG_COMMAND_NAME  = 0;
    private const string    __COMMAND_PROMPT    = "Enter command:";
    private const string    __ARG_EXIT          = "exit";
    private const string    __ARG_HELP          = "help";
    private const string    __ARG_ADD           = "add";
    private const string    __ARG_CHECK         = "check";
    private const string    __ARG_MAP           = "map";
    private const string    __ARG_PATH          = "path";

    /// <summary>
    /// <c>Command</c> interface - provides a common return type to each command object through the <c>Handler</c> method.
    /// </summary>
    public interface ICommand {
        /// <summary>
        /// <c>Handler</c> interface for each command object to funnel output through.
        /// <example>
        /// This interface should wrap the main method of each <c>Command</c> object such that the returned value is a string:
        /// <code>
        /// class SomeCommand : ICommand {
        ///     public string Handler(string[] args) {
        ///
        ///         // returns the result or error of a command as a string:
        ///         if (some_invalid_arg) {
        ///             return "I will print an error...";
        ///         } else {
        ///             return "I will print a result!";
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        /// </summary>
        string Handler(string[] args);
    }

    /// <summary>
    /// Return invalid <c>Command</c> input with the <c>ICommand</c> interface.
    /// If the first item of the <c>args</c> array does not match a known command, then it cannot be
    /// used to create a command object and is default-matched to the <c>InputError</c> object below,
    /// which will return information describing the issue.
    /// </summary>
    private class InputError : ICommand {
        public string Handler(string[] args) {
            return $"Invalid option: {args[__ARG_COMMAND_NAME]}.\nType 'help' to see a list of commands.";
        }
    }

    /// <summary>
    /// Nullable string field to hold user input.
    /// </summary>
    private string? input;

    /// <summary>
    /// Runs the main loop of the CLI, providing an interface for the user's input to communicate with the logic
    /// of an associated command.
    /// </summary>
    public void InputLoop() {
        do {
            Console.WriteLine(__COMMAND_PROMPT);
            input = (Console.ReadLine() ?? string.Empty).Trim();

            // base cases for very simple commands that dont require much in the way of computation.
            // input -> "exit"
            if (input == __ARG_EXIT) {
                break;
            }
            // input -> "help"
            if (input == __ARG_HELP) {
                Console.WriteLine(Help.__HELP);
                continue;
            }

            // split input into lowercase array of strings - 'args' - separated by whitespace
            string[] args = input.ToLower().Split(" ");

            // pattern matching with a switch statement to create a command respective to the
            // string in `arg[0]`.
            // each object will handle its own logic and return an error or result as a string.
            ICommand cmd = args[__ARG_COMMAND_NAME] switch {
                __ARG_ADD => new Add(),
                __ARG_CHECK => new Check(),
                __ARG_MAP => new Map(),
                __ARG_PATH => new Path(),
                _ => new InputError(), // default case: creates a new `InputError` object if the command can't be matched
            };

            // format the command's returned output such that it is displayed neatly, in an expected format
            var parsed = cmd.Handler(args);
            if (parsed.Contains("\n")) {
                string[] lines = parsed.Split("\n");
                foreach (string line in lines) {
                    Console.WriteLine(line);
                }
            } else {
                Console.WriteLine(parsed);
            }

        } while (true); // loop until death
    }
}
