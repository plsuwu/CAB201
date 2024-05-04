namespace a2cs;

class Sensor {
    // its a little messy
    public static void Add(int x, int y, double range) {
        int rangeInt = (int)Math.Ceiling(range);
        int left = x - rangeInt;
        int right = x + rangeInt;
        double rangeSquared = range * range;

        for (int i = left; i <= right; ++i) {
            double dXSquared = (i - x) * (i - x);

            if (dXSquared <= rangeSquared) {
                double hMax = Math.Sqrt(rangeSquared - dXSquared);
                int bottom = (int)Math.Floor(y - hMax);
                int top = (int)Math.Floor(y + hMax);

                for (int j = bottom; j <= top; ++j) {
                    double totalDistanceSquared = dXSquared + (j - y) * (j - y);
                    if (totalDistanceSquared <= rangeSquared) {
                        Obstacle.New(i, j, 'S', null);
                    }
                }
            }
        }
    }
}
