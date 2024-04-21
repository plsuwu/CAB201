namespace a2cs;

public class Check : Command.ICommand {
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
        if (!int.TryParse(args[1], out int x) ||
            !int.TryParse(args[2], out int y)) {
            return "Coordinates are not valid integers.";
        }

        List<((int, int), char)> fixedObstacles = Active.fixedObstacles;
        if (fixedObstacles.Any(obstacle => obstacle.Item1.Item1 == x &&
                                           obstacle.Item1.Item2 == y)) {
            return "Agent, your location is compromised. Abort mission.";
        }

        List<string> safe = new List<string>();

        foreach (var dir in directions) {
            int xAdjacent = x + dir.Value[0];
            int yAdjacent = y + dir.Value[1];

            // Console.WriteLine($"{xAdjacent}, {yAdjacent}");

            if (!fixedObstacles.Any(obstacle =>
                                        obstacle.Item1.Item1 == xAdjacent &&
                                        obstacle.Item1.Item2 == yAdjacent)) {
                safe.Add(dir.Key);
            }
        }

        if (safe.Count < 1) {
            return "You cannot safely move in any direction. Abort mission.";
        }

        safe.Insert(0, "You can safely take any of the following directions:");
        safe.Add("");

        return string.Join("\n", safe);
    }
}
