
#pragma warning disable CS8604
namespace DungeonTask;

class Room {
    // you could not pay me to understand why we want a NextRoom field on Room.
    // public Room NextRoom { get; set; }
    public string? Enemy { get; set; }

    public Room(string enemy) {
        Enemy = enemy;
    }
}

class Dungeon {
    public List<Room> rooms;

    public Dungeon() {
        rooms = new List<Room>();
    }

    public void AddRoom(string enemyName) {
        rooms.Add(new Room(enemyName));
    }

    public void Reverse() {
        rooms.Reverse();
    }

    public void Display() {
        if (IsEmpty()) {
            Console.WriteLine("The dungeon is empty.");
            return;
        }
        for (var i = 0; i < rooms.Count; ++i) {
            Console.WriteLine(rooms[i].Enemy);
        }
    }

    public bool IsEmpty() {
        return rooms.Count == 0;
    }
}

class Program {
    static void ReadInputLoop(Dungeon dungeon) {
        while (true) {
            string? input = Console.ReadLine();
            if (input == "stop") {
                return;
            }
            dungeon.AddRoom(input);

            Console.WriteLine(
                "Enter the name of the enemy in the next room or enter \"stop\".");
        }
    }

    static void Main(string[] args) {
        // Keep the following lines intact
        Console.WriteLine("===========================");
        Console.WriteLine("Welcome to the dungeon!");
        Console.WriteLine(
            "If you would like to add a room, enter the name of the enemy in the room.");
        Console.WriteLine("If you would like to stop, enter \"stop\".");

        Dungeon dungeon = new Dungeon();
        ReadInputLoop(dungeon);

        Console.WriteLine(
            "You entered the dungeon and saw the enemies in this order:");
        dungeon.Display();
        Console.WriteLine(
            "You backtracked your way out of the dungeon and saw the enemies in this order:");
        dungeon.Reverse();
        dungeon.Display();

        // Keep the following line intact
        Console.WriteLine("===========================");
    }
}
