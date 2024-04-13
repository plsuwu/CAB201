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
        Coordinates agent = new Coordinates();
        Coordinates goal = new Coordinates();

        string[] rawCoordinates = args.Skip(1).ToArray();
        int[] parsedCoordinates = new int[4];

        try {
            for (int i = 0; i < rawCoordinates.Length; ++i) {
                parsedCoordinates[i] = int.Parse(rawCoordinates[i]);
                if (parsedCoordinates.Any(num => num < 0)) {
                    return "Width and height must be valid positive integers.";
                }
            }
        } catch (Exception e) {
            if (e is FormatException || e is ArgumentException) {
                return "Coordinates are not valid integers.";
            }
        }

        for (int i = 0; i < args.Length / 2; ++i) {
            coordinates[i].SetValue(agent, int.Parse(rawCoordinates[i]));
            coordinates[i].SetValue(goal, int.Parse(rawCoordinates[(i + 2)]));
        }

        Render render = new Render();
        string[] mapRender = render.Map(agent, goal);

        return string.Join("\n", mapRender);
    }
}
