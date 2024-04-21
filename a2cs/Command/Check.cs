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

        List<((int, int), char, string?)> fixedObstacles = Active.fixedObstacles;
        if (fixedObstacles.Any(obstacle => obstacle.Item2 == 'C')) {
            var cameras =
                fixedObstacles.FindAll(obstacle => obstacle.Item2 == 'C');

            /**                 refactor me!!               */
            // needs to be pulled out into a separate fn
            // (code ripped wholesale from `Map.cs` here)
            var coordinates = typeof(Map.Coordinates).GetFields();

            Map.Coordinates start = new Map.Coordinates();
            Map.Coordinates size = new Map.Coordinates();
            Render render = new Render();

            string[] rawCoordinates = args.Skip(1).ToArray();
            int[] parsedCoordinates = new int[2];

            // bunch of garbage that isn't super necessary here.
            try {
                for (int i = 0; i < rawCoordinates.Length; ++i) {
                    parsedCoordinates[i] = int.Parse(rawCoordinates[i]);
                    if (parsedCoordinates.Any(num => num < 0)) {
                        return "Width and height must be valid positive integers.";
                    }
                }
            } catch (Exception e) {
                if (e is FormatException || e is ArgumentException) {
                    return "Coordinates are not valid integers.";
                }
            }

            coordinates[0].SetValue(start, int.Parse(rawCoordinates[0]));
            coordinates[1].SetValue(start, int.Parse(rawCoordinates[1]));
            coordinates[0].SetValue(size, 1);
            coordinates[1].SetValue(size, 1);


            /**
             *                  refactor ahbopve!!!!!!!
             * */

            // this (from 'render' (?)) also could probably use its own fn call
            foreach (var camera in cameras) {
                if (camera.Item3 is not null) {
                    render.CameraBlocking(start, size, camera.Item3,
                                          camera.Item1.Item1,
                                          camera.Item1.Item2);
                }
            }
        }
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
