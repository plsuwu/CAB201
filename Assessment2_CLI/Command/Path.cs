namespace a2cs;
// didnt manage to finish this one :( - i feel like i am very close!!


/// <summary>
/// This class handles the pathfinding command.
/// </summary>
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
    private const string __INVALID_AGENT_COORDS = "Agent coordinates are not valid integers.";
    private const string __INVALID_OBJEC_COORDS = "Objective coordinates are not valid integers.";
    private const string __INCORRECT_NUM_ARGS = "Incorrect number of arguments.";

    /// <summary>
    /// A hashset that contains a list of characters indicating whether a cell
    /// is blocked by an obstacle.
    /// </summary>
    private static readonly HashSet<char> __BLOCKED_CELL_CHARS =
        new HashSet<char> { 'G', 'F', 'S', 'C' };

    /// <summary>
    /// This array holds a nested array of offsets that assist with pathfinding
    /// directions.
    /// </summary>
    private static int[][] dirsArray = {
        new int[] { 0, 1 },
        new int[] { 1, 0 },
        new int[] { 0, -1 },
        new int[] { -1, 0 },
    };

    /// <summary>
    /// <c>SplitMap</c> takes output from the <c>Grid.Render</c> function and
    /// splits it into an array of nested arrays containing each sequential
    /// character for a given <c>Map</c>, such that it can be iterated through
    /// in the pathfinding method.
    /// </summary>
    /// <param name="map">The <c>Grid.Render</c> output</param>
    /// <param name="gridSize">The overall size of the grid</param>
    /// <returns>An array containing a nested array of <c>char</c>s.</returns>
    private char[][] SplitMap(string[] map, int[] gridSize) {
        var droppedMessage =
            map.Reverse().Skip(1).ToArray().Reverse().ToArray();  // reverse the render and remove
                                                                  // the inserted output message

        char[][] grid = new char [droppedMessage.Length][];
        // iterate through the main array, instantiating the size of the
        // return variable's nested array
        for (int i = 0; i < droppedMessage.Length; ++i) {
            grid[i] = new char[droppedMessage[0].Length];
            // iterate through the nested array, pushing each character in
            // the render to a new array
            for (int j = 0; j < droppedMessage[0].Length; ++j) {
                grid[i][j] = droppedMessage[i][j];
            }
        }

        return grid;
    }

    public string Handler(string[] args) {
        if (args.Length != __NUM_EXPECTED_ARGS) {
            return __INCORRECT_NUM_ARGS;
        }

        int repetition = 0;
        // drop the main command arg so coordinates are easier to work with
        string[] rawCoordinates = args.Skip(1).ToArray();
        int[] parsedCoordinates = new int[rawCoordinates.Length];

        // try/catch block to validate args
        for (int i = 0; i < parsedCoordinates.Length; ++i) {
            try {
                parsedCoordinates[i] = int.Parse(rawCoordinates[i]);
            } catch (Exception err) {
                // if the args cnanot be parsed out to an int, return an
                // error
                if (err is FormatException || err is ArgumentException) {
                    return i < __ARG_OBJECTIVE_X_INDEX ? __INVALID_AGENT_COORDS
                                                       : __INVALID_OBJEC_COORDS;
                }
            }
        }
        while (true) {
            // create objects to hold the starting cell coordinate, the ending
            // cell (goal/object) coordinate, and the width and height of the
            // grid.
            int[] startCell = new int[__NUM_COORDINATE_PAIRS];
            int[] endCell = new int[__NUM_COORDINATE_PAIRS];
            int[] sizeOfGrid = new int[__NUM_COORDINATE_PAIRS];

            // these could implement IEnumerable to instantiate easier?
            Grid.Cell startAsCell = new Grid.Cell();
            Grid.Cell goalAsCell = new Grid.Cell();

            for (int i = 0; i < parsedCoordinates.Length / __NUM_COORDINATE_PAIRS; ++i) {
                startCell[i] = parsedCoordinates[i];
                endCell[i] = parsedCoordinates[i + __NUM_COORDINATE_PAIRS];
                sizeOfGrid[i] = (Math.Abs(endCell[i]) + 1) - Math.Abs(startCell[i] - 1);
            }
            try {
                // cast the int[] type into the Grid.Cell type
                startAsCell.X = startCell[__DIR_X_INDEX];
                startAsCell.Y = startCell[__DIR_Y_INDEX];
                goalAsCell.X = endCell[__DIR_X_INDEX];
                goalAsCell.Y = endCell[__DIR_Y_INDEX];

                if (startAsCell.Equals(goalAsCell)) {
                    return "Agent, you are already at the objective.";
                }

                // expand scope per repitition
                sizeOfGrid[0] += repetition;
                sizeOfGrid[1] += repetition;
                // build the grid coordinates
                Grid grid = new Grid();
                List<Grid.Cell> coordinates = grid.Build(startCell, sizeOfGrid);

                // render the grid as text
                Render render = new Render();
                string[] mapRaw =
                    render.Map(coordinates[__INDEX_START_CELL], coordinates[__INDEX_END_CELL]);

                if (Obstacle.blockedCells.TryGetValue((goalAsCell.X, goalAsCell.Y),
                                                      out var obstacle)) {
                    return "The objective is blocked by an obstacle and cannot be reached.";
                }

                // call the splitting function to split the render into its
                // characters
                char[][] mapChars = SplitMap(mapRaw, sizeOfGrid);

                // attempt to find a path
                var path = FindPath(mapChars, startAsCell, goalAsCell);
                List<string> result = new List<string>();

                // if we return a valid path, convert that path into directions
                // (head <direction> for <n> klick(s))
                List<string> cardinals = PathAsCardinal(path);
                cardinals.Add("");

                return string.Join("\n", cardinals);
            } catch (IndexOutOfRangeException) {
                repetition++;
                if (repetition > 50) {
                    return "There is no safe path to the objective.";
                }
            }
        }
    }

    /// <summary>
    /// Recursive walker function that iterates through a list of directions,
    /// using a <c>Heuristic</c> function to determine Manhattan distance to
    /// walk towards the objective efficiently.
    /// </summary>
    /// <param name="map">Output from the <c>SplitMap</c> function</param>
    /// <param name="curr">The current <c>Cell</c> coordinate</param>
    /// <param name="path">A stack (FILO structure) holding the current path
    /// that this function has walked</param> <param name="seen">A set
    /// containing tuples of ints (int,int) representing coordinates that this
    /// function has been to</param> <param name="goal">The agent's
    /// object</param> <param name="offset">An offset to adjust the <c>map</c>
    /// length such that it is relative to the start/end cell values</param>
    private bool Walk(char[][] map, Grid.Cell curr, Stack<Grid.Cell> path, HashSet<(int, int)> seen, Grid.Cell goal, Grid.Cell offset) {
        // base cases
        //
        // if we are currently on the goal, push the value onto the stack and
        // return true.
        if (curr.Equals(goal)) {
            path.Push(curr);
            return true;
        }

        // if we are outside our bounds, return false
        if (curr.X < 0 || curr.Y > (map.Length + offset.Y) || curr.Y < 0 ||
            curr.X > (map[1].Length + offset.Y)) {
            return false;
        }

        // if we are on an unsafe cell, return false
        if (map[curr.Y - offset.Y][curr.X - offset.X] != '.') {
            return false;
        }

        // if we have already visited this cell, return false
        if (seen.Contains((curr.X, curr.Y))) {
            return false;
        }

        // pre
        //
        // add the current cell to the list of seen cells and push it onto the
        // path stack
        seen.Add((curr.X, curr.Y));
        path.Push(curr);

        // heuristic - determine which coordinate to use when we recurse
        Array.Sort(dirsArray, (a, b) => {
            int dA = Heuristic(curr.X + a[0], curr.Y + a[1], goal);
            int dB = Heuristic(curr.X + b[0], curr.Y + b[1], goal);
            return dA.CompareTo(dB);
        });

        // recurse
        //
        // iterating through the array of directions while the recursively
        // called 'walk()' function returns false
        foreach (var dir in dirsArray) {
            Grid.Cell next = new Grid.Cell(curr.X + dir[0], curr.Y + dir[1]);
            if (Walk(map, next, path, seen, goal, offset)) {
                // return true when the Walk() func returns true (i.e, at the
                // goal cell)
                return true;
            }
        }

        // post
        //
        // if we cannot continue forward from here, we are at a dead end;
        // pop the value from the top of the stack and return false for this
        // cell
        path.Pop();
        return false;
    }

    /// <summary>
    /// calculates the Manhattan distance for this cell in comparison to the
    /// goal
    /// </summary>
    /// <param name="x">the x coordinate of the cell being tested</param>
    /// <param name="y">the y coordinate of the cell being tested</param>
    /// <param name="goal">the objective/goal cell</param>
    private int Heuristic(int x, int y, Grid.Cell goal) {
        return Math.Abs(x - goal.X) + Math.Abs(y - goal.Y);
    }

    /// <summary>
    /// Attempts to find a path from a starting cell to the objective
    /// </summary>
    /// <param name="map">the char array returned from the <c>SplitMap</c>
    /// function</param> <param name="start">the agent cell</param> <param
    /// name="goal">the objective cell</param>
    private List<Grid.Cell> FindPath(char[][] map, Grid.Cell start, Grid.Cell goal) {
        Stack<Grid.Cell> path = new Stack<Grid.Cell>();  // easy to push/pop values from - FILO
        HashSet<(int, int)> seen = new HashSet<(int, int)>();  // quick hashed lookup

        // if we return true from the Walk function, reverse the resulting path
        // to get the correct direction order, and return it
        if (Walk(map, start, path, seen, goal, start)) {
            List<Grid.Cell> result = new List<Grid.Cell>(path);
            result.Reverse();
            return result;
        }

        // if we do not return true, return an empty list
        return new List<Grid.Cell>();
    }

    /// <summary>
    /// Converts a list of coordinate values into an instruction set using
    /// Klicks and cardinal directions
    /// </summary>
    /// <param name="path">the return value of <c>FindPath</c></param>
    /// <returns>a list of directions to the objective</returns>
    private List<string> PathAsCardinal(List<Grid.Cell> path) {
        List<string> result = new List<string>();
        if (path.Count == 0) {
            // if the path doesn't have any items, then FindPath was unable to
            // safely direct the agent to the objective
            result.Add("There is no safe path to the objective.");
            return result;
        }

        // push the expected message string to the list first
        result.Add("The following path will take you to the objective:");

        // initialize variables to bind the previous item in the path (used to
        // compare to the current item to determine the direction), the number
        // of steps taken in that direction, and the previous direction, also
        // used to check that we have not taken steps in a different direction
        Grid.Cell prev = path[0];
        int step = 0;
        string prevDirection = new string(string.Empty);

        for (int i = 1; i < path.Count; ++i) {
            Grid.Cell curr = path[i];  // current step is the index of this iterator

            // the direction taken is equal to the current coordinate minus the
            // previous coordinate pattern match the resulting tuple to get the
            // cardinal direction
            string direction = (curr.X - prev.X, curr.Y - prev.Y) switch {
                (0, 1) => "north",
                (0, -1) => "south",
                (1, 0) => "east",
                _ => "west"
            };

            // if we havent changed direction, increment the counter for the
            // steps taken in the current direction
            if (direction == prevDirection) {
                step++;
            } else {
                // otherwise, append the number of steps we took before changing
                // direction as an instruction
                if (step > 0) {
                    string kl = step == 1 ? "klick" : "klicks";
                    result.Add($"Head {prevDirection} for {step} {kl}.");
                }
                // reset our variables
                prevDirection = direction;
                step = 1;
            }

            // set the previous item to the current item at the end of the
            // current iteration
            prev = curr;
        }

        // final check after the iterator is done
        if (step > 0) {
            string kl = step == 1 ? "klick" : "klicks";
            result.Add($"Head {prevDirection} for {step} {kl}.");
        }

        // return the list
        return result;
    }
}
