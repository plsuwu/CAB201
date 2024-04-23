using System;

namespace Q3 {
    public class Q3 {
        static string Validate(int month, int day) {
            int maxDays = 31; // default to most common number of days

            int[][] days = new int [2][];
            days[0] = new int[1] { 2 };            // 28 day month
            days[1] = new int[4] { 4, 6, 9, 11 };  // 30 day months

            for (var i = 0; i < days.Length; ++i) {
                if (days[i].Contains(month)) {
                    switch (i) {
                        case 0:
                            maxDays = 28;
                            break;
                        case 1:
                            maxDays = 30;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (!(Enumerable.Range(1, 12).Contains(month)) ||
                (!(Enumerable.Range(1, maxDays).Contains(day)))) {
                return "invalid";
            }

            return "valid";
        }

        static void Main() {
            // Keep the following line intact
            Console.WriteLine("===========================");

            Console.WriteLine("Enter the month:");
            int.TryParse(Console.ReadLine(), out var month);
            Console.WriteLine("Enter the day:");
            int.TryParse(Console.ReadLine(), out var day);

            var validated = Validate(month, day);

            Console.WriteLine($"Combination ({month},{day}) is {validated}.");

            // Keep the following line intact
            Console.WriteLine("===========================");
        }
    }
}
