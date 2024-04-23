namespace TicTacToe {
    class Program {
        /// <summary>
        /// The entry point of the program. Do not change this method.
        /// </summary>
        static void Main(string[] args) {
            Console.WriteLine("===========================");

            Console.WriteLine("Enter the 3x3 board below:");
            string[] lines = new string[3];
            for (int i = 0; i < 3; i++) {
                lines[i] = Console.ReadLine();
            }
            Game game = new Game(lines);
            if (game.Board.IsValid()) {
                Console.WriteLine(game.HasWinner()
                                      ? $"The winner is: {game.GetWinner()}!"
                                      : "There is no winner!");
            } else {
                Console.WriteLine("The board is invalid!");
            }
            Console.WriteLine("===========================");
        }
    }
}
