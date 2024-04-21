namespace a2cs;

public class Active {
    // maybe we calculate the blocking area of each obstacle
    // and then append it as an
    //
    //      ((int int), (int int), string obstacle <"G", "F", "C", "S">);
    //        ^           ^         ^           ^         ^
    //        from(x,y)   to(x,y)   one of obstacle symbols
    //

    static public List<((int, int), char)> fixedObstacles =
        new List<((int, int), char)>();

    public static void Guard(int x, int y) {
        fixedObstacles.Add(((x, y), 'G'));
    }

    public static void Fence(int x, int y, string orientation, int len) {
        for (int i = 0; i < len; ++i) {
            if (orientation == "north") {
                fixedObstacles.Add(((x, y + i), 'F'));
            } else {
                fixedObstacles.Add(((x + i, y), 'F'));
            }
        }
    }

    public static void Sensor(int x, int y, double range) {
        double squaredRange = range * range;
        int left = (int)Math.Floor(x - range);
        int right = (int)Math.Ceiling(x + range);

        for (int xIter = left; xIter <= right; ++xIter) {
            double deltaXSquare =
                (xIter - x) * (xIter - x);  // precalculated squared horizontal

            if (deltaXSquare <= squaredRange) {
                double yMax = Math.Sqrt(squaredRange - deltaXSquare);
                int bottom = (int)Math.Floor(y - yMax);
                int top = (int)Math.Ceiling(y + yMax);

                for (int yIter = bottom; yIter <= top; ++yIter) {
                    double totalDistanceSquared =
                        deltaXSquare + (yIter - y) * (yIter - y);
                    if (totalDistanceSquared <= squaredRange) {
                        fixedObstacles.Add(((xIter, yIter), 'S'));
                    }
                }
            }
        }
    }
}
