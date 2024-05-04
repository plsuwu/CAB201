namespace a2cs;

public class Path : Command.ICommand {

    private const int __START_CELL_INDEX = 0;
    private const int __SIZE_CELL_INDEX = 1;
    private const int __COORDINATE_PAIRS = 2;
    private const int __NUM_EXPECTED_ARGS = 5;

    public string Handler(string[] args) {
        if (args.Length != __NUM_EXPECTED_ARGS) {
            return "Incorrect number of arguments.";
        }

        string[] rawCoordinates = args.Skip(1).ToArray();

        // to be refactored - see `Map.cs` - note slight semantic differences in
        // expected errors and commented statements.
        int[] parsedCoordinates = new int[rawCoordinates.Length];

        for (int i = 0; i < parsedCoordinates.Length; ++i) {
            try {
                parsedCoordinates[i] = int.Parse(rawCoordinates[i]);
            } catch (Exception err) {
                if (err is FormatException || err is ArgumentException) {
                    return i < 2 ?
                    // note that this line differs to the one in `Map.cs`
                    "Agent coordinates are not valid integers."
                      : "Objective coordinates are not valid integers.";
                }
            }
        }

        int[] startCell = new int[__COORDINATE_PAIRS];
        int[] sizeOfGrid = new int[__COORDINATE_PAIRS];

        for (int i = 0; i < parsedCoordinates.Length / __COORDINATE_PAIRS; ++i) {
            // if (parsedCoordinates[i] > parsedCoordinates[i + __COORDINATE_PAIRS]) {
            //     return "Width and height must be valid positive integers.";
            // }
            startCell[i] = parsedCoordinates[i];
            sizeOfGrid[i] = parsedCoordinates[i + __COORDINATE_PAIRS];
        }

        Grid grid = new Grid();
        List<Grid.Cell> coordinates = grid.Build(startCell, sizeOfGrid);

        Render render = new Render();
        string[] map =
            render.Map(coordinates[__START_CELL_INDEX], coordinates[__SIZE_CELL_INDEX]);

        var mapSplitChars = SplitRender(map);
        var path = FindPath(mapSplitChars);

        return "unimplemented";
    }
    //
    //     // discard main command from args array
    //     string[] rawCoordinates = args.Skip(1).ToArray();
    //
    //     // create objects to store parsed data
    //     int[] parsedCoordinates = new int[4];
    //     var coordinates = typeof(Map.Coordinates).GetFields();
    //     Map.Coordinates agent = new Map.Coordinates();
    //     Map.Coordinates goal = new Map.Coordinates();
    //
    //
    //     try {
    //         for (int i = 0; i < rawCoordinates.Length; ++i) {
    //             parsedCoordinates[i] = int.Parse(rawCoordinates[i]);
    //             if (parsedCoordinates.Any(num => num < 0)) {
    //                 return "Width and height must be valid positive integers.";
    //             }
    //         }
    //     } catch (Exception e) {
    //         if (e is FormatException || e is ArgumentException) {
    //             return "Agent coordinates are not valid integers.";
    //         }
    //     }
    //
    //     for (int i = 0; i < args.Length / 2; ++i) {
    //         coordinates[i].SetValue(agent, int.Parse(rawCoordinates[i]));
    //         coordinates[i].SetValue(goal, int.Parse(rawCoordinates[(i + 2)]));
    //     }
    //
    //     // create a render instance and offload parsed data to renderer function
    //     Render renderer = new Render();
    //     string[] render = renderer.Map(agent, goal);
    //
    //
    //     return string.Join("\n", render);
    //     // result from below might be too rudimentary of an implementation, might want
    //     to expand scope
    //     // of walker if a valid path exists but can't be found.
    //     // also, probably want to check that we aren't moving AWAY from the objective
    //     if there is a valid path
    //     // that moves us closer...??
    //     //      -> maybe a gradually expanding scope as a last resort is the best best
    //     here, so we are
    //     //          'boxed in', so to speak.
    // }

    // args are probably wrong im eepy
    // public bool Walk(int[] maze, List<(int, int)> unsafeWall, int[] agent, int[] goal,
    // bool[][] seen, int[][] path) { base cases
    // ...
    // (this is typescript but the idea is there)
    //
    // 	if (curr.x < 0 || curr.x >= maze[0].length ||
    //         curr.y < 0 || curr.y >= maze.length) {
    //
    // 		return false;
    // 	}
    //
    // 	if (maze[curr.y][curr.x] === wall) {
    // 		return false;
    // 	}
    //
    // 	if (curr.x === end.x && curr.y === end.y) {
    // 		path.push(end);
    // 		return true;
    // 	}
    //
    // 	if (seen[curr.y][curr.x]) {
    // 		return false;
    // 	}
    // 	// recurse
    // 	for (let i = 0; i < dir.length; ++i) {
    // 		const [x, y] = dir[i];
    // 		if (walk(maze, wall, { x: curr.x + x, y: curr.y + y,}, end, seen, path)) {
    // 			return true;
    // 		}
    // 	}
    // 	// post
    // 	path.pop();
    // 	return false;
    // }

    // return an array of directions (e.g [ [0,1],     [0,1],      [1,0]]) to describe the
    // relative directions.
    //                                       ^ north,   ^ north,    ^ east
    // sequences could then be counted to determine the path traversed in terms of the
    // expected output
    //                                                                      `head <dir>
    //                                                                      for <n>
    //                                                                      klick(s)`
    private int[][] FindPath(List<List<char>> map) {

        return [[0]];

        // public int[][] FindPath(int[] maze, List<(int,int)> unsafeCell, int[] agent,
        // int[] goal) { impl recursive walker with stack that it can push/pop from
        // something along these lines:
        //
        // 	const seen: boolean[][] = [];
        // 	const path: Point[] = [];
        //
        // 	for (let i = 0; i < maze.length; i++) {
        // 		seen.push(new Array(maze[0].length).fill(false));
        // 	}
        //
        // 	walk(maze, wall, start, end, seen, path);
        // 	return path;
        // }
    }

    private List<List<char>> SplitRender(string[] map) {
        List<List<char>> mapChars = new List<List<char>>();
        var baseMap = map.Skip(1).ToList();

        for (int i = 0; i < map.Skip(1).ToArray().Length; ++i) {
            List<char> line = baseMap[i].ToCharArray().ToList<char>();
            mapChars.Add(line);
        }

        return mapChars;
    }
}
