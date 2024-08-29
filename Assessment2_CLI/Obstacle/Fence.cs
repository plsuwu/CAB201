namespace a2cs;

class Fence {
    /// <summary>
    /// Adder function for the <c>Fence</c> obstacle
    /// <summary>
    /// <param name="x">x-coordinate for this fence obstacle instance</param>
    /// <param name="y">y-coordinate for this fence obstacle instance</param>
    /// <param name="orientation">one of [east, north] to determine whether the fence extends vertically or horizontally</param>
    /// <param name="len">length that the fence extends out from its initial cell</param>
    public static void Add(int x, int y, string orientation, int len) {

        // give the fence length and orientation by incrementing the x/y coordinate value
        // respective to its indicated orientation, pushing each new coordinate pair to
        // the obstacles dict per iteration
        for (int i = 0; i < len; ++i) {
            if (orientation == "north") {
                Obstacle.New(x, y + i, 'F', null); // vertically oriented
            } else {
                Obstacle.New(x + i, y, 'F', null); // horizontally oriented
            }
        }
    }
}
