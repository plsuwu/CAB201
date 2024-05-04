namespace a2cs;

// this is a big ugly class that needs refactoring and condensing
public class Add : Command.ICommand {

    private const int __MIN_ARG_LENGTH_ANY = 2;
    private const int __MIN_ARG_LENGTH_GUARD = 4;
    private const int __MIN_ARG_LENGTH_FENCE = 6;
    private const int __MIN_ARG_LENGTH_SENSOR = 5;
    private const int __MIN_ARG_LENGTH_CAMERA = 5;

    private const int __ARG_OBSTACLE_NAME = 1;
    private const int __ARG_OBSTACLE_OPT = 4;
    private const int __ARG_X_COORDINATE = 2;
    private const int __ARG_Y_COORDINATE = 3;
    private const int __ARG_FENCE_LENGTH = 5;
    private const int __ARG_FENCE_ORIEN = 4;
    // private const int __ARG_CAMERA_DIR = 4;
    // private const int __ARG_SENSOR_RANGE = 4;

    private Dictionary<string, Func<string[], string>> actions;
    private static readonly string[] Orientations = ["north", "east"];
    private static readonly string[] Directions = ["north", "east", "south", "west"];

    public enum Type {
        Guard,
        Fence,
        Sensor,
        Camera,
    }

    public Add() {
        actions = new Dictionary<string, Func<string[], string>>() {
            { "guard", ProcessGuard },
            { "fence", ProcessFence },
            { "sensor", ProcessSensor },
            { "camera", ProcessCamera },
        };
    }

    public string Handler(string[] args) {
        if (args.Length < __MIN_ARG_LENGTH_ANY)
            return "You need to specify an obstacle type.";

        if (actions.TryGetValue(args[__ARG_OBSTACLE_NAME], out var action))
            return action(args);

        return "Invalid obstacle type.";
    }

    private string Validate(string[] args, Type obstacle) {
        if (!int.TryParse(args[__ARG_X_COORDINATE], out int x) ||
            !int.TryParse(args[__ARG_Y_COORDINATE], out int y)) {
            return "Coordinates are not valid integers.";
        }

        if (obstacle != Type.Guard) {
            string option = args[__ARG_OBSTACLE_OPT];

            if (obstacle == Type.Fence && (!Orientations.Contains(option))) {
                return "Orientation must be 'east' or 'north'.";
            }
            if (obstacle == Type.Camera && (!Directions.Contains(option))) {
                return "Direction must be 'north', 'south', 'east' or 'west'.";
            }
            if (obstacle == Type.Sensor &&
                (!double.TryParse(option, out double range) || range <= 0)) {
                return "Range must be a valid positive number.";
            }
            if (obstacle == Type.Fence &&
                (!int.TryParse(args[__ARG_FENCE_LENGTH], out int length) ||
                 length <= 0)) {
                return "Length must be a valid integer greater than 0.";
            }
        }

        return "ok";
    }

    private string Process(string[] args, Type type) {
        var validation = Validate(args, type);
        if (validation != "ok")
            return validation;

        switch (type) {

        case Type.Guard:
            Guard.Add(int.Parse(args[__ARG_X_COORDINATE]),
                      int.Parse(args[__ARG_Y_COORDINATE]));
            break;

        case Type.Fence:
            Fence.Add(int.Parse(args[__ARG_X_COORDINATE]),
                      int.Parse(args[__ARG_Y_COORDINATE]), args[__ARG_OBSTACLE_OPT],
                      int.Parse(args[__ARG_FENCE_LENGTH]));
            break;

        case Type.Sensor:
            Sensor.Add(int.Parse(args[__ARG_X_COORDINATE]),
                       int.Parse(args[__ARG_Y_COORDINATE]),
                       double.Parse(args[__ARG_OBSTACLE_OPT]));
            break;

        case Type.Camera:
            Camera.Add(int.Parse(args[__ARG_X_COORDINATE]),
                       int.Parse(args[__ARG_Y_COORDINATE]), args[__ARG_OBSTACLE_OPT]);
            break;

        }

        return $"Successfully added {args[__ARG_OBSTACLE_NAME]} obstacle.";
    }

    private string ProcessGuard(string[] args) {
        if (args.Length != __MIN_ARG_LENGTH_GUARD)
            return "Incorrect number of arguments.";

        return Process(args, Type.Guard);
    }
    private string ProcessFence(string[] args) {
        if (args.Length != __MIN_ARG_LENGTH_FENCE)
            return "Incorrect number of arguments.";

        return Process(args, Type.Fence);
    }
    private string ProcessSensor(string[] args) {
        if (args.Length != __MIN_ARG_LENGTH_SENSOR)
            return "Incorrect number of arguments.";

        return Process(args, Type.Sensor);
    }
    private string ProcessCamera(string[] args) {
        if (args.Length != __MIN_ARG_LENGTH_CAMERA)
            return "Incorrect number of arguments.";

        return Process(args, Type.Camera);
    }
}
