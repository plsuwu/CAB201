namespace a2cs;


class Render {

    // this is a really tidy solution - unfortunately i dont think this will work for the camera obstacle.
    public string[] Map(Map.Coordinates agent, Map.Coordinates goal) {

        // using `LinkedList<T>` here as this function only wants nodes appended to its structure
        LinkedList<string> map = new LinkedList<string>();
        List<((int, int), char)> fixedObstacles = Active.fixedObstacles;

        int xSize = goal.X - agent.X;
        int ySize = goal.Y - agent.Y;

        map.AddFirst("Here is a map of obstacles in the selected region:");
        for (int y = 0; y < ySize; ++y) {
            LinkedList<char> line = new LinkedList<char>(); // create a new list for each line
            for (int x = 0; x < xSize; ++x) {

                // bind the first matching `obstacles` tuple by comparing to the iterator's current cell (x,y)
                // if there is no match, bind a default value (null).
                ((int, int), char)blocked = fixedObstacles.FirstOrDefault(
                    obstacle =>
                        obstacle.Item1.Item1 == x && obstacle.Item1.Item2 == y);

                // use the character from a bound obstacle tuple if we matched the current cell to a blocked
                // cell
                LinkedListNode<char> node =
                    (blocked != default
                         ? new LinkedListNode<char>(blocked.Item2)
                         : new LinkedListNode<char>('.'));

                line.AddLast(node);
            }

            // push the result as a single string to the end of the main list
            map.AddLast(string.Concat(line));
        }

        // does this have a tangible effect on our O(1) LL?
        return map.ToArray();
    }
}
