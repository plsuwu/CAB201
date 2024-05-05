namespace a2cs;

class Camera {

    // push the core camera location to the obstacles dict - this will be probably created once a
    // grid size is given
    public static void Add(int x, int y, string? direction) {
        Obstacle.New(x, y, 'C', direction);
    }

    // uses a known grid area to create the full blocking effect of a camera
    public static void GetBlockedCells(Grid.Cell start, Grid.Cell size, int x, int y, string direction) {

        int hMax = Math.Min(start.X, x) + Math.Max(size.X - 1, x);
        int vMax = Math.Min(start.Y, y) + Math.Max(size.Y - 1, y);

        switch (direction) {

            // using the camera's direction to determine the where camera's 'core' sightline points, walk
            // down that core until the furthest horizontal or vertical coordinate containing a cell is
            // reached, incrementally expanding the vision around the core sightline in a triangle by
            // adding the same number of steps taken to both sides of the core.
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
