namespace a2cs;

public class Check : Command.ICommand {

    private const int __DIR_X_INDEX = 0;
    private const int __DIR_Y_INDEX = 1;
    private const int __START_CELL_INDEX = 0;
    private const int __SIZE_CELL_INDEX = 1;
    private const int __MIN_SAFE_CELLS = 1;
    private const int __ARG_MIN_LENGTH = 3;
    private const int __ARG_X_COORDINATE = 1;
    private const int __ARG_Y_COORDINATE = 2;

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

        int[] startCell = { x - 1, y - 1 };
        int[] sizeOfGrid = { x + 1, y + 1 };

        Grid grid = new Grid();
        List<Grid.Cell> coordinates = grid.Build(startCell, sizeOfGrid);

        Render render = new Render();
        string[] map =
            render.Map(coordinates[__START_CELL_INDEX], coordinates[__SIZE_CELL_INDEX]);

        if (Obstacle.blockedCells.TryGetValue((x, y), out var obstacle)) {
            return "Agent, your location is compromised. Abort mission.";
        }

        List<string> safe = new List<string>();

        foreach (var dir in directions) {
            int adjacentXCell = x + dir.Value[__DIR_X_INDEX];
            int adjacentYCell = y + dir.Value[__DIR_Y_INDEX];

            if (!Obstacle.blockedCells.TryGetValue((adjacentXCell, adjacentYCell),
                                                   out var blocked)) {
                safe.Add(dir.Key);
            }
        }

        if (safe.Count < __MIN_SAFE_CELLS) {
            return "You cannot safely move in any direction. Abort mission.";
        }

        safe.Insert(0, "You can safely take any of the following directions:");
        safe.Add("");

        return string.Join("\n", safe);
    }
}
