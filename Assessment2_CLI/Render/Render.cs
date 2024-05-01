namespace a2cs;

// could be cleaner but im taking a working implementation for now
class Render {
    // maybe pull this out into `Obstacle.cs`
    public static void CameraBlocking(Map.Coordinates start,
                                      Map.Coordinates size, string direction,
                                      int x, int y) {
        int hMax =
           Math.Min(start.X, x) + Math.Max(size.X - 1, x);  // represents the maximum horizontal distance
                                   // for a horizontally-oriented camera
        int vMax = Math.Min(start.Y, y) + Math.Max(size.Y - 1, y);  // vertically-oriented counterpart of the above binding

        switch (direction) {
            case "north":

                // vertical iterator increments y coordinate (ie, walks north)
                // until the top of the map is reached
                for (int i = 0; y + i <= vMax; ++i) {
                    // for each upward iteration, incrementally offset the
                    // horizontal vision either side of the camera center;
                    // reading from bottom to top:
                    //
                    // ```
                    //
                    //      .ccc|c|ccc    // j -> iteration 3 (and so on...)
                    //      ..cc|c|cc.    // j -> iteration 2; new `C` @
                    //      [(3,2),(4,2),(5,2),(6,2),(7,2)]
                    //      ...c|c|c..    // j -> iteration 1; new `C` @
                    //      [(4,1),(5,1),(6,1)]
                    //      ....|c|...    // j -> iteration 0; new `C` @ [(5,0)]
                    //           ^
                    //           camera center
                    // ```
                    //
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

    public string[] Map(Map.Coordinates start, Map.Coordinates size) {
        // using `LinkedList` to build a FILO/stack structure
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
         *          nullable string - hashset of a list perhaps?
         *
         *      --> alternatively, we just check to see if that location
         *          already has a camera instance, only pushing
         *          the coordinates if not.
         * -----------------------------------------------------
         */

        // if there are any camera obstacles in the active obstacles list, get
        // their coordinates
        if (fixedObstacles.Any(obstacle => obstacle.Item2 == 'C')) {
            var cameras =
                fixedObstacles.FindAll(obstacle => obstacle.Item2 == 'C');

            foreach (var camera in cameras) {
                if (camera.Item3 is not
                        null) {  // if the camera has a direction in its list
                                 // entry, it is the base camera location

                    // call the above camera activation method that will use a
                    // known map area to determine relevant blocking locations.
                    CameraBlocking(start, size, camera.Item3,
                                   camera.Item1.Item1, camera.Item1.Item2);

                    // avoid nulling `camera.Item3` here so a render can still
                    // be correctly generated if a new render is called with a
                    // larger area
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
