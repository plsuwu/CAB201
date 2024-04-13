namespace a2cs;

public class Path {

    static int[][] Dirs = [[-1, 0], [1, 0], [0, 1], [0, -1]];
    // this coord parser is the same (or the same function in essence) to that
    // of `Render.Map()` or whatever thjat method is
    public string Cells(string[] args) {
        if (args.Length != 5) {
            return "Incorrect number of arguments.";
        }

        string[] rawCoordinates = args.Skip(1).ToArray();
        int[] parsedCoordinates = new int[rawCoordinates.Length];

        List<(int, int)> unsafeCell = new List<(int, int)>();

        int[] agent = new int[2];
        int[] goal = new int[2];

        // this error handling isnt correct for this command
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

        // put all unsafe cells into this iterator somehow idk
        foreach (var guard in Active.guards) {
            unsafeCell.Add(guard);
        }

        for (int i = 0; i < args.Length / 2; ++i) {
            agent[i] = int.Parse(rawCoordinates[i]);
            goal[i] = int.Parse(rawCoordinates[(i + 2)]);
        }

        // // this is the renderer func; make the map a `string[]` type and `unsafeCell`s into '#' or something
        // // similar and this is just a matter of recursively pushing & popping cells from `seen` (dont pop
        // // from this) and `path`.
        //
        // for (int y = 0; y < ySize; ++y) {
        //     LinkedList<string> line = new LinkedList<string>();

        //     for (int x = 0; x < xSize; ++x) {
        //         LinkedListNode<string> node = new LinkedListNode<string>(".");
        //         line.AddLast(node);
        //     }

        //     map.AddLast(string.Concat(line));
        // }

        var safePath = FindPath(agent, goal, unsafeCell);
    }

    // args are probably wrong im eepy
    public bool Walk(int[] maze, List<(int, int)> unsafeWall, int[] agent, int[] goal, bool[][] seen, int[][] path) {
        // base cases
        // ...
        // (thjis is from my ts algos but u get the point). ...
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
    }

    public int[][] FindPath(int[] maze, List<(int,int)> unsafeCell, int[] agent, int[] goal) {
        // impl recursive walker with stack
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
    }
}

