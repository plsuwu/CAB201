namespace a2cs;

class Guard {
    /// <summary>
    /// Adder function for a guard obstacle
    /// </summary>
    /// <param name="x">x-coordinate for this guard instance</param>
    /// <param name="y">y-coordinate for this guard instance</param>
    public static void Add(int x, int y) {
        // simple instance - we just want to associate a `G` char with its location
        Obstacle.New(x, y, 'G', null);
    }
}
