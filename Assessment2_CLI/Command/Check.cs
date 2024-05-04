namespace a2cs;

public class Check : Command.ICommand {

    private const int __MIN_SAFE_CELLS = 1;
    private const int __DIR_X_INDEX = 0;
    private const int __DIR_Y_INDEX = 1;
    private const int __START_CELL_INDEX = 0;
    private const int __SIZE_CELL_INDEX = 1;

    private static readonly Dictionary<string, int[]> directions =
        new Dictionary<string, int[]> {
            { "North", new int[] { 0, 1 } },
            { "South", new int[] { 0, -1 } },
            { "East", new int[] { 1, 0 } },
            { "West", new int[] { -1, 0 } },
        };

    public string Handler(string[] args) {
        if (args.Length != 3) {
            return "Incorrect number of arguments.";
        }
        if (!int.TryParse(args[1], out int x) || !int.TryParse(args[2], out int y)) {
            return "Coordinates are not valid integers.";
        }

        int[] startCell = { x - 1, y - 1 };
        int[] sizeOfGrid = { x + 1, y + 1 };

        Grid grid = new Grid();
        List<Grid.Cell> coordinates = grid.Build(startCell, sizeOfGrid);

        Render render = new Render();
        string[] map =
            render.Map(coordinates[__START_CELL_INDEX], coordinates[__SIZE_CELL_INDEX]);

        List<((int, int), char, string?)> fixedObstacles = Active.fixedObstacles;
        if (fixedObstacles.Any(obstacle => obstacle.Item1.Item1 == x &&
                                           obstacle.Item1.Item2 == y)) {
            return "Agent, your location is compromised. Abort mission.";
        }

        List<string> safe = new List<string>();

        foreach (var dir in directions) {
            int adjacentXCell = x + dir.Value[__DIR_X_INDEX];
            int adjacentYCell = y + dir.Value[__DIR_Y_INDEX];

            if (!fixedObstacles.Any(obstacle => obstacle.Item1.Item1 == adjacentXCell &&
                                                obstacle.Item1.Item2 == adjacentYCell)) {
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
