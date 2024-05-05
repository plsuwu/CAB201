namespace a2cs;

public class Obstacle {

    // globally accessible dictionary of cells containing an obstacle - allows very
    // efficient item lookup - no need to traverse each item in an array/list
    static public Dictionary<(int, int), (char, string?)> blockedCells =
        new Dictionary<(int, int), (char, string?)>();

    public static void New(int x, int y, char type, string? options = null) {
        blockedCells[(x, y)] = (type, options);
    }
}
