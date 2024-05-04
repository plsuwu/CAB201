namespace a2cs;

class Map : Command.ICommand {

    private const int __START_CELL_INDEX = 0;
    private const int __SIZE_CELL_INDEX = 1;
    private const int __COORDINATE_PAIRS = 2;
    private const int __NUM_EXPECTED_ARGS = 5;

    public string Handler(string[] args) {
        if (args.Length != __NUM_EXPECTED_ARGS) {
            return "Incorrect number of arguments.";
        }

        string[] rawCoordinates = args.Skip(1).ToArray();

        // logic from here until about line 44 could be pulled out into its own
        // method
        int[] parsedCoordinates = new int[rawCoordinates.Length];

        try {
            for (int i = 0; i < rawCoordinates.Length; ++i) {
                parsedCoordinates[i] = int.Parse(rawCoordinates[i]);
            }
        } catch (Exception err) {
            if (err is FormatException || err is ArgumentException) {
                return "Coordinates are not valid integers.";
            }
        }

        int[] startCell = new int[__COORDINATE_PAIRS];
        int[] sizeOfGrid = new int[__COORDINATE_PAIRS];

        for (int i = 0; i < parsedCoordinates.Length / __COORDINATE_PAIRS; ++i) {
            if (parsedCoordinates[i] > parsedCoordinates[i + __COORDINATE_PAIRS]) {
                return "Width and height must be valid positive integers.";
            }
            startCell[i] = parsedCoordinates[i];
            sizeOfGrid[i] = parsedCoordinates[i + __COORDINATE_PAIRS];
        }

        Grid grid = new Grid();
        List<Grid.Cell> coordinates = grid.Build(startCell, sizeOfGrid);

        Render render = new Render();
        string[] map =
            render.Map(coordinates[__START_CELL_INDEX], coordinates[__SIZE_CELL_INDEX]);

        return string.Join("\n", map);
    }
}
