namespace a2cs;

/// <summary>
/// Logic for parsing and executing `add` command
/// </summary>
public class Add : Command.ICommand {

    public string Handler(string[] args) {
        if (args.Length != 2) {
           return "You need to specify an obstacle type.";
        }

        return args[1] switch {
            "guard" => Guard(args),
            // "fence" => Fence(args),
            // "sensor" => Sensor(args),
            // "camera" => Camera(args),
            _ => "Invalid obstacle type.",
        };

    }

    public string Guard(string[] args) {
        if (args.Length < 4) {
            return "Incorrect number of arguments.";
        }
        if (!(int.TryParse(args[2], out int xPos)) || !(int.TryParse(args[3], out int yPos))) {
            return "Coordinates are not valid integers.";
        }

        return $"Add guard => (from '{args[1]}') in position {xPos}, {yPos}";
    }
}
