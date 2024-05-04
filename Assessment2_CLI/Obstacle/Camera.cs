namespace a2cs;

class Camera {

    public static void Add(int x, int y, string? direction) {
        Obstacle.New(x, y, 'C', direction);
    }

    public static void GetBlockedCells(Grid.Cell start, Grid.Cell size, int x, int y, string direction) {
        int hMax = Math.Min(start.X, x) + Math.Max(size.X - 1, x);
        int vMax = Math.Min(start.Y, y) + Math.Max(size.Y - 1, y);

        switch (direction) {

            case "north":
                for (int i = 0; y + i <= vMax; ++i) {
                    int stepXLower = Math.Max(start.X, x - i);
                    int stepXUpper = Math.Min(hMax, x + i);
                    for (int j = stepXLower; j <= stepXUpper; ++j) {
                        Add(j, y + i, null);
                    }
                }
                break;

            case "south":
                for (int i = 0; y - i >= start.Y; ++i) {
                    int stepXLower = Math.Max(start.X, x - i);
                    int stepXUpper = Math.Min(hMax, x + i);
                    for (int j = stepXLower; j <= stepXUpper; ++j) {
                       Add(j, y - i, null);
                    }
                }
                break;

            case "east":
                for (int i = 0; x + i <= hMax; ++i) {
                    int stepYLower = Math.Max(start.Y, y - i);
                    int stepYUpper = Math.Min(vMax, y + i);
                    for (int j = stepYLower; j <= stepYUpper; ++j) {
                        Add(x + i, j, null);
                    }
                }
                break;

            case "west":
                for (int i = 0; x - i >= start.X; ++i) {
                    int stepYLower = Math.Max(start.Y, y - i);
                    int stepYUpper = Math.Min(vMax, y + i);
                    for (int j = stepYLower; j <= stepYUpper; ++j) {
                        Add(x - i, j, null);
                    }
                }
                break;

        }

        return;
    }
}
