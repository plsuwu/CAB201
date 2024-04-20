namespace a2cs;

/// <summary>
/// Logic for parsing and executing `add` command
/// </summary>
public class Add : Command.ICommand {
    public enum Obstacle {
        Guard = 0,
        Fence = 1,
        Sensor = 2,
        Camera = 3,
    }

    public string Handler(string[] args) {
        if (args.Length < 2) {
            return "You need to specify an obstacle type.";
        }

        return args[1] switch {
            "guard" => Guard(args),
            "fence" => Fence(args),
            "sensor" => Sensor(args),
            // "camera" => Camera(args),
            _ => "Invalid obstacle type.",
        };
    }

    public string Validate(string[] args, Obstacle obstacle) {
        if (!(int.TryParse(args[2], out int x) ||
              !(int.TryParse(args[3], out int y)))) {
            return "Coordinates are not valid integers";
        }
        if ((obstacle == Obstacle.Fence) &&
            (args[4] != "east" && args[4] != "north")) {
            return "Orientation must be 'east' or 'north'.";
        }
        if ((obstacle == Obstacle.Camera) &&
            (args[4] != "east" && args[4] != "north" && args[4] != "south" &&
             args[4] != "west")) {
            return "Direction must be 'north', 'south', 'east' or 'west'.";
        }
        if (obstacle == Obstacle.Sensor &&
            (!(double.TryParse(args[4], out double range) ||
               range <= 0))) {
            return "Range must be a valid positive number.";
        }
        if (obstacle == Obstacle.Fence &&
            (!(int.TryParse(args[5], out int length) ||
               length <= 0))) {
            return "Length must be a valid integer greater than 0.";
        }

        return "ok";
    }

    public string Guard(string[] args) {
        if (args.Length != 4) {
            return "Incorrect number of arguments.";
        }

        var valid = Validate(args, Obstacle.Guard);
        if (valid == "ok") {
            Active.Guard(int.Parse(args[2]), int.Parse(args[3]));
            return $"Successfully added {args[1]} obstacle.";
        }

        return valid;
    }

    public string Fence(string[] args) {
        if (args.Length != 6) {
            return "Incorrect number of arguments.";
        }
        var valid = Validate(args, Obstacle.Fence);
        if (valid == "ok") {
            Active.Fence(int.Parse(args[2]), int.Parse(args[3]), args[4], int.Parse(args[5]));
            return $"Successfully added {args[1]} obstacle.";
        }

        return valid;
    }

    public string Sensor(string[] args) {
        if (args.Length != 5) {
            return "Incorrect number of arguments.";
        }
        var valid = Validate (args, Obstacle.Sensor);
        if (valid == "ok") {
            Active.Sensor(int.Parse(args[2]), int.Parse(args[3]), double.Parse(args[4]));
            return $"Successfully added {args[1]} obstacle.";
        }

        return valid;
    }
}
