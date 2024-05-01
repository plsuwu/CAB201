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

        var coordinates = typeof(Map.Coordinates).GetFields();

        Map.Coordinates southwest = new Map.Coordinates();
        Map.Coordinates size = new Map.Coordinates();

        string[] rawCoordinates = args.Skip(1).ToArray();
        int[] parsedCoordinates = new int[4];

        try {
            for (int i = 0; i < rawCoordinates.Length; ++i) {
                parsedCoordinates[i] = int.Parse(rawCoordinates[i]);
            }
        } catch (Exception e) {
            if (e is FormatException || e is ArgumentException) {
                return "Coordinates are not valid integers.";
            }
        }

        // (i < 2) where | 2 == args.Skip(1).Length
        for (int i = 0; i < 1; ++i) {
            coordinates[i].SetValue(southwest, int.Parse(rawCoordinates[i]));
            coordinates[i].SetValue(size, int.Parse(rawCoordinates[(i)]));
        }

        Render render = new Render();

        // render method only parsing a 1x1 square here; does not take into account
        // the overall 3x3 checked section below.
        string[] map = render.Map(southwest, size);

        List<((int, int), char, string?)> fixedObstacles = Active.fixedObstacles;
        if (fixedObstacles.Any(obstacle => obstacle.Item1.Item1 == x &&
                                           obstacle.Item1.Item2 == y)) {
            return "Agent, your location is compromised. Abort mission.";
        }

        List<string> safe = new List<string>();

        foreach (var dir in directions) {
            int xAdjacent = x + dir.Value[0];
            int yAdjacent = y + dir.Value[1];

            if (!fixedObstacles.Any(obstacle =>
                                        obstacle.Item1.Item1 == xAdjacent &&
                                        obstacle.Item1.Item2 == yAdjacent)) {
                // foreach (var val in fixedObstacles) {
                //     Console.WriteLine("({0}, {1}), {2}, {3}", val.Item1.Item1, val.Item1.Item2, val.Item2, val.Item3);
                // }
                safe.Add(dir.Key);
            }
        }

        // foreach (var s in safe) {
        //     Console.WriteLine("{0}", s);
        // }

        if (safe.Count < 1) {
            return "You cannot safely move in any direction. Abort mission.";
        }

        safe.Insert(0, "You can safely take any of the following directions:");
        safe.Add("");

        return string.Join("\n", safe);
    }
}
