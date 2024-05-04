namespace a2cs;

public class Obstacle {

    // globally accessible dict of cells containing an obstacle
    static public Dictionary<(int, int), (char, string?)> blockedCells =
        new Dictionary<(int, int), (char, string?)>();

    public static void New(int x, int y, char type, string? options = null) {
        blockedCells[(x, y)] = (type, options);
    }
}
