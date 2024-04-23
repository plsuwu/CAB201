using System;

namespace Q2 {
    public class Program {
        class Simulation {
            public int Period { get; set; }
            public List<Town> Towns { get; set; }

            public Simulation() {
                Towns = new List<Town>();
            }
        }

        class Town {
            public int Number { get; set; }
            public string? Name { get; set; } = string.Empty;
            public double Population { get; set; }
            public double PopulationGrowth { get; set; }
        }

        public static double RunSim(int period, string name, double pop,
                                    double growth) {
            var popCurrent = pop;

            for (var currentYear = 0; currentYear < period; ++currentYear) {
                popCurrent = (Math.Round(popCurrent * growth));
                if (popCurrent < 0) {
                    popCurrent = 0;
                }
            }

            return popCurrent;
        }

        public static void Main(string[] args) {
            // Keep the following line intact
            Console.WriteLine("===========================");

            var whyExit = string.Empty;
            Simulation simulation = new Simulation();

            Console.WriteLine("Enter the simulation period (whole years):");
            if (int.TryParse(Console.ReadLine(), out var simPeriod) &&
                simPeriod > 0) {
                simulation.Period = simPeriod;

                Console.WriteLine("Enter the number of towns:");
                if (int.TryParse(Console.ReadLine(), out var simTowns) &&
                    simTowns > 0) {
                    for (var townIdx = 0; townIdx < simTowns; ++townIdx) {
                        Town town = new Town();
                        town.Number = townIdx;

                        Console.WriteLine("Enter the name of the town:");
                        town.Name = Console.ReadLine();

                        Console.WriteLine(
                            "Enter the initial population of the town:");
                        if (double.TryParse(Console.ReadLine(), out var tPop)) {
                            town.Population = tPop;
                        }
                        Console.WriteLine("Enter the growth rate of the town:");
                        if (double.TryParse(Console.ReadLine(),
                                            out var tGrowthRate)) {
                            town.PopulationGrowth = tGrowthRate;
                        }

                        var simOutput =
                            RunSim(simulation.Period, town.Name,
                                   town.Population, town.PopulationGrowth);

                        Console.WriteLine(
                            $"The population of {town.Name} goes from {town.Population} to {simOutput} with growth factor {town.PopulationGrowth} after {simulation.Period} years.");

                        // simulation.Towns.Add(town);
                    }
                } else {
                    whyExit = "towns";
                }
            } else {
                whyExit = "years";
            }

            if (whyExit != "") {
                Console.WriteLine($"Simulation cancelled: no {whyExit}.");
            }
            // Keep the following lines intact
            Console.WriteLine("===========================");
        }
    }
}
