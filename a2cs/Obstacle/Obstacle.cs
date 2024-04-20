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

        // avoid big `Math.Sqrt` operations for this, especially on floating point numbers
        double squareRange = range * range;

        int left = (int)Math.Floor(x - range);
        int right = (int)Math.Ceiling(x + range);

        List<(int, int)> cells = new List<(int, int)>();

        for (int i = left; i <= right; ++i) {
            double squareX = Math.Pow(i - x, 2); // dont attempt sqrt'ing a negative int
            if (squareX <= squareRange) {
                double yComp = Math.Sqrt(squareRange - squareX);
                int bottom = (int)Math.Floor(y - yComp);
                int top = (int)Math.Ceiling(y + yComp);

                for (int j = bottom; j <= top; ++j) {
                    double distance = squareX + Math.Pow(j - y, 2);
                    if (distance <= squareRange) {
                        fixedObstacles.Add(((i, j), 'S'));
                    }
                }
            }
        }
    }
}
