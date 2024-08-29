namespace a2cs;

public class Obstacle {
    /// <summary>
    /// static, globally-available dictionary containing objects. this uses the
    /// object's [x,y] coordinate as its key, and the character + any bonus args
    /// (only used for cameras to determine a base instance to build a blocking
    /// area out from)
    /// </summary>
    public static Dictionary<(int, int), (char, string?)> blockedCells =
        new Dictionary<(int, int), (char, string?)>();

    /// <summary>
    /// static, globally available convenience function to call when an object
    /// needs to be pushed to the obstacle dictionary
    /// </summary>
    /// <param name="x">x-coordinate of the obstacle</param>
    /// <param name="y">y-coordinate of the obstacle</param>
    /// <param name="type">associated character for this obstacle (i.e, one of
    /// 'G', 'F', 'S', or 'C')</param> <param name="options">any other required
    /// kwargs - currently only used for the base camera obstacle, such that we
    /// can determine the direction it faces when called at the point of map
    /// rendering</param>
    public static void New(int x, int y, char type, string? options = null) {
        blockedCells[(x, y)] = (type, options);
    }
}
