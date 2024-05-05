namespace a2cs;

class Guard {
    // push a single (x,y) cell instance to the obstacles dict
    public static void Add(int x, int y) {
        Obstacle.New(x, y, 'G', null);
    }
}
