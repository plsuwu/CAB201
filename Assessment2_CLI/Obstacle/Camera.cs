namespace a2cs;

/// <summary>
/// Class containing <c>Camera</c> obstacle instantiation and functions
/// </summary>
class Camera {

    /// <summary>
    /// Public instantiation function - pushes a <c>Camera</c> instance to the active objects dictionary.
    /// </summary>
    /// <param name="x">The instance's horizontal position</param>
    /// <param name="y">The instance's vertical position</param>
    /// <param name="direction">If this is the base Camera instance, this parameter informs the generation of its blocking cells</param>
    public static void Add(int x, int y, string? direction) {
        Obstacle.New(x, y, 'C', direction);
    }

    /// <summary>
    /// Given a base <c>Camera</c> cell, direction, and known map size, generates that <c>Camera</c>'s blocking cells within the given map area.
    /// <para>
    /// Using the camera's direction to draw a 'core' sightline from the camera's initial cell.
    /// Walk down that core line until the furthest horizontal or vertical coordinate containing a cell is reached.
    /// With each core step, incrementally expand the vision area around the core sightline in a triangular pattern by
    /// adding the same number of steps taken to either side of the core.
    /// </para>
    /// </summary>
    /// <param name="start">Known <c>Map</c> starting cell</param>
    /// <param name="size">Known <c>Map</c> area size, calculated relative to the given start cell</param>
    /// <param name="x">Base <c>Camera</c> horizontal position </param>
    /// <param name="y">Base <c>Camera</c> vertical position </param>
    /// <param name="direction">Base <c>Camera</c> facing direction (n/s/e/w)</param>
    public static void GetBlockedCells(Grid.Cell start, Grid.Cell size, int x, int y, string direction) {

        // calculate the different 'max' distances depending on camera direction
        int hMax = Math.Min(start.X, x) + Math.Max(size.X - 1, x); // for east/west
        int vMax = Math.Min(start.Y, y) + Math.Max(size.Y - 1, y); // for north/south

        switch (direction) {

            case "north":
                // ... 'for each cell `i` between the starting cell of this camera and the max distance pMax' ...
                for (int i = 0; y + i <= vMax; ++i) {

                    // ... 'find the perpendicular area of the camera's sightline
                    //      given its current distance from the starting square' ...
                    int stepXLower = Math.Max(start.X, x - i);
                    int stepXUpper = Math.Min(hMax, x + i);
                    // ... 'and for each cell in the previous range, call the `Add` method
                    //      to push these coordinates to the dictionary of blocking cells'.
                    for (int j = stepXLower; j <= stepXUpper; ++j) {
                        // `y + i` describes the current y-coord iterator as an offset from the initial camera location.
                        Add(j, y + i, null);
                    }
                }
                break;

            // slight differences for each direction; e.g, here we reverse the check from using `<= vMax`
            // to use `>= start.Y` so that the iterator walks in the opposite direction, and change `y + i`
            // to use `y - i` so that we describe a negative offset (ie, towards lower values).
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
