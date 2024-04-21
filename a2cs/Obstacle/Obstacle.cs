namespace a2cs;

public class Active {

    static public List<((int, int), char, string?)> fixedObstacles =
        new List<((int, int), char, string?)>();

    public static void Guard(int x, int y) {
        fixedObstacles.Add(((x, y), 'G', null));
    }

    // the camera's blocking zone will be calculated during Render function runtime
    public static void Camera(int x, int y, string direction) {
        fixedObstacles.Add(((x, y), 'C', direction)); // the camera's args will always be valid here.
        var cam = fixedObstacles.FindAll(obstacle => obstacle.Item2 == 'C');
        foreach (var c in cam) {
            Console.WriteLine("{0}", c);
        }
    }

    public static void Fence(int x, int y, string orientation, int len) {
        for (int i = 0; i < len; ++i) {
            if (orientation == "north") {
                fixedObstacles.Add(((x, y + i), 'F', null));
            } else {
                fixedObstacles.Add(((x + i, y), 'F', null));
            }
        }
    }

    public static void Sensor(int x, int y, double range) {
        double rangeSquared = range * range;
        int left = (int)Math.Floor(x - range);
        int right = (int)Math.Ceiling(x + range);

        for (int xIter = left; xIter <= right; ++xIter) {
            double deltaXSquare =
                (xIter - x) * (xIter - x);  // pre-calculate squared horizontal

            if (deltaXSquare <= rangeSquared) {
                double yMax = Math.Sqrt(rangeSquared - deltaXSquare);
                int bottom = (int)Math.Floor(y - yMax);
                int top = (int)Math.Ceiling(y + yMax);

                for (int yIter = bottom; yIter <= top; ++yIter) {
                    double totalDistanceSquared =
                        deltaXSquare + (yIter - y) * (yIter - y);
                    if (totalDistanceSquared <= rangeSquared) {
                        fixedObstacles.Add(((xIter, yIter), 'S', null));
                    }
                }
            }
        }
    }
}
