namespace a2cs;

class Render {
    public string[] Map(Map.Coordinates agent, Map.Coordinates goal) {

        // LinkedList<string> as we only want to push Nodes to the end of the structure
        LinkedList<string> map = new LinkedList<string>();

        int xSize = goal.X - agent.X;
        int ySize = goal.Y - agent.Y;


        map.AddFirst("Here is a map of obstacles in the selected region:");
        for (int y = 0; y < ySize; ++y) {
            LinkedList<string> line = new LinkedList<string>();

            for (int x = 0; x < xSize; ++x) {
                LinkedListNode<string> node = new LinkedListNode<string>(".");
                line.AddLast(node);
            }

            map.AddLast(string.Concat(line));
        }

        // does this have a tangible effect on our O(1) LL?
        return map.ToArray();
    }
}
