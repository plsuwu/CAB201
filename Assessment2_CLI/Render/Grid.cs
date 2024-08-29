namespace a2cs;

class Grid {
    private const int __X_COORDINATE_INDEX = 0;
    private const int __Y_COORDINATE_INDEX = 1;
    private const int __ARG_OBJECTIVE_X_INDEX = 2;
    private const int __NUM_COORDINATE_PAIRS = 2;
    private const string __INVALID_AGENT_COORDS =
        "Agent coordinates are not valid integers.";
    private const string __INVALID_OBJEC_COORDS =
        "Objective coordinates are not valid integers.";

    /// <summary>
    /// class describing the X/Y coordinate values
    /// </summary>
    public struct Cell {
        public int X;
        public int Y;

        /// <summary>
        /// getter/setter for the above class
        /// </summary>
        /// <param name="x">x-coordinate for the cell</param>
        /// <param name="y">y-coordinate for the cell</param>
        public Cell(int x, int y) {
            X = x;
            Y = y;
        }

        /// <summary>
        /// determeines whether the specified cell is equal to a current cell based on its x,y coordinates
        /// </summary>
        /// <param name="obj">the object being compared with the current object</param>
        /// <returns>true if `obj` is equal to the current object, otherwise false</returns>
        public override bool Equals(object? obj) {
            if (obj is Cell cell) {
                return X == cell.X && Y == cell.Y;
            }

            return false;
        }

        /// <summary>
        /// overrides the default hash function so that cell objects can be compared based on their coordinates
        /// </summary>
        /// <returns>a hash code for the current object</returns>
        public override int GetHashCode() {
            return (X, Y).GetHashCode();
        }
    }

    /// <summary>
    /// method to build a list containing the starting cell as well as the gridsize, represented as a `Cell` object
    /// </summary>
    /// <param name="start">the coordinates of the starting cell</param>
    /// <param name="size">the size of the grid</param>
    /// <returns>a list containing the two Cell objects</returns>
    public List<Cell> Build(int[] start, int[] size) {
        List<Cell> coordinates = new List<Cell>();

        // create the starting cell and the grid size from an array of coordinates
        Cell startCell = new Cell { X = start[__X_COORDINATE_INDEX],
                                    Y = start[__Y_COORDINATE_INDEX] };
        Cell sizeOfGrid = new Cell { X = size[__X_COORDINATE_INDEX],
                                     Y = size[__Y_COORDINATE_INDEX] };

        // push the cells to the coordinates list and return the filled list
        coordinates.Add(startCell);
        coordinates.Add(sizeOfGrid);

        return coordinates;
    }
}
