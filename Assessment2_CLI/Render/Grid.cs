namespace a2cs;

class Grid {

    private const int __X_COORDINATE_INDEX = 0;
    private const int __Y_COORDINATE_INDEX = 1;

    public class Cell {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public List<Cell> Build(int[] start, int[] size) {

        List<Cell> coordinates = new List<Cell>();

        Cell startCell = new Cell {
            X = start[__X_COORDINATE_INDEX],
            Y = start[__Y_COORDINATE_INDEX]
        };

        Cell sizeOfGrid = new Cell {
            X = size[__X_COORDINATE_INDEX],
            Y = size[__Y_COORDINATE_INDEX]
        };

        coordinates.Add(startCell);
        coordinates.Add(sizeOfGrid);

        return coordinates;
    }
}
