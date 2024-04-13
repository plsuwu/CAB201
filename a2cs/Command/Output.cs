namespace a2cs;

public class Output {
    public class Info {
        public const string Welcome =
            "Welcome to the Threat-o-tron 9000 Obstacle Avoidance System.";
        public const string Prompt = "Enter command:";
        public const string Exit =
            "Thank you for using the Threat-o-tron 9000.";
        public const string Help =
            @"
Valid commands are:
add guard <x> <y>: registers a guard obstacle
add fence <x> <y> <orientation> <length>: registers a fence obstacle. Orientation must be 'east' or 'north'.
add sensor <x> <y> <radius>: registers a sensor obstacle
add camera <x> <y> <direction>: registers a camera obstacle. Direction must be 'north', 'south', 'east' or 'west'.
check <x> <y>: checks whether a location and its surroundings are safe
map <x> <y> <width> <height>: draws a text-based map of registered obstacles
path <agent x> <agent y> <objective x> <objective y>: finds a path free of obstacles
help: displays this help message
exit: closes this program
";
    }
}
