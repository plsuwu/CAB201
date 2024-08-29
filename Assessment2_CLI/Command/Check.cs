namespace a2cs;

/// <summary>
/// performs a check for obstacles at and around a given cell
/// </summary>
public class Check : Command.ICommand {

    private const int __DIR_X_INDEX = 0;
    private const int __DIR_Y_INDEX = 1;
    private const int __START_CELL_INDEX = 0;
    private const int __SIZE_CELL_INDEX = 1;
    private const int __MIN_SAFE_CELLS = 1;
    private const int __ARG_MIN_LENGTH = 3;
    private const int __ARG_X_COORDINATE = 1;
    private const int __ARG_Y_COORDINATE = 2;

    private const string __LOCATION_COMPROMISED =
        "Agent, your location is compromised. Abort mission.";

    private static readonly Dictionary<string, int[]> directions =
        new Dictionary<string, int[]> {
            { "North", new int[] { 0, 1 } },
            { "South", new int[] { 0, -1 } },
            { "East", new int[] { 1, 0 } },
            { "West", new int[] { -1, 0 } },
        };

    public string Handler(string[] args) {
        if (args.Length != __ARG_MIN_LENGTH)
            return "Incorrect number of arguments.";

        if (!int.TryParse(args[__ARG_X_COORDINATE], out int x) ||
            !int.TryParse(args[__ARG_Y_COORDINATE], out int y))
            return "Coordinates are not valid integers.";

        // expand the 1x1 cell passed to the command by 1 on all sides so that the surrounding cells
        // can also be checked for obstacles
        int[] startCell = { x - 1, y - 1 };
        int[] sizeOfGrid = { x + 1, y + 1 };

        Grid grid = new Grid();
        List<Grid.Cell> coordinates = grid.Build(startCell, sizeOfGrid);

        // render the area to determine the ethereal camera obstacle's blocking cells
        Render render = new Render();
        string[] map =
            render.Map(coordinates[__START_CELL_INDEX], coordinates[__SIZE_CELL_INDEX]);

        // return the result if the given cell is a key in the obstacles dictionary
        if (Obstacle.blockedCells.TryGetValue((x, y), out var obstacle)) {
            return __LOCATION_COMPROMISED;
        }

        // instantiate a list of safe directions
        List<string> safe = new List<string>();

        foreach (var dir in directions) {
            // create the adjacent cell via an iterator over coordinate modifiers
            int adjacentXCell = x + dir.Value[__DIR_X_INDEX];
            int adjacentYCell = y + dir.Value[__DIR_Y_INDEX];

            // if the adjacent cell is NOT in the dictionary of obstacles, add the MODIFIER used to the list of safe direction
            if (!Obstacle.blockedCells.TryGetValue((adjacentXCell, adjacentYCell), out var blocked)) {
                safe.Add(dir.Key);
            }
        }

        // if the number of safe cells are less than 1, return the status
        if (safe.Count < __MIN_SAFE_CELLS) {
            return "You cannot safely move in any direction. Abort mission.";
        }

        // if there are one or more safe directions, insert the message and return the concatenated string to the caller
        safe.Insert(0, "You can safely take any of the following directions:");
        safe.Add("");

        return string.Join("\n", safe);
    }
}
