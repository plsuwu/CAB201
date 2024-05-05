namespace a2cs;

// doesn't return any kind of expected response - i am still trying to figure out what
// i'm doing with this.
// this is only just started so it is pretty messy:))
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

    private static readonly Dictionary<string, int[]> directions =
        new Dictionary<string, int[]> {
            { "North", new int[] { 0, 1 } },
            { "East", new int[] { 1, 0 } },
            { "South", new int[] { 0, -1 } },
            { "West", new int[] { -1, 0 } },
        };

    // testing purposes, will likely want to use above dict
    private static readonly int[][] dirsArray = {
        new int[] { 0, 1 },
        new int[] { 1, 0 },
        new int[] { 0, -1 },
        new int[] { -1, 0 },
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

        // these could implement IEnumerable to instantiate easier?
        Grid.Cell startAsCell = new Grid.Cell();
        Grid.Cell goalAsCell = new Grid.Cell();

        for (int i = 0; i < parsedCoordinates.Length / __NUM_COORDINATE_PAIRS; ++i) {
            startCell[i] = parsedCoordinates[i];
            endCell[i] = parsedCoordinates[i + __NUM_COORDINATE_PAIRS];
            sizeOfGrid[i] = (Math.Abs(endCell[i]) + 1) - Math.Abs(startCell[i]);
        }

        // this is not permanent but for the purpose of having something work
        startAsCell.X = startCell[__DIR_X_INDEX];
        startAsCell.Y = startCell[__DIR_Y_INDEX];
        goalAsCell.X = endCell[__DIR_X_INDEX];
        goalAsCell.Y = endCell[__DIR_Y_INDEX];

        Grid grid = new Grid();
        List<Grid.Cell> coordinates = grid.Build(startCell, sizeOfGrid);

        Render render = new Render();
        string[] mapRaw =
            render.Map(coordinates[__INDEX_START_CELL], coordinates[__INDEX_END_CELL]);

        char[][] mapChars = SplitMap(mapRaw, sizeOfGrid);

        Console.WriteLine($"{startAsCell.X}, {startAsCell.Y}");
        Console.WriteLine($"{goalAsCell.X}, {goalAsCell.Y}");
        var path = FindPath(mapChars, startAsCell, goalAsCell);

        // Console.WriteLine($"{path.Peek()}");

        return ""; // we return junk right now and print to stdout while building
    }

    // very much trying to make a start rather than be fully functional; this implementation will find a single
    // path when that path is surrounded by obstacles but isn't so great for the actual task.
    private bool WalkMap(char[][] map, Grid.Cell current, Grid.Cell goal,
                         List<int[]> path, bool[][] seen) {

        // Console.WriteLine($"standing on char: '{map[current.X][current.Y].ToString()}'.");
        // (x < 0 || y < 0) so we dont just walk off into the distance but this might
        // need to be modified if we need to walk back to move around an obstacle
        if (current.X < 0 || current.X >= map[0].Length || current.Y < 0 ||
            current.Y >= map.Length) {
            return false;
        }
        if (map[current.X][current.Y] != __SAFE_CHAR) {
            return false;
        }
        if (current.X == goal.X && current.Y == goal.Y) {
            path.Add([goal.X, goal.Y]);
            return true;
        }
        if (seen[current.X][current.Y]) {
            return false;
        }

        seen[current.X][current.Y] = true;
        path.Add([current.X, current.Y]);

        // log values to stdout as they are pushed
        Console.WriteLine($"added `{current.X},{current.Y}` to path stack.");
        foreach (var coord_pair in path) {
            Console.Write($"[ ({coord_pair[0]},{coord_pair[1]}) ] \n");
        }


        // this needs to try to actively increment TOWARDS the goal else we just add
        // every single free coord to stack, which isnt super helpful.
        //
        for (int i = 0; i < dirsArray.Length; ++i) {

            // is this yielding the iterator value to return? is that how this has always worked??
            // i feel like this little routine should:
            //      1. increment the iterator,
            //      2. use the iterator to index `dirsArray`,
            //      3. bind adjacent to the sum of the current coord
            //          with the indexed coordinate
            //      4a. iterate through the dirsArray until we return true or
            //          run out of directions to try, or,
            //      4b. return true to the caller if we meet the
            //          criteria, which should restart this iterator, no?
            //
            // but we see to push values to path indicating we're just doing the
            // full loop and pushing the entire circle to `path`??
            //            >
            //         ^    v
            //           <

            Grid.Cell adjacent = new Grid.Cell();

            adjacent.X = current.X + dirsArray[i][0];
            adjacent.Y = current.Y + dirsArray[i][1];

            Console.WriteLine($"{adjacent.X}, {adjacent.Y}");
                    // const [x, y] = dir[i];
                    // if (walk(maze, wall,
                    // {
                    //     x: curr.x + x,
                    //     y: curr.y + y
                    // },
                    // end, seen, path))
                    // {
                    //     return true;
                    // }
            if (WalkMap(map, adjacent, goal, path, seen)) {
                return true;
            }
        }

        path.RemoveAt(path.Count - 1);
        return false;
    }

    private List<int[]> FindPath(char[][] map, Grid.Cell start, Grid.Cell goal) {
        List<int[]> path = new List<int[]>();
        bool[][] seen = new bool [map.Length][];

        for (int i = 0; i < map.Length; ++i) {
            seen[i] = Enumerable.Repeat(false, map[i].Length).ToArray();
        }

        Console.WriteLine($"starting @ {start.X}, {start.Y}");
        WalkMap(map, start, goal, path, seen);

        foreach (var coord in path) {
            Console.WriteLine($"[ {coord[0]},{coord[1]} ], ");
        }

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
