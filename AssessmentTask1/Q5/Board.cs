namespace TicTacToe {
    class Game {
        private Board? board;
        public Board Board { get; set; }

        public Game(string[] lines) {
            Board = new Board(lines);
        }

        // check for a winner, return true if there is a winner otherwise false.
        public bool HasWinner() {
            List<List<char>> board = Board.board;
            List<char> diagCharsLR = new List<char>();
            List<char> diagCharsRL = new List<char>();
            string[] containsWin = ["XXX", "OOO"];

            // rows
            for (var i = 0; i < board.Count; ++i) {
                string row = String.Concat(board[i]);

                if (containsWin.Contains(row)) {
                    return true;
                }

                List<char> colChars = new List<char>();

                for (var j = 0; j < board[i].Count; ++j) {
                    colChars.Add(board[j][i]);

                    if (colChars.Count == board.Count &&
                        containsWin.Contains(String.Concat(colChars))) {
                        return true;
                    }

                    if (i == j) {
                        diagCharsLR.Add(board[i][j]);
                    }

                    if (i == 0 && j == (board[i].Count - 1) ||
                        j == 0 && (i == board.Count - 1) ||
                        ((board[i].Count - 1) / 2) == i &&
                            ((board[i].Count - 1) / 2) == j) {
                        diagCharsRL.Add(board[i][j]);
                    }
                }
            }

            if (containsWin.Contains(String.Concat(diagCharsLR)) ||
                containsWin.Contains(String.Concat(diagCharsRL))) {
                return true;
            }

            return false;
        }

        // literally the same as `HasWinner` method but returns a string
        // containing the character of the winning side
        public string GetWinner() {
            List<List<char>> board = Board.board;
            List<char> diagCharsLR = new List<char>();
            List<char> diagCharsRL = new List<char>();
            string[] containsWin = ["XXX", "OOO"];

            // rows
            for (var i = 0; i < board.Count; ++i) {
                string row = String.Concat(board[i]);

                if (containsWin.Contains(row)) {
                    return String.Concat(row[0]);
                }

                List<char> colChars = new List<char>();

                for (var j = 0; j < board[i].Count; ++j) {
                    colChars.Add(board[j][i]);

                    if (colChars.Count == board.Count &&
                        containsWin.Contains(String.Concat(colChars))) {
                        return String.Concat(colChars[0]);
                    }

                    if (i == j) {
                        diagCharsLR.Add(board[i][j]);
                    }

                    if (i == 0 && j == (board[i].Count - 1) ||
                        j == 0 && (i == board.Count - 1) ||
                        ((board[i].Count - 1) / 2) == i &&
                            ((board[i].Count - 1) / 2) == j) {
                        diagCharsRL.Add(board[i][j]);
                    }
                }
            }

            if (containsWin.Contains(String.Concat(diagCharsLR))) {
                return String.Concat(diagCharsLR[0]);
            }

            if (containsWin.Contains(String.Concat(diagCharsRL))) {
                return String.Concat(diagCharsRL[0]);
            }

            throw new Exception();
        }
    }
}

class Board {
    public List<List<char>> board;

    /// <summary>
    /// Constructor of the Board class. Fill the 2D board of characters
    /// using the lines array of strings.
    /// </summary>
    /// <param name="lines">The array of strings to fill the 2D board of
    /// characters</param>
    public Board(string[] lines) {
        board = new List<List<char>>();

        for (var i = 0; i < lines.Length; ++i) {
            List<char> row = new List<char>();

            for (var j = 0; j < lines[i].Length; ++j) {
                row.Add(lines[i][j]);
            }
            board.Add(row);
        }
    }

    /// <summary>
    /// Get whether the board is valid, i.e. the board is a 3x3 board with
    /// only 'X', 'O' and '.' characters and the number of 'X' is equal to
    /// or one more than the number of 'O'
    /// </summary>
    /// <returns>True if the board is valid, false otherwise</returns>
    public bool IsValid() {
        int xs = 0;
        int os = 0;

        // is it even possible for `board.Count` to not be 3??
        for (var i = 0; i < board.Count; ++i) {
            if (board.Count != 3 || board[i].Count != 3) {
                return false;
            }

            for (var j = 0; j < board[i].Count; ++j) {
                char c = board[i][j];

                switch (c) {
                    case 'X':
                        ++xs;
                        break;

                    case 'O':
                        ++os;
                        break;

                    case '.':
                        break;

                    default:
                        return false;
                }
            }
        }

        if (xs != os && xs != os + 1) {
            return false;
        }

        return true;
    }
}
