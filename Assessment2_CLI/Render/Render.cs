namespace a2cs;

// though it works, this class is in a pretty unreadable state (sorry) - i plan on coming back and refactoring
// this all out. hopefully the comments throughout aren't making things worse lol
class Render {

    // this can be pulled out into `Obstace/Camera.cs`
    public static void CameraBlocking(Grid.Cell start, Grid.Cell size, string direction, int x, int y) {

        // this function takes a camera's location and direction, along with a known grid area, and pushes any
        // relevant cells blocked by the camera to the `fixedObstacles` array (which will probably end up as a
        // hashset - to avoid creating a gigantic list of redundant objects + hashed lookups).
        //
        //
        // noting that each incremental step in the camera's direction adds two blocked cells perpendicular to
        // that direction, below diagram shows how the coords for a north-facing camera are built:
        //     1. starting from the camera's location:
        //         a. create an iterator, `i`, which will increment the y coordinate by 1 to gradually walk north,
        //         b. for each `i` iteration, create a new iterator, `j`, which offsets the camera's initial x value
        //              by `-i`, incrementing the x coordinate until it has walked a number of steps
        //              equal to the number that have been taken vertically
        //         c. if any of these cells are within the specified grid area, push the values of the iterators
        //              as the location of a camera instance (`x` == `j` && `y` == `i`),
        //      2. repeat `1.a`-`1.c` until we are outside the bounds of a given grid's area.
        //
        // ```
        //      .ccc|c|ccc    // iteration 3 appends ...
        //      ..cc|c|cc.    // iteration 2 appends [(3,2),(4,2),(5,2),(6,2),(7,2)]
        //      ...c|c|c..    // iteration 1 appends [(4,1),(5,1),(6,1)]
        //      ....|c|...    // iteration 0 appends [(5,0)]
        //           ^
        //           camera center: `x` == 5, `y` == 0
        // ```
        //
        // cases for each direction are slightly different (depending on whether we want to walk forward/backward,
        // up/down), but fundamentally remain the same.

        // max horizontal distance for a given grid
        int hMax = Math.Min(start.X, x) + Math.Max(size.X - 1, x);

        // max vertical distance for a given grid
        int vMax = Math.Min(start.Y, y) + Math.Max(size.Y - 1, y);

        switch (direction) {
            case "north":

                // vertical iterator increments y coordinate (ie, walks north)
                // until the top of the map is reached
                for (int i = 0; y + i <= vMax; ++i) {
                    for (int j = -i; j <= i; ++j) {
                        // if the current i & j iterators represent a coordinate
                        // inside the map, push that value to the list of active
                        // obstacles
                        if (x + j >= start.X && x + j <= hMax) {
                            Active.fixedObstacles.Add(
                                ((x + j, y + i), 'C', null));
                        }
                    }
                }
                break;

            case "south":
                // as above but decrement y to walk down
                for (int i = 0; y - i >= start.Y; ++i) {
                    for (int j = -i; j <= i; ++j) {
                        if (x + j >= start.X && x + j <= hMax) {
                            Active.fixedObstacles.Add(
                                ((x + j, y - i), 'C', null));
                        }
                    }
                }
                break;

            // swap x/y & v/h in above cases to walk horizontally
            case "east":
                for (int i = 0; x + i <= hMax; ++i) {
                    for (int j = -i; j <= i; ++j) {
                        if (y + j >= start.Y && y + j <= vMax) {
                            Active.fixedObstacles.Add(
                                ((x + i, y + j), 'C', null));
                        }
                    }
                }
                break;

            case "west":
                for (int i = 0; x - i >= start.X; ++i) {
                    for (int j = -i; j <= i; ++j) {
                        if (y + j >= start.Y && y + j <= vMax) {
                            Active.fixedObstacles.Add(
                                ((x - i, y + j), 'C', null));
                        }
                    }
                }
                break;
        }

        return;
    }

    public string[] Map(Grid.Cell start, Grid.Cell size) {
        // using `LinkedList` to build a FILO/stack structure
        LinkedList<string> map = new LinkedList<string>();
        List<((int, int), char, string?)> fixedObstacles = Active.fixedObstacles;

        // check for cameras and create their blocking zones - our `fixedObstacles` list
        // here should be a hashset (no duplicates, faster lookup)
        //
        // if there are any camera obstacles in the active obstacles list, get
        // their coordinates
        if (fixedObstacles.Any(obstacle => obstacle.Item2 == 'C')) {
            var cameras =
                fixedObstacles.FindAll(obstacle => obstacle.Item2 == 'C');

            foreach (var camera in cameras) {
                // check for a `direction` string to determine which camera to use where a
                // camera may already have been pushed to the obstacles array
                if (camera.Item3 is not null) {

                    // call the above camera activation method that will use the known map dimensions
                    // to determine relevant blocking locations.
                    CameraBlocking(start, size, camera.Item3, camera.Item1.Item1, camera.Item1.Item2);

                    // `camera.Item3` SHOULD NOT be set to null here, otherwise we cannot
                    // re-render a larger map/different portion of the map.
                }
            }
        }
        for (int i = 0; i < size.Y; ++i) {
            LinkedList<char> line = new LinkedList<char>();  // create a new list for each line

            // the starting (x, y) coordinates are not related to the map width/height, so a separate
            // iterator must be used to correctly determine the value of each
            int y = i + start.Y;

            for (int j = 0; j < size.X; ++j) {
                int x = j + start.X;

                // bind the first `fixedObstacles` matching this cell's coordinates; default (null?) value is
                // bound if no matching coordinate is found.
                ((int, int), char, string?) blocked =
                    fixedObstacles.FirstOrDefault(obstacle => (
                        obstacle.Item1.Item1 == x &&
                        obstacle.Item1.Item2 == y
                    ));

                // push the object's character if `blocked` contains a binding, otherwise push safe cell char.
                LinkedListNode<char> node =
                    (blocked != default
                         ? new LinkedListNode<char>(blocked.Item2)
                         : new LinkedListNode<char>('.'));

                line.AddLast(node);  // left-to-right; append node at tail
            }

            // push the result as a single string to the end of the main list
            map.AddFirst(string.Concat(line));  // bottom-to-top; concatenate node and insert at head

            // i believe pushing characters to a linked list is the same as `System.Text.StringBuilder`, though
            // i feel like this gives a greater degree of control and StringBuilder doesn't make that much
            // sense to me if i am honest.
        }

        // add the message at the head to print it above the map
        map.AddFirst("Here is a map of obstacles in the selected region:");
        return map.ToArray();
    }
}
