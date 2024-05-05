namespace a2cs;

class Sensor {

    // this one is a little messy (no good at variable names) but we are performing more calculations with
    // respect to squared values with only one sqrt operation in the outermost loop
    public static void Add(int x, int y, double range) {
        int rangeIntCeil = (int)Math.Ceiling(range);
        int leftBound = x - rangeIntCeil;
        int rightBound = x + rangeIntCeil;
        double rangeSquared = range * range; // likely faster than function call overhead for `Math.Pow()`


        // scan horizontally - left to right
        for (int i = leftBound; i <= rightBound; ++i) {
            double deltaXSquared = (i - x) * (i - x); // (X2 - X1)^2

            // if we are inside horizontal bounds, scan vertically
            if (deltaXSquared <= rangeSquared) {
                double verticalMax = Math.Sqrt(rangeSquared - deltaXSquared);
                int lowerBound = (int)Math.Floor(y - verticalMax);
                int upperBound = (int)Math.Floor(y + verticalMax); // math.ceil? doesnt seem to matter.

                for (int j = lowerBound; j <= upperBound; ++j) {

                    // make sure the current squared distance from center to current cell (i, j)
                    double totalDistanceSquared = deltaXSquared + (j - y) * (j - y);  // n + (X2 - X1)^2 + (Y2 - Y1)^2

                    // if we are inside vertical bounds, create the camera point
                    if (totalDistanceSquared <= rangeSquared) {
                        Obstacle.New(i, j, 'S', null);
                    }
                }
            }
        }
    }
}
