using System.Text;
namespace a2cs;

/// <summary>
/// the render manager class - responsible for creating a grid from safe characters alongside the obstacles' characters
/// </summary>
class Render {

    /// <summary>
    /// the main function for rendering a map
    /// </summary>
    /// <param name="start">the starting <c>Grid.Cell</c></param>
    /// <param name="size">the overall map size</param>
    /// <returns>an array of strings that describe the layout of obstacles within a region</returns>
    public string[] Map(Grid.Cell start, Grid.Cell size) {
        List<string> map = new List<string>();

        // clone the obstacles dict so we can iterate over it and call the `GetBlockedCells` func
        // without throwing when camera blocking cells are added
        var blockedCellsClone = new Dictionary<(int, int), (char, string?)>(Obstacle.blockedCells);

        // if there are any camera instances that contain a non-null optional string value, use that camera instance
        // as a 'base' to construct the blocking effect of a camera on a particular region
        foreach (var obstacle in blockedCellsClone) {
            if (obstacle.Value.Item1 == 'C' && obstacle.Value.Item2 != null) {

                // call the instantiation function with the known mapped area
                Camera.GetBlockedCells(start, size, obstacle.Key.Item1, obstacle.Key.Item2, obstacle.Value.Item2);
            }
        }

        // for each Y-coordinate, create a new line representing the horizontal plane/x-axis
        for (int i = 0; i < size.Y; ++i) {
            // offset the current iterator `i` by the vertical size to get the true y-coordinate
            int y = i + start.Y;
            StringBuilder line = new StringBuilder(); // mutable string

            // for each X-coordinate, check to see if the
            for (int j = 0; j < size.X; ++j) {
                // offset the current iterator `j` by the horization size to get the true x-coordinate
                int x = j + start.X;

                // if there is an obstacle at this location, use its character at this location
                if (Obstacle.blockedCells.TryGetValue((x, y), out var obstacle)) {
                    line.Append(obstacle.Item1);
                } else {
                    // otherwise, use the safe character
                    line.Append('.');
                }
            }

            // push the line into the map, concatenating the string in the StringBuilder object
            map.Add(line.ToString());
        }

        // insert the message at the front of the array, and convert the `List<string>` structure to an array type
        map.Add("Here is a map of obstacles in the selected region:");
        return map.ToArray();
    }
}
