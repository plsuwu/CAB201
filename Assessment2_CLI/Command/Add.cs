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

    private const string __COORDS_INVALID_INTS = "Coordinates are not valid integers.";
    private const string __INVALID_ORIENTATION = "Orientation must be 'east' or 'north'.";
    private const string __INVALID_DIRECTION = "Direction must be 'north', 'south', 'east' or 'west'.";
    private const string __INVALID_RANGE = "Range must be a valid positive number.";
    private const string __INVALID_LENGTH = "Length must be a valid integer greater than 0.";
    private const string __NO_OBSTACLE_TYPE = "You need to specify an obstacle type.";
    private const string __INVALID_OBSTACLE_TYPE = "Invalid obstacle type.";
    private const string __INCORRECT_NUM_ARGS = "Incorrect number of arguments.";

    private Dictionary<string, Func<string[], string>> actions;
    private static readonly string[] Orientations = ["north", "east"];
    private static readonly string[] Directions = ["north", "east", "south", "west"];

    /// <summary>
    /// enum for possible obstacle types
    /// </summary>
    public enum Type {
        Guard,
        Fence,
        Sensor,
        Camera,
    }

    /// <summary>
    /// maps the obstacle name to its corresponding processor function
    /// </summary>
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
            return __NO_OBSTACLE_TYPE;

        if (actions.TryGetValue(args[__ARG_OBSTACLE_NAME], out var action))
            return action(args);

        return __INVALID_OBSTACLE_TYPE;
    }

    /// <summary>
    /// validation function to ensure arguements are valid for the given obstacle type
    /// </summary>
    /// <param name="args">user args from stdin</param>
    /// <param name="obstacle">obstacle type from the "Type" enum</param>
    private string Validate(string[] args, Type obstacle) {
        // check that coordinates are valid numbers
        if (!int.TryParse(args[__ARG_X_COORDINATE], out int x) ||
            !int.TryParse(args[__ARG_Y_COORDINATE], out int y)) {
            return __COORDS_INVALID_INTS;
        }

        // a guard only uses `x, y` coordinates whereas other obstacles are more complex; we can disregard the
        // guard obstacle here
        if (obstacle != Type.Guard) {
            string option = args[__ARG_OBSTACLE_OPT];

            // check that a fence instance is being instantiated with a valid orientation option
            if (obstacle == Type.Fence && (!Orientations.Contains(option))) {
                return __INVALID_ORIENTATION;
            }
            // check that a camera instance is being instantiated with a valid direction option
            if (obstacle == Type.Camera && (!Directions.Contains(option))) {
                return __INVALID_DIRECTION;
            }
            // check that a sensor instance is being instantiated with a valid range value
            if (obstacle == Type.Sensor &&
                (!double.TryParse(option, out double range) || range <= 0)) {
                return __INVALID_RANGE;
            }
            // check that a fence instance is being instantiated with a valid numerical length value
            if (obstacle == Type.Fence &&
                (!int.TryParse(args[__ARG_FENCE_LENGTH], out int length) ||
                 length <= 0)) {
                return __INVALID_LENGTH;
            }
        }

        return "ok"; // return a confirmation string if all checks pass
    }

    /// <summary>
    /// passes necessary args on to the validation function, and runs the relevant <c>[Obstacle].Add</c> handler in the <c>Obstacle</c> manager
    /// </summary>
    /// <param name="args">arguments passed alongside the obstacle name</param>
    /// <param name="type">the obstacle type/name</param>
    private string Process(string[] args, Type type) {

        // validator for misc arguments (i.e, non-coordinate args) passed along with an obstacle and its coordinates
        var validation = Validate(args, type);

        // if we dont recieve the confirmation string when `Validate` returns, we return
        // that value immediately as it is an error
        if (validation != "ok")
            return validation;

        // pattern match the obstacle type to its Adder function to push the obstacle and its location and any relevant options into
        // the obstacles dictionary
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

        // return the success message
        return $"Successfully added {args[__ARG_OBSTACLE_NAME]} obstacle.";
    }

    // some preliminary validation; if we have the correct number of args for that obstacle at this point, then we can process it immediately
    // using the above `Process` function
    /// <summary>
    /// process a guard obstacle
    /// </summary>
    /// <param name="args">array of arguments passed alongside this obstacle</param>
    private string ProcessGuard(string[] args) {
        if (args.Length != __MIN_ARG_LENGTH_GUARD)
            return __INCORRECT_NUM_ARGS;

        return Process(args, Type.Guard);
    }

    /// <summary>
    /// process a fence obstacle
    /// </summary>
    /// <param name="args">array of arguments passed alongside this obstacle</param>
    private string ProcessFence(string[] args) {
        if (args.Length != __MIN_ARG_LENGTH_FENCE)
            return __INCORRECT_NUM_ARGS;

        return Process(args, Type.Fence);
    }

    /// <summary>
    /// process a sensor obstacle
    /// </summary>
    /// <param name="args">array of arguments passed alongside this obstacle</param>
    private string ProcessSensor(string[] args) {
        if (args.Length != __MIN_ARG_LENGTH_SENSOR)
            return __INCORRECT_NUM_ARGS;

        return Process(args, Type.Sensor);
    }

    /// <summary>
    /// process a camera obstacle
    /// </summary>
    /// <param name="args">array of arguments passed alongside this obstacle</param>
    private string ProcessCamera(string[] args) {
        if (args.Length != __MIN_ARG_LENGTH_CAMERA)
            return __INCORRECT_NUM_ARGS;

        return Process(args, Type.Camera);
    }
}
