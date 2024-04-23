using System;

namespace Q2 {
    public class Q1 {
        static void Main() {
            // Keep the following line intact
            Console.WriteLine("===========================");

            var totalEggs = 0;

            Console.WriteLine("Enter the number of chickens: ");
            int.TryParse(Console.ReadLine(), out var chickenCount);

            for (var chickenNum = 0; chickenNum < chickenCount; ++chickenNum) {
                Console.WriteLine("Eggs: ");

                if (int.TryParse(Console.ReadLine(),
                                 out var eggsFromCurrentChicken) &&
                    eggsFromCurrentChicken > 0) {
                    totalEggs += eggsFromCurrentChicken;
                };
            };

            var eggDozens = totalEggs / 12;
            var eggLeftover = totalEggs % 12;

            var eggWordTotal = totalEggs == 1 ? "egg" : "eggs";
            var eggWordLeftover = eggLeftover == 1 ? "egg" : "eggs";

            Console.WriteLine(
                $"You have {totalEggs} {eggWordTotal} which equals {eggDozens} dozen and {eggLeftover} {eggWordLeftover} left over.");

            // Keep the following lines intact
            Console.WriteLine("===========================");
        }
    }
}
