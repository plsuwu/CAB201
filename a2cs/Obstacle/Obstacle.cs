namespace a2cs;

public class Active {
    static public List<(int, int)> guards = new List<(int, int)>();

    public void Guard(int x, int y) {
        guards.Add((x,y));
    }
}
