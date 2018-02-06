///===============================================================================================================================
///   Namespace: CheckersBlizzardtest
///   Description: Checkers game represented by ASCII char on the Windows Console.  Made for Activision Blizzard interview.
///   Author: Juno Kim
///   Date: 02/04/2018
///   Notes: Other information regarding the test in the included README.txt
///================================================================================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersBlizzardtest
{
    class Program
    {
        static void Main()
        {
            displayIntro();
            bool playing = true; 
            while (playing)
            {
                gameLoop();
                Console.WriteLine("Would you like to PLAY AGAIN??? (enter y)");
                if (Console.ReadLine() != "y" || Console.ReadLine() != "Y")
                {
                    playing = false;
                }
            }
            Console.WriteLine("Thanks for playing! :)");
        }

        /// <summary>
        /// Displays the Checker Ascii art
        /// </summary>
        static void displayIntro()
        {
            Console.Write("    XXXXX  XX    XX XXXXXX   XXXXX  XX   XX XXXXXX XXXXXX\n   XXXXXXX XX    XX XX      XXXXXXX XX  XX  XX     XX   XX\n  XX|      XXXXXXXX XXXXXX XX|      XXXX    XXXXXX XX   XX\n  XX|      XXXXXXXX XX     XX|      XXXX    XX     XXXXXX\n   XXXXXXX XX    XX XX      XXXXXXX XX  XX  XX     XX   XX\n    XXXXX  XX    XX XXXXXX   XXXXX  XX   XX XXXXXX XX   XXX\n============================================================");
            Console.WriteLine("\n                   Press Any Key To Start!");
            Console.ReadKey();
        }

        /// <summary>
        /// the main gameLoop
        /// Will loop until there are no valid moves for either X or O.
        /// </summary>
        static private void gameLoop()
        {
            //creates the game
            Game game = startNewGame();
            bool active = true;
            int turnCount = 0;
            while (active) //Game Loop
            {
                turnCount++; //keeps track of turn number
                Console.WriteLine("$$$$$$$$$$--Turn " + turnCount + "--$$$$$$$$$$");
                active = game.checkForValidMoves(1); //checks if there are valid moves for O then breaks loop if there are none.
                if (active == false)
                {
                    break;
                }
                bool moveValid = false;
                while (!moveValid) //Move Loop
                {
                    int[] square = null; //is null until a valid square is chosen. Loops until square != null
                    char dir = '0'; //is 0 until l or r is chosen.  Loops until dir != 0.
                    printBoard(game);
                    while (square == null) //Square Selection
                    {
                        square = selectSquare();
                        if (square != null)
                        {
                            if (!game.checkSquarePiece(square[0], square[1], 1)) // checks to see if the piece player selected is their own. 
                            {
                                square = null;
                                Console.WriteLine("---- You don't have a piece on that square");
                            }
                        }
                        else
                        {
                            printBoard(game);
                        }
                    }
                    printBoard(game, square);
                    Console.WriteLine("---- enter 'L' to go left || enter 'R' to go right");
                    while (dir == '0') //Direction Selection
                    {
                        dir = selectDir();
                    }
                    moveValid = game.Move(square[0], square[1], dir);
                }
                printBoard(game);
                active = game.checkForValidMoves(-1);
                Console.WriteLine("Computer::: Thinking Emoji..............");
                game.doAIMove(); //AI moves
            }
            Console.WriteLine("GAME OVER");
        }

        static private Game startNewGame()
        {
            Game board = new Game();
            board.resetBoard();
            return board;
        }

        /// <summary>
        /// Prompts player to choose a piece
        /// Parses the line entered and checks if the values they chose are between 1 and 8
        /// Subtracts 1 from each value to make transferring values to arrays easier later
        /// </summary>
        /// <returns>returns a int[2] if valid else returns null</returns>
        static private int[] selectSquare()
        {
            Console.WriteLine("---- Choose Your Minion!");
            Console.WriteLine("---- (Enter Row# and Column# separated by a space)");
            string line = Console.ReadLine();
            string[] tokens = line.Split();
            try
            {
                int[] square = Array.ConvertAll(tokens, int.Parse);
                //Checks if there are only 2 values and that they're both between 1 and 8
                if (square.Length == 2)
                {
                    if (Array.TrueForAll(square, x => ((x <= 8) && (x > 0))))
                    {
                        square[0] = square[0] - 1;
                        square[1] = square[1] - 1;
                        return square;
                    }
                }
                Console.WriteLine("---- Invalid Selection");
                return null;
            }
            catch (FormatException e)
            {
                Console.WriteLine("---- Invalid Selection");
                return null;
            }
        }

        /// <summary>
        /// Prompts player to choose which direction to move a piece and checks if the value they entered is left or right.
        /// In this version of checkers pieces only have max of two decisions.
        /// </summary>
        /// <returns>returns r for right, l for left, 0 for neither and will prompt to select again</returns>
        static private char selectDir()
        {
            string line = Console.ReadLine();
            if (line == "r" || line == "R")
            {
                return 'r';
            }
            else if (line == "l" || line == "L")
            {
                return 'l';
            }
            Console.WriteLine("Invalid Selection");
            return '0';
        }

        /// <summary>
        /// Displays the current board on Console
        /// Uses the game class's _boardStatus and writes characters accordingly
        /// </summary>
        /// <param name="board"></param>
        static private void printBoard(Game board)
        {
            int[,] boardStatus = board.getBoard();
            Console.WriteLine(" __________________ ");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(i + 1);
                Console.Write("|");
                for (int j = 0; j < 8; j++)
                {
                    switch (boardStatus[i, j])
                    {
                        case -1:
                            Console.Write("X");
                            break;
                        case 1:
                            Console.Write("O");
                            break;
                        default:
                            Console.Write(".");
                            break;
                    }
                    Console.Write(" ");
                }
                Console.Write("|");
                Console.Write("\n");
            }
            Console.WriteLine(" ------------------ \n- 1 2 3 4 5 6 7 8 -");
        }

        /// <summary>
        /// Specific to display a "!" to notify players which square was selected
        /// </summary>
        /// <param name="board"></param>
        /// <param name="selectedSquare"></param>
        static private void printBoard(Game board, int[] selectedSquare)
        {
            int[,] boardStatus = board.getBoard();
            Console.WriteLine(" __________________ ");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(i + 1);
                Console.Write("|");
                for (int j = 0; j < 8; j++)
                {
                    if (i == selectedSquare[0] && j == selectedSquare[1])
                    {
                        Console.Write("!");
                    }
                    else
                    {
                        switch (boardStatus[i, j])
                        {
                            case -1:
                                Console.Write("X");
                                break;
                            case 1:
                                Console.Write("O");
                                break;
                            default:
                                Console.Write(".");
                                break;
                        }
                    }
                    Console.Write(" ");
                }
                Console.Write("|");
                Console.Write("\n");
            }
            Console.WriteLine(" ------------------ \n- 1 2 3 4 5 6 7 8 -");
        }
    }

    /// <summary>
    /// Game Class.  Contains the gameplay info for the main Program
    /// </summary>
    class Game
    {
        private int[,] _boardStatus; //2D array that represents the board. 1 = O, -1 = X, 0 = .;
        private int Oalive; //number of alive O pieces
        private int Xalive; //number of alive X pieces
        private List<Tuple<int, int>> OList; //location of all O pieces
        private List<Tuple<int, int>> XList; //locations of all X pieces

        /// <summary>
        /// default Constructor
        /// </summary>
        public Game()
        {
            _boardStatus = new int[8, 8];
        }

        /// <summary>
        /// resets the board to original status
        /// will be used to set up new games
        /// </summary>
        public void resetBoard()
        {
            Oalive = 12;
            Xalive = 12;
            // Sets checker pieces
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i < 3 && (i + j) % 2 == 1)
                    {
                        _boardStatus[i, j] = -1;
                    }
                    else if (i > 4 && (i + j) % 2 == 1)
                    {
                        _boardStatus[i, j] = 1;
                    }
                }
            }
            OList = findAlivePieces(1);
            XList = findAlivePieces(-1);
        }

        /// <summary>
        /// Used by main program to try to move a piece
        /// Will also check if the move is valid.
        /// Returns false, if the move was invalid and prompt player to try again.
        /// </summary>
        /// <param name="row">piece row value</param>
        /// <param name="col">piece col value</param>
        /// <param name="dir">direction piece is trying to move</param>
        /// <returns>true if Move is valid, false if move is invalid</returns>
        public bool Move(int row, int col, char dir)
        {
            int preCheck = validateMove(row, col, dir);
            int race = _boardStatus[row, col];
            switch (preCheck)
            {
                case 0:
                    Console.WriteLine("---- Piece is Blocked.  Try a different strategy.");
                    return false;
                case 1:
                    movePiece(row, col, dir, false);
                    return true;
                case 2:
                    Console.WriteLine(" ---- Nice Jumping Skills!!!");
                    movePiece(row, col, dir, true);
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Confirms if a specific color is on the square specified with row and col.
        /// </summary>
        /// <param name="row">row value</param>
        /// <param name="col">col value</param>
        /// <param name="race">-1 is X, 1 is O</param>
        /// <returns></returns>
        public bool checkSquarePiece(int row, int col, int race)
        {
            if (_boardStatus[row, col] == race)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get function for board status
        /// </summary>
        /// <returns>int[8,8] status of the gameboard</returns>
        public int[,] getBoard()
        {
            return _boardStatus;
        }

        /// <summary>
        /// Checks if any moves are available for specific color
        /// Used to determine whether to end game.
        /// Returns whether there's a move still available to player or AI
        /// </summary>
        /// <param name="race"></param>
        /// <returns></returns>
        public bool checkForValidMoves(int race)
        {
            if (listValidMoves(race).Count > 0)
            {
                return true;
            }
            else if (race == 1)
            {
                Console.WriteLine("---- X is the Winner!@#$!");
            }
            else if (race == -1)
            {
                Console.WriteLine("---- O is the Winner!@#$!");
            }
            return false;
        }

        /// <summary>
        /// checks if any moves are availabe for either player
        /// </summary>
        /// <returns></returns>
        public bool checkForValidMoves()
        {
            if (listValidMoves(1).Count > 0 && listValidMoves(-1).Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Main Program calls this to do enemy turn.
        /// Does the first move from listValidMoves() which will prioritize jumps.
        /// If no moves exist, AI loses.
        /// </summary>
        public void doAIMove()
        {
            List<Tuple<int, int, char>> AIMoves = listValidMoves(-1);
            try
            {
                Move(AIMoves[0].Item1, AIMoves[0].Item2, AIMoves[0].Item3);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("(╯°□°）╯︵ ┻━┻ I have no moves left!┻━┻ ︵ ヽ(°□°ヽ)"); //Note to self. These Ascii don't work.  Still is replaced with ? and looks cool
            }
        }

        /// <summary>
        /// Method to list all available moves for a player.
        /// Uses validateMove
        /// </summary>
        /// <param name="race">1 for O, -1 for X</param>
        /// <returns>a list of Tuple of pieces and directions it can move </returns>
        private List<Tuple<int, int, char>> listValidMoves(int race)
        {
            List<Tuple<int, int>> aliveList;
            List<Tuple<int, int, char>> moveList = new List<Tuple<int, int, char>>();  //Keeps regular moves separate from jumps
            List<Tuple<int, int, char>> returnList = new List<Tuple<int, int, char>>();
            //Picks which color to check
            switch (race)
            {
                case 1:
                    OList = findAlivePieces(1);
                    aliveList = OList;
                    break;
                case -1:
                    XList = findAlivePieces(-1);
                    aliveList = XList;
                    break;
                default:
                    throw new FormatException();
            }
            //checks each piece on board for that color to see if it can move left and right
            foreach (Tuple<int, int> t in aliveList)
            {
                char[] directions = new char[2] { 'l', 'r' };
                foreach (char c in directions)
                {
                    int result = validateMove(t.Item1, t.Item2, c);
                    if (result == 1)
                    {
                        moveList.Add(new Tuple<int, int, char>(t.Item1, t.Item2, c));
                    }
                    else if (result == 2)
                    {
                        returnList.Add(new Tuple<int, int, char>(t.Item1, t.Item2, c)); //Jumps are priority above regular moves
                    }
                }
            }
            moveList = shuffler(moveList); //Shuffles list to randomize actions
            foreach (Tuple<int, int, char> s in moveList)
            {
                returnList.Add(s); //Adds the moveList to the returnList but still after the Jumps if there were any.
            }
            return returnList;
        }

        /// <summary>
        /// Find all pieces that are alive for a specific color and store their coordinates in a list
        /// </summary>
        /// <param name="race"></param>
        /// <returns>Tuples of alive pieces' coordinates</returns>
        private List<Tuple<int, int>> findAlivePieces(int race)
        {
            List<Tuple<int, int>> returnList = new List<Tuple<int, int>>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_boardStatus[i, j] == race)
                    {
                        returnList.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            return returnList;
        }

        /// <summary>
        /// Helper function to check if a move is valid.  Prechecks the position a piece would be moving to and then returns an integer depending on which action can be taken.
        /// </summary>
        /// <param name="row">current square row value</param>
        /// <param name="col">current square column value</param>
        /// <param name="dir">l or r depending which direction to move</param>
        /// <returns>0 = invalid move; 1 = moves to open square; 2 = jumps an enemy</returns>
        /// TODO: change directional values to enums to not have to calculate each square brute force.
        private int validateMove(int row, int col, char dir)
        {
            int race = _boardStatus[row, col];
            int[] checkSquare = new int[2];
            if (race == 1)
            {
                if (dir == 'l')
                {
                    checkSquare[0] = row - 1;  //if race is O then you're moving up and left
                    checkSquare[1] = col - 1;
                    if (Array.TrueForAll(checkSquare, x => ((x < 8) && (x >= 0)))) //checks if both row and col values are between 0 and 7
                    {
                        if (_boardStatus[checkSquare[0], checkSquare[1]] == race)  //if the race is the same in the square, the piece is blocked
                        {
                            return 0;
                        }
                        else if (_boardStatus[checkSquare[0], checkSquare[1]] == -race)  //the space is occupied by enemy so check if jump is available
                        {
                            int[] jumpSquare = new int[2] { checkSquare[0] - 1, checkSquare[1] - 1 };
                            if (Array.TrueForAll(jumpSquare, x => ((x < 8) && (x >= 0))) && _boardStatus[checkSquare[0] - 1, checkSquare[1] - 1] == 0) //if two spaces to the diagnal is open then it can jump
                            {
                                return 2;
                            }
                            return 0; // else blocked
                        }
                        else //else the square is empty
                        {
                            return 1;
                        }
                    }
                    return 0; //out of bounds
                }
                else if (dir == 'r')
                {
                    checkSquare[0] = row - 1;  //if race is O then you're moving up and right
                    checkSquare[1] = col + 1;
                    if (Array.TrueForAll(checkSquare, x => ((x < 8) && (x >= 0)))) //checks if both row and col values are between 0 and 7
                    {
                        if (_boardStatus[checkSquare[0], checkSquare[1]] == race)  //if the race is the same in the square, the piece is blocked
                        {
                            return 0;
                        }
                        else if (_boardStatus[checkSquare[0], checkSquare[1]] == -race)  //the space is occupied by enemy so check if jump is available
                        {
                            int[] jumpSquare = new int[2] { checkSquare[0] - 1, checkSquare[1] + 1 };
                            if (Array.TrueForAll(jumpSquare, x => ((x < 8) && (x >= 0))) && _boardStatus[checkSquare[0] - 1, checkSquare[1] + 1] == 0) //if two spaces to the diagnal is open then it can jump
                            {
                                return 2;
                            }
                            return 0; // else blocked
                        }
                        else //else the square is empty
                        {
                            return 1;
                        }
                    }
                    return 0; //out of bounds
                }
            }
            if (race == -1) //if X you're just moving down instead
            {
                if (dir == 'l')
                {
                    checkSquare[0] = row + 1;  //if race is O then you're moving down and left
                    checkSquare[1] = col - 1;
                    if (Array.TrueForAll(checkSquare, x => ((x < 8) && (x >= 0)))) //checks if both row and col values are between 0 and 7
                    {
                        if (_boardStatus[checkSquare[0], checkSquare[1]] == race)  //if the race is the same in the square, the piece is blocked
                        {
                            return 0;
                        }
                        else if (_boardStatus[checkSquare[0], checkSquare[1]] == -race)  //the space is occupied by enemy so check if jump is available
                        {
                            int[] jumpSquare = new int[2] { checkSquare[0] + 1, checkSquare[1] - 1 };
                            if (Array.TrueForAll(jumpSquare, x => ((x < 8) && (x >= 0))) && _boardStatus[checkSquare[0] + 1, checkSquare[1] - 1] == 0) //if two spaces to the diagnal is open then it can jump
                            {
                                return 2;
                            }
                            return 0; // else blocked
                        }
                        else //else the square is empty
                        {
                            return 1;
                        }
                    }
                    return 0; //out of bounds
                }
                else if (dir == 'r')
                {
                    checkSquare[0] = row + 1;  //if race is O then you're moving down and right
                    checkSquare[1] = col + 1;
                    if (Array.TrueForAll(checkSquare, x => ((x < 8) && (x >= 0)))) //checks if both row and col values are between 0 and 7
                    {
                        if (_boardStatus[checkSquare[0], checkSquare[1]] == race)  //if the race is the same in the square, the piece is blocked
                        {
                            return 0;
                        }
                        else if (_boardStatus[checkSquare[0], checkSquare[1]] == -race)  //the space is occupied by enemy so check if jump is available
                        {
                            int[] jumpSquare = new int[2] { checkSquare[0] + 1, checkSquare[1] + 1 };
                            if (Array.TrueForAll(jumpSquare, x => ((x < 8) && (x >= 0))) && _boardStatus[checkSquare[0] + 1, checkSquare[1] + 1] == 0) //if two spaces to the diagnal is open then it can jump
                            {
                                return 2;
                            }
                            return 0; // else blocked
                        }
                        else //else the square is empty
                        {
                            return 1;
                        }
                    }
                    return 0; //out of bounds
                }
            }
            throw new FormatException();//should only be race 1 or -1;  
        }

        /// <summary>
        /// Helper function for Move() method that changes the board itself.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="dir"></param>
        /// <param name="jumped"></param>
        /// //TODO enum directions as to not have to hardcode every calculations
        private void movePiece(int row, int col, char dir, bool jumped)
        {
            int race = _boardStatus[row, col];
            if (race == 1)
            {
                _boardStatus[row, col] = 0; //Current location now empty
                if (dir == 'l')
                {
                    if (jumped)
                    {
                        _boardStatus[row - 2, col - 2] = race;
                        _boardStatus[row - 1, col - 1] = 0; //Piece Caught
                        Xalive -= 1;
                    }
                    else
                    {
                        _boardStatus[row - 1, col - 1] = race;
                    }
                }
                if (dir == 'r')
                {
                    if (jumped)
                    {
                        _boardStatus[row - 2, col + 2] = race;
                        _boardStatus[row - 1, col + 1] = 0; //Piece Caught
                        Xalive -= 1;
                    }
                    else
                    {
                        _boardStatus[row - 1, col + 1] = race;
                    }
                }

            }
            if (race == -1)
            {
                _boardStatus[row, col] = 0; //Current location now empty
                if (dir == 'l')
                {
                    if (jumped)
                    {
                        _boardStatus[row + 2, col - 2] = race;
                        _boardStatus[row + 1, col - 1] = 0; //Piece Caught
                        Oalive -= 1;
                    }
                    else
                    {
                        _boardStatus[row + 1, col - 1] = race;
                    }
                }
                if (dir == 'r')
                {
                    if (jumped)
                    {
                        _boardStatus[row + 2, col + 2] = race;
                        _boardStatus[row + 1, col + 1] = 0; //Piece Caught
                        Xalive -= 1;
                    }
                    else
                    {
                        _boardStatus[row + 1, col + 1] = race;
                    }
                }

            }
        }

        /// <summary>
        /// Helper function to shuffle list. Used to randomize AI decision.
        /// </summary>
        /// <param name="list"></param>
        /// <returns>the same list but shuffled</returns>
        private List<Tuple<int, int, char>> shuffler(List<Tuple<int, int, char>> list)
        {
            Random rand = new Random();
            var shuffled = list.OrderBy(c => rand.Next()).Select(c => c).ToList();
            return shuffled;
        }
    }
}
