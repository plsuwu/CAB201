namespace a2cs;

class Guard {
    public static void Add(int x, int y) {
        Obstacle.New(x, y, 'G', null);
    }
}
