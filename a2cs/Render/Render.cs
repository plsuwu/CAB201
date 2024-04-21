namespace a2cs;

class Render {

    // this is a really neat solution for objects blocking cells we can precalculate, which i dont think will work for
    // the camera obstacle.
    public string[] Map(Map.Coordinates start, Map.Coordinates size) {

        // using `LinkedList<T>` here as this function only wants nodes appended to its structure
        LinkedList<string> map = new LinkedList<string>();
        List<((int, int), char)> fixedObstacles = Active.fixedObstacles;

        for (int i = 0; i < size.Y; ++i) {
            LinkedList<char> line = new LinkedList<char>(); // create a new list for each line

            // the starting (x, y) coordinates are not related to the map width/height, so a separate iterator
            // must be used to correctly determine the value of each
            int y = i + start.Y;

            for (int j = 0; j < size.X; ++j) {
                int x = j + start.X;

                // bind the first matching `fixedObstacles` tuple by comparing to the iterator's current cell (x,y)
                // if there is no match, bind a default value.
                ((int, int), char)blocked = fixedObstacles.FirstOrDefault(
                    obstacle =>
                        obstacle.Item1.Item1 == x && obstacle.Item1.Item2 == y);

                // use the character from a bound obstacle tuple if we matched the current cell to a blocked
                // cell
                LinkedListNode<char> node =
                    (blocked != default
                         ? new LinkedListNode<char>(blocked.Item2)
                         : new LinkedListNode<char>('.'));

                line.AddLast(node); // left-to-right by appending node on the end
            }

            // push the result as a single string to the end of the main list
            // map.AddLast(string.Concat(line));
            map.AddFirst(string.Concat(line)); // bottom-to-top by inserting node at the top
        }

        // add the message as the first node to print it above the map
        map.AddFirst("Here is a map of obstacles in the selected region:");
        return map.ToArray();
    }
}
