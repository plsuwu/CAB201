namespace a2cs;

// this isnt quite finished so its a little messy
public class Path : Command.ICommand {

    private const int __ARG_OBJECTIVE_X_INDEX = 2;
    private const int __INDEX_START_CELL = 0;
    private const int __INDEX_X_COORDINATE = 0;
    private const int __INDEX_Y_COORDINATE = 1;
    private const int __DIR_X_INDEX = 0;
    private const int __DIR_Y_INDEX = 1;
    private const int __INDEX_END_CELL = 1;
    private const int __NUM_COORDINATE_PAIRS = 2;
    private const int __NUM_EXPECTED_ARGS = 5;
    private const char __SAFE_CHAR = '.';

    private static readonly HashSet<char> __BLOCKED_CELL_CHARS =
        new HashSet<char> { 'G', 'F', 'S', 'C' };

    // i've slightly modified the order here; it does a fun little spiral
    private static readonly Dictionary<string, int[]> directions =
        new Dictionary<string, int[]> {
            { "North", new int[] { 0, 1 } },
            { "East", new int[] { 1, 0 } },
            { "South", new int[] { 0, -1 } },
            { "West", new int[] { -1, 0 } },
        };

    private char[][] SplitMap(string[] map, int[] gridSize) {
        var droppedMessage = map.Reverse().Skip(1).ToArray();
        // Console.WriteLine($"{gridSize[__INDEX_Y_COORDINATE]}");

        char[][] grid = new char[gridSize[__INDEX_Y_COORDINATE]][];
        for (int i = 0; i < gridSize[__INDEX_Y_COORDINATE]; ++i) {
            grid[i] = droppedMessage[i].ToCharArray();
        }

        return grid;
    }

    public string Handler(string[] args) {
        if (args.Length != __NUM_EXPECTED_ARGS) {
            return "Incorrect number of arguments.";
        }

        // this section should be pulled into its own func along with the
        // similar section in `Map.cs`
        string[] rawCoordinates = args.Skip(1).ToArray();
        int[] parsedCoordinates = new int[rawCoordinates.Length];

        for (int i = 0; i < parsedCoordinates.Length; ++i) {
            try {
                parsedCoordinates[i] = int.Parse(rawCoordinates[i]);
            } catch (Exception err) {
                if (err is FormatException || err is ArgumentException) {
                    return i < __ARG_OBJECTIVE_X_INDEX
                               ? "Agent coordinates are not valid integers."
                               : "Objective coordinates are not valid integers.";
                }
            }
        }

        int[] startCell = new int[__NUM_COORDINATE_PAIRS];
        int[] endCell = new int[__NUM_COORDINATE_PAIRS];
        int[] sizeOfGrid = new int[__NUM_COORDINATE_PAIRS];

        for (int i = 0; i < parsedCoordinates.Length / __NUM_COORDINATE_PAIRS; ++i) {
            startCell[i] = parsedCoordinates[i];
            endCell[i] = parsedCoordinates[i + __NUM_COORDINATE_PAIRS];
            sizeOfGrid[i] = (Math.Abs(endCell[i]) + 1) - Math.Abs(startCell[i]);
        }

        Grid grid = new Grid();
        List<Grid.Cell> coordinates = grid.Build(startCell, sizeOfGrid);

        Render render = new Render();
        string[] mapRaw =
            render.Map(coordinates[__INDEX_START_CELL], coordinates[__INDEX_END_CELL]);

        char[][] mapChars = SplitMap(mapRaw, sizeOfGrid);
        Stack<(int, int)> path = FindPath(mapChars, coordinates[__INDEX_START_CELL],
                                         coordinates[__INDEX_END_CELL]);

        return "";
    }

    private bool WalkMap(char[][] map, char safeChar, Grid.Cell current, Grid.Cell goal,
                         Stack<(int, int)> path, bool[][] seen) {
        if (current.X < 0 || current.X >= map[0].Length || current.Y < 0 ||
            current.Y >= map.Length) {
            return false;
        }
        if (map[current.X][current.Y] != safeChar) {
            return false;
        }
        if (current == goal) {
            path.Push((goal.X, goal.Y));
            return true;
        }
        if (seen[current.X][current.Y]) {
            return false;
        }

        seen[current.X][current.Y] = true;
        path.Push((current.X, current.Y));

        // debug
        Console.WriteLine($"added `{current.X},{current.Y}` to path stack.");
        Console.WriteLine($"peeking stack: {path.Peek()}");

        // this needs to try to actively increment TOWARDS the goal else we just add
        // every single free coord to stack, which isnt overly helpful.
        foreach (var dir in directions) {
            Grid.Cell adjacent = new Grid.Cell();

            // could call Check method here instead?
            adjacent.X = current.X + dir.Value[__DIR_X_INDEX];
            adjacent.Y = current.Y + dir.Value[__DIR_Y_INDEX];

            if (WalkMap(map, safeChar, adjacent, goal, path, seen)) {
                return true;
            }
        }

        path.Pop();
        return false;
    }

    private Stack<(int, int)> FindPath(char[][] map, Grid.Cell start, Grid.Cell goal) {
        Stack<(int, int)> path = new Stack<(int, int)>();
        bool[][] seen = new bool [map.Length][];

        for (int i = 0; i < map.Length; ++i) {
            seen[i] = Enumerable.Repeat(false, map[i].Length).ToArray();
        }

        Console.WriteLine($"starting @ {start.X}, {start.Y}");
        WalkMap(map, __SAFE_CHAR, start, goal, path, seen);

        return path;
    }
    /*
        function solve(maze: string[], wall: string, start: Point, end: Point): Point[] {
                const seen: boolean[][] = [];
                const path: Point[] = [];

                for (let i = 0; i < maze.length; i++) {
                        seen.push(new Array(maze[0].length).fill(false));
                }

                walk(maze, wall, start, end, seen, path);
                return path;
        }
        function walk(maze: string[], wall: string, curr: Point, end: Point, seen:
       boolean[][], path: Point[]): boolean { if (curr.x < 0 || curr.x >= maze[0].length
       || curr.y < 0 || curr.y >= maze.length) {

                        return false;
                }

                if (maze[curr.y][curr.x] === wall) {
                        return false;
                }

                if (curr.x === end.x && curr.y === end.y) {
                        path.push(end);
                        return true;
                }

                if (seen[curr.y][curr.x]) {
                        return false;
                }

                // current square is not:
            //      off the map,
            //      a wall,
            //      the end square,
            //      a point in the `seen` array,
            // so we recurse.

            // pre
                seen[curr.y][curr.x] = true;
                path.push(curr);

                // recurse
                for (let i = 0; i < dir.length; ++i) {
                    const [x, y] = dir[i];
                    if (walk(maze, wall, {
                        x: curr.x + x,
                        y: curr.y + y
                    }, end, seen, path)) {
                        return true;
                    }
                }

                // post
                path.pop();

                return false;
        }
    */
}
