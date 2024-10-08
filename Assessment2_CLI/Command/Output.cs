namespace a2cs;

/// <summary>
/// Hides chunks of text away from logic of classes/functions
/// </summary>
public class Help {
    public const string __HELP = @"
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
