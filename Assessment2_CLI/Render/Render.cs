using System.Text;
namespace a2cs;

// forced to refactor as two important data structure choices were fundamentally incorrect:
//  - blockedCells: originally used `List<((int,int), char, string?)>`  => disgustingly poor lookup,
//  - map:          originally used `LinkedList<LinkedList<char>>`      => cursed mem alloc/dealloc overhead
//
// this was slowing the `Map` method below to an absolute crawl lol
class Render {

    public string[] Map(Grid.Cell start, Grid.Cell size) {
        List<string> map = new List<string>();

        var blockedCellsClone = new Dictionary<(int, int), (char, string?)>(Obstacle.blockedCells);
        foreach (var obstacle in blockedCellsClone) {
            if (obstacle.Value.Item1 == 'C' && obstacle.Value.Item2 != null) {
                Camera.GetBlockedCells(start, size, obstacle.Key.Item1, obstacle.Key.Item2, obstacle.Value.Item2);
            }
        }

        for (int i = 0; i < size.Y; ++i) {
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
