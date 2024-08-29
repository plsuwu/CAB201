namespace a2cs;

class Sensor {

    /// <summary>
    /// Adder function for the sensor obstacle. We want to avoid as many expensive `sqrt()` operations as possible, especially
    /// concerning double-precision floating point values or during inner loops, so we true to calculate as much as we can in
    /// outer loops and with respect to squared values
    /// </summary>
    /// <param name="x">x-coordinate for this sensor instance</param>
    /// <param name="y">y-coordinate for this sensor instance</param>
    /// <param name="range">double-precision floating point number to describe the range of the sensor in cells</param>
    public static void Add(int x, int y, double range) {
        int rangeIntCeil = (int)Math.Ceiling(range);
        int leftBound = x - rangeIntCeil;
        int rightBound = x + rangeIntCeil;
        double rangeSquared = range * range; // likely faster than the overhead for a function call for `Math.Pow(n)`


        // scan horizontally - left to right
        for (int i = leftBound; i <= rightBound; ++i) {
            double deltaXSquared = (i - x) * (i - x); // this is `(X2 - X1)^2`

            // if we are inside horizontal bounds, scan vertically
            if (deltaXSquared <= rangeSquared) {
                double verticalMax = Math.Sqrt(rangeSquared - deltaXSquared);
                int lowerBound = (int)Math.Floor(y - verticalMax);
                int upperBound = (int)Math.Floor(y + verticalMax); // this should probably be `Math.Ceil()`, however it works so i leave it

                for (int j = lowerBound; j <= upperBound; ++j) {

                    // make sure the current squared distance from center to current cell (i, j)
                    double totalDistanceSquared = deltaXSquared + (j - y) * (j - y);  // this is `n + (X2 - X1)^2 + (Y2 - Y1)^2`

                    // if we are inside vertical bounds, create the camera point
                    if (totalDistanceSquared <= rangeSquared) {
                        Obstacle.New(i, j, 'S', null);
                    }
                }
            }
        }
    }
}
