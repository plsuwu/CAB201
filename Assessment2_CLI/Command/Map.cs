namespace a2cs;

/// <summary>
/// handles the formation of a renderable region of the map from the <c>Map</c> command
/// </summary>
class Map : Command.ICommand {
    private const int START_CELL_INDEX = 0;
    private const int SIZE_CELL_INDEX = 1;
    private const int COORDINATE_PAIRS = 2;
    private const int NUM_EXPECTED_ARGS = 5;

    private const string INCORRECT_NUM_ARGS =
        "Incorrect number of arguments.";
    private const string INVALID_COORDINATES =
        "Coordinates are not valid integers.";
    private const string INVALID_WIDTH_HEIGHT =
        "Width and height must be valid positive integers.";

    public string Handler(string[] args) {
        if (args.Length != NUM_EXPECTED_ARGS) {
            return INCORRECT_NUM_ARGS;
        }

        string[] rawCoordinates = args.Skip(1).ToArray(); // discard command argument

        // perform validity checks on the arguments, returning an error if we cannot convert the argument to an int type
        int[] parsedCoordinates = new int[rawCoordinates.Length];
        try {
            for (int i = 0; i < rawCoordinates.Length; ++i) {
                parsedCoordinates[i] = int.Parse(rawCoordinates[i]);
            }
        } catch (Exception err) {
            if (err is FormatException || err is ArgumentException) {
                return INVALID_COORDINATES;
            }
        }

        // form the grid
        int[] startCell = new int[COORDINATE_PAIRS];
        int[] sizeOfGrid = new int[COORDINATE_PAIRS];

        for (int i = 0; i < parsedCoordinates.Length / COORDINATE_PAIRS;
             ++i) {
            if (parsedCoordinates[i + COORDINATE_PAIRS] <= 0) {
                return INVALID_WIDTH_HEIGHT;
            }
            startCell[i] = parsedCoordinates[i];
            sizeOfGrid[i] = parsedCoordinates[i + COORDINATE_PAIRS];
        }

        // build the grid as coordinates
        Grid grid = new Grid();
        List<Grid.Cell> coordinates = grid.Build(startCell, sizeOfGrid);

        // call the render function with the built map, reverse the array, concatenate the strings (delimiting
        // with a newline) and return the resulting string
        Render render = new Render();
        string[] map = render.Map(coordinates[START_CELL_INDEX],
                                  coordinates[SIZE_CELL_INDEX]);

        return string.Join("\n", map.Reverse());
    }
}
