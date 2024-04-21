namespace a2cs;

public class Path : Command.ICommand {

    static readonly int[][] Directions = [[-1, 0], [1, 0], [0, 1], [0, -1]];

    public string Handler(string[] args) {
        if (args.Length != 5) {
            return "Incorrect number of arguments.";
        }

        // discard main command from args array
        string[] rawCoordinates = args.Skip(1).ToArray();

        // create objects to store parsed data
        int[] parsedCoordinates = new int[4];
        var coordinates = typeof(Map.Coordinates).GetFields();
        Map.Coordinates agent = new Map.Coordinates();
        Map.Coordinates goal = new Map.Coordinates();


        try {
            for (int i = 0; i < rawCoordinates.Length; ++i) {
                parsedCoordinates[i] = int.Parse(rawCoordinates[i]);
                if (parsedCoordinates.Any(num => num < 0)) {
                    return "Width and height must be valid positive integers.";
                }
            }
        } catch (Exception e) {
            if (e is FormatException || e is ArgumentException) {
                return "Agent coordinates are not valid integers.";
            }
        }

        for (int i = 0; i < args.Length / 2; ++i) {
            coordinates[i].SetValue(agent, int.Parse(rawCoordinates[i]));
            coordinates[i].SetValue(goal, int.Parse(rawCoordinates[(i + 2)]));
        }

        // create a render instance and offload parsed data to renderer function
        Render renderer = new Render();
        string[] render = renderer.Map(agent, goal);

        return "any";

        // rudimentary implementation, might want to expand scope of walker if a valid path
        // exists but can't be found.
    }
}


    // args are probably wrong im eepy
    // public bool Walk(int[] maze, List<(int, int)> unsafeWall, int[] agent, int[] goal, bool[][] seen, int[][] path) {
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
    // }

    // public int[][] FindPath(int[] maze, List<(int,int)> unsafeCell, int[] agent, int[] goal) {
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
    // }
// }

