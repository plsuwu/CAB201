using System.Text;
namespace a2cs;

//  - blockedCells:     `List<((int,int), char, string?)>`      => disgustingly poor lookup perf
//  - map:              `LinkedList<LinkedList<char>>`          => cursed alloc/dealloc overhead
// this was slowing the method below to an absolute crawl
class Render {

    public string[] Map(Grid.Cell start, Grid.Cell size) {
        List<string> map = new List<string>();

        // clone the obstacles dict so we can iterate over it and call the `GetBlockedCells` func
        // without throwing when camera blocking cells are added
        var blockedCellsClone = new Dictionary<(int, int), (char, string?)>(Obstacle.blockedCells);

        foreach (var obstacle in blockedCellsClone) {
            if (obstacle.Value.Item1 == 'C' && obstacle.Value.Item2 != null) {
                Camera.GetBlockedCells(start, size, obstacle.Key.Item1, obstacle.Key.Item2, obstacle.Value.Item2);
            }
        }

        for (int i = 0; i < size.Y; ++i) {

            // this implementation was different in my original solution, so i think it might be ok
            // to start the i/j iterators at `start.Y`/`start.X`
            int y = i + start.Y;
            StringBuilder line = new StringBuilder();

            for (int j = 0; j < size.X; ++j) {
                int x = j + start.X;

                if (Obstacle.blockedCells.TryGetValue((x, y), out var obstacle)) {
                    line.Append(obstacle.Item1);
                } else {
                    line.Append('.');
                }
            }

            map.Add(line.ToString());
        }

        map.Add("Here is a map of obstacles in the selected region:");
        return map.ToArray();
    }
}
