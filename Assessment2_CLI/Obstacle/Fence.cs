namespace a2cs;

class Fence {
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
