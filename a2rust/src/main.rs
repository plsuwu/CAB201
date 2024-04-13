use std::io;

const WELCOME: &str = "Welcome to the Threat-o-tron 9000 Obstacle Avoidance System.";
const PROMPT: &str = "Enter command:";
const EXIT: &str = "Thank you for using the Threat-o-tron 9000.";
const HELP: &str = "
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

fn main() {
    println!("{}", WELCOME);
    println!("{}", HELP);

    loop {
        let mut input = String::new();
        println!("{}", PROMPT);

        io::stdin().read_line(&mut input).unwrap();
        let input = input.trim();

        let args = input.trim().split(" ").collect::<Vec<_>>();
        if input == "exit" {
            println!("{}", EXIT);
            break;
        }

        if input == "help" {
            println!("{}", HELP);
        }

        println!("{:?}", args);
    }
}
