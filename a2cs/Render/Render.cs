namespace a2cs;

class Render {
    public void CameraBlocking(Map.Coordinates start, Map.Coordinates size,
                               string direction, int x, int y) {
        // Console.WriteLine($"camera obstacle facing {direction} at {x}, {y}??");

        // could be cleaner but im taking the functional implementation for the time
        // being

        int hMax = start.X + size.X - 1; // represents the maximum horizontal distance for an E/W-facing camera
        int vMax = start.Y + size.Y - 1; // maximum vertical distance for a N/S-facing camera

        // Console.WriteLine(
        //     $"map(xy), (xy): ({start.X},{start.Y}), ({hMax}, {vMax})");

        // shrimple as
        switch (direction) {
            case "north":

                // vertical iterator increases y coordinate (ie, walks north), until the
                // top of the map is exceeded
                for (int i = 0; y + i <= vMax; ++i) {

                    // for each step upward, offset the horizontal vision by
                    // the same value on either side of the camera center
                    for (int j = -i; j <= i; ++j) {

                        // if the current i & j iterators represent a coordinate inside
                        // the map, push that value to the list of active obstacles
                        if (x + j >= start.X && x + j <= hMax) {
                            Active.fixedObstacles.Add(((x + j, y + i), 'C', null));
                        }
                    }
                }
                break;

            case "south":
                // similar to above but decrement y to walk down
                for (int i = 0; y - i >= start.Y; ++i) {
                    for (int j = -i; j <= i; ++j) {
                        if (x + j >= start.X && x + j <= hMax) {
                            Active.fixedObstacles.Add(((x + j, y - i), 'C', null));
                        }
                    }
                }
                break;

            case "east":
                // swap x with y from the above two cases to walk horizontally rather than vertically
                for (int i = 0; x + i <= hMax; ++i) {
                    for (int j = -i; j <= i; ++j) {
                        if (y + j >= start.Y && y + j <= vMax) {
                            Active.fixedObstacles.Add(((x + i, y + j), 'C', null));
                        }
                    }
                }
                break;

            case "west":
                for (int i = 0; x - i >= start.X; ++i) {
                    for (int j = -i; j <= i; ++j) {
                        if (y + j >= start.X && x + j <= hMax) {
                            Active.fixedObstacles.Add(((x - i, y + j), 'C', null));
                        }
                    }
                }
                break;
        }

        return;
    }

    public string[] Map(Map.Coordinates start, Map.Coordinates size) {
        // using `LinkedList<T>` here as this function only wants nodes added to
        // the head or tail of its structure
        LinkedList<string> map = new LinkedList<string>();
        List<((int, int), char, string?)> fixedObstacles = Active.fixedObstacles;


        // check for cameras and create their blocking zones -- might be more
        // performant to create a new List<T> here (shouldn't affect
        // functionality aside from redundancy in obstacle list); could also run
        // a cleanup at the end (can the char == 'C' obstacles somehow be marked
        // for garbage collection)?
        //
        //
        /**
         * relating to the above - a note:
         * -----------------------------------------------------
         *      maybe part of this is a hashset in so that
         *      if we try to insert an existing coordinate pair
         *      it can simply discard itself??
         *
         *      --> haven't thought this through so idk how
         *          it would work given the obstacle chars & the
         *          nullable string.
         * -----------------------------------------------------
         */
        if (fixedObstacles.Any(obstacle => obstacle.Item2 == 'C')) {
            var cameras =
                fixedObstacles.FindAll(obstacle => obstacle.Item2 == 'C');

            foreach (var camera in cameras) {
                if (camera.Item3 is not null) {
                    // call a camera activation function that will use a known map
                    // area to determine relevant camera blocking sections.
                    CameraBlocking(start, size, camera.Item3,
                                   camera.Item1.Item1, camera.Item1.Item2);
                }
            }
        }

        for (int i = 0; i < size.Y; ++i) {
            LinkedList<char> line =
                new LinkedList<char>();  // create a new list for each line

            // the starting (x, y) coordinates are not related to the map
            // width/height, so a separate iterator must be used to correctly
            // determine the value of each
            int y = i + start.Y;

            for (int j = 0; j < size.X; ++j) {
                int x = j + start.X;

                // bind the first matching `fixedObstacles` tuple by comparing
                // to the iterator's current cell (x,y) if there is no match,
                // bind a default value.
                ((int, int), char, string?) blocked = fixedObstacles.FirstOrDefault(
                    obstacle =>
                        obstacle.Item1.Item1 == x && obstacle.Item1.Item2 == y);

                // use the character from a bound obstacle tuple if we matched
                // the current cell to a blocked cell
                LinkedListNode<char> node =
                    (blocked != default
                         ? new LinkedListNode<char>(blocked.Item2)
                         : new LinkedListNode<char>('.'));

                line.AddLast(
                    node);  // left-to-right by appending node on the end
            }

            // push the result as a single string to the end of the main list
            // map.AddLast(string.Concat(line));
            map.AddFirst(string.Concat(
                line));  // bottom-to-top by inserting node at the top
        }

        // add the message as the first node to print it above the map
        map.AddFirst("Here is a map of obstacles in the selected region:");
        return map.ToArray();
    }
}
