namespace a2cs;

class Map : Command.ICommand {

    public class Coordinates {
        public int X;
        public int Y;
    }

    public string Handler(string[] args) {
        if (args.Length != 5) {
            return "Incorrect number of arguments.";
        }

        var coordinates = typeof(Coordinates).GetFields();

        Coordinates southwest = new Coordinates();
        Coordinates size = new Coordinates();

        string[] rawCoordinates = args.Skip(1).ToArray();
        int[] parsedCoordinates = new int[4];

        try {
            for (int i = 0; i < rawCoordinates.Length; ++i) {
                parsedCoordinates[i] = int.Parse(rawCoordinates[i]);
            }
        } catch (Exception e) {
            if (e is FormatException || e is ArgumentException) {
                return "Coordinates are not valid integers.";
            }
        }

        for (int i = 0; i < args.Length / 2; ++i) {
            coordinates[i].SetValue(southwest, int.Parse(rawCoordinates[i]));
            coordinates[i].SetValue(size, int.Parse(rawCoordinates[(i + 2)]));
        }

        Render render = new Render();
        string[] map = render.Map(southwest, size);

        return string.Join("\n", map);
    }
}
