namespace a2cs;

    // a little messy (no good at variable names) but we are performing more calculations in
    // terms of the values' squares with only one reasonably cheap sqrt operation in the outermost loop;
    //
    // this avoids a large number of expensive sqrt ops on double precision floating point values
    // which will kill the clr given large enough values :)
class Sensor {

    public static void Add(int x, int y, double range) {
        int rangeIntCeil = (int)Math.Ceiling(range);
        int leftBound = x - rangeIntCeil;
        int rightBound = x + rangeIntCeil;
        double rangeSquared = Math.Pow(range, 2);

        // scan horizontally - left to right
        for (int i = leftBound; i <= rightBound; ++i) {

            // (X2 - X1)^2
            double deltaXSquared = Math.Pow(i - x, 2);

            // if we are inside horizontal bounds, scan vertically
            if (deltaXSquared <= rangeSquared) {
                double verticalMax = Math.Sqrt(rangeSquared - deltaXSquared);
                int lowerBound = (int)Math.Floor(y - verticalMax);
                int upperBound = (int)Math.Floor(y + verticalMax); // math.ceil? doesnt seem to matter.

                for (int j = lowerBound; j <= upperBound; ++j) {

                    // (X2 - X1)^2 + (Y2 - Y1)^2
                    // make sure the current squared distance from center to current cell (i, j)
                    double totalDistanceSquared = deltaXSquared + Math.Pow(j - y, 2);

                    // if we are inside vertical bounds, create the camera point
                    if (totalDistanceSquared <= rangeSquared) {
                        Obstacle.New(i, j, 'S', null);
                    }
                }
            }
        }
    }
}
