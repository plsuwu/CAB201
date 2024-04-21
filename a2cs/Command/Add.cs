namespace a2cs;

/// <summary>
/// Logic for parsing and executing `add` command
/// </summary>
public class Add : Command.ICommand {
    private static readonly string[] Orientations = ["north", "east"];
    private static readonly string[] Directions =
        ["north", "east", "south", "west"];

    public enum Obstacle {
        Guard,
        Fence,
        Sensor,
        Camera,
    }

    private Dictionary<string, Func<string[], string>> actions;

    public Add() {
        actions = new Dictionary<string, Func<string[], string>>() {
            { "guard", Guard },
            { "fence", Fence },
            { "sensor", Sensor },
            { "camera", Camera },
        };
    }

    public string Handler(string[] args) {
        if (args.Length < 2)
            return "You need to specify an obstacle type.";

        if (actions.TryGetValue(args[1], out var action))
            return action(args);

        return "Invalid obstacle type.";
    }

    private string Validate(string[] args, Obstacle obstacle) {
        if (!int.TryParse(args[2], out int x) ||
            !int.TryParse(args[3], out int y)) {
            return "Coordinates are not valid integers.";
        }

        if (obstacle != Obstacle.Guard) {

            string cardinal = args[4];
            if (obstacle == Obstacle.Fence &&
                (!Orientations.Contains(cardinal))) {
                return "Orientation must be 'east' or 'north'.";
            }
            if (obstacle == Obstacle.Camera &&
                (!Directions.Contains(cardinal))) {
                return "Direction must be 'north', 'south', 'east' or 'west'.";
            }
            if (obstacle == Obstacle.Sensor &&
                (!double.TryParse(cardinal, out double range) || range <= 0)) {
                return "Range must be a valid positive number.";
            }
            if (obstacle == Obstacle.Fence &&
                (!int.TryParse(args[5], out int length) || length <= 0)) {
                return "Length must be a valid integer greater than 0.";
            }
        }

        return "ok";
    }

    private string Process(string[] args, Obstacle type) {
        var validation = Validate(args, type);
        if (validation != "ok")
            return validation;

        switch (type) {
            case Obstacle.Guard:
                Active.Guard(int.Parse(args[2]), int.Parse(args[3]));
                break;

            case Obstacle.Fence:
                Active.Fence(int.Parse(args[2]), int.Parse(args[3]), args[4],
                             int.Parse(args[5]));
                break;

            case Obstacle.Sensor:
                Active.Sensor(int.Parse(args[2]), int.Parse(args[3]),
                              double.Parse(args[4]));
                break;

            case Obstacle.Camera:
                Active.Camera(int.Parse(args[2]), int.Parse(args[3]), args[4]);
                break;

        }

        return $"Successfully added {args[1]} obstacle.";
    }

    private string Guard(string[] args) {
        if (args.Length != 4)
            return "Incorrect number of arguments.";
        return Process(args, Obstacle.Guard);
    }
    private string Fence(string[] args) {
        if (args.Length != 6)
            return "Incorrect number of arguments.";
        return Process(args, Obstacle.Fence);
    }
    private string Sensor(string[] args) {
        if (args.Length != 5)
            return "Incorrect number of arguments.";
        return Process(args, Obstacle.Sensor);
    }
    private string Camera(string[] args) {
        if (args.Length != 5)
            return "Incorrect number of arguments.";
        return Process(args, Obstacle.Camera);
    }
}
