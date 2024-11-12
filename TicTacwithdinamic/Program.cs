using System.Numerics;
using System;

namespace TicTacwithdinamic
{
    public class Board
    {
        public char[,] Grid { get; set; }
        public int Size { get; set; }
        public Board(int size)
        {
            Size = size;
            Grid = new char[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid[i, j] = ' ';
                }
            }
        }
        public void Display()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Console.Write($"_{Grid[i, j]}_");
                    if (j < Size - 1)
                        Console.Write("|");
                }
                Console.WriteLine();
            }
        }
        public bool IsFull()
        {
            foreach (char c in Grid)
            {
                if (c == ' ') return false;
            }
            return true;
        }
        public bool ValidNumber(int row, int col)
        {
            return row >= 0 && row < Size && col >= 0 && col < Size;
        }
        public bool IsEmpty(int row, int col) => Grid[row, col] == ' ';
        public bool PlaceMarker(int row, int col, char marker)
        {
            if(ValidNumber(row, col) && IsEmpty(row, col))
            {
                Grid[row, col] = marker; return true;
            }
            return false;
        }

    }
    public class Player
    {
        public string Name;
        public char Marker;
        public Player(string name, char marker)
        {
            Name = name;
            Marker = marker;
        }
    }
    public class Game
    {
        private Board board;
        private Player player1;
        private Player player2;
        private Player currentPlayer;
        public char computerMarker;
        public char playerMarker;
        public bool flag = true;
        
        public Game(int size)
        {
            board = new Board(size);
            player1 = new Player("Player1", 'X');
            player2 = new Player("Player2", 'O');
            currentPlayer = player1;
        }
        private void SwitchPlayer()
        {
            currentPlayer = (currentPlayer == player1) ? player2 : player1;
        }
        public bool CheckWin()
        {
            int n = board.Size;
            char marker = currentPlayer.Marker;

            for (int i = 0; i < n; i++)
            {
                bool rowWin = true;
                for (int j = 0; j < n; j++)
                {
                    if (board.Grid[i, j] != marker)
                    {
                        rowWin = false;
                        break;
                    }
                }
                if (rowWin) return true;
            }

            for (int j = 0; j < n; j++)
            {
                bool colWin = true;
                for (int i = 0; i < n; i++)
                {
                    if (board.Grid[i, j] != marker)
                    {
                        colWin = false;
                        break;
                    }
                }
                if (colWin) return true;
            }

            bool mainDiagonalWin = true;
            for (int i = 0; i < n; i++)
            {
                if (board.Grid[i, i] != marker)
                {
                    mainDiagonalWin = false;
                    break;
                }
            }
            if (mainDiagonalWin) return true;

            bool secondaryDiagonalWin = true;
            for (int i = 0; i < n; i++)
            {
                if (board.Grid[i, n - i - 1] != marker)
                {
                    secondaryDiagonalWin = false;
                    break;
                }
            }
            return secondaryDiagonalWin;
        }
        public void Play()
        {
            while (!board.IsFull())
            {
                board.Display();
                Console.WriteLine($"{currentPlayer.Name}'s turn ({currentPlayer.Marker}). Enter row and column:");

                int row = int.Parse(Console.ReadLine()) - 1;
                int col = int.Parse(Console.ReadLine()) - 1;

                if (row >= 0 && row < board.Size && col >= 0 && col < board.Size && board.IsEmpty(row, col))
                {
                    board.PlaceMarker(row, col, currentPlayer.Marker);
                    if (CheckWin())
                    {
                        board.Display();
                        Console.WriteLine($"{currentPlayer.Name} wins!");
                        return;
                    }
                    SwitchPlayer();
                }
                else
                {
                    Console.WriteLine("Invalid move, try again.");
                }
            }

            Console.WriteLine("It's a draw!");
        }
        
        private void CompMove()
        {
            Random random = new Random();
            int row, col;
            do
            {
                row = random.Next(board.Size);
                col = random.Next(board.Size);
            } while (!board.IsEmpty(row, col));

            Console.WriteLine($"Computer chooses: Row {row + 1}, Column {col + 1}");
            board.PlaceMarker(row, col, computerMarker);
        }
        public void ChooseMarker()
        {
            Console.WriteLine("Enter your marker (X or O): ");
            char choice;

            while (true)
            {
                choice = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();

                if (choice == 'X')
                {
                    playerMarker = 'X';
                    computerMarker = '0';
                    break;
                }
                else if (choice == '0')
                {
                    
                    playerMarker = '0';
                    computerMarker = 'X';
                    
                    CompMove();
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please choose X or O.");
                }
            }

        }
        public void PlayWithComputer()
        {
          
            ChooseMarker();
            

            while (true)
            {
                board.Display();
                Console.WriteLine("Your turn: ");
                int row = int.Parse(Console.ReadLine()) - 1;
                int col = int.Parse(Console.ReadLine()) - 1;
                while (!board.ValidNumber(row, col) && board.IsEmpty(row, col))
                {
                    Console.WriteLine("Invalid input or cell is not empty. :");
                }

                board.PlaceMarker(row,col, playerMarker);

                if (CheckWin())
                {
                    board.Display();
                    Console.WriteLine("You win!");
                    break;
                }
                if (CheckWin())
                {
                    board.Display();
                    Console.WriteLine("Computer wins!");
                    break;
                }


                if (board.IsFull())
                {
                    board.Display();
                    Console.WriteLine("It's a draw!");
                    break;
                }

                CompMove();

                if (CheckWin())
                {
                    board.Display();
                    Console.WriteLine("Computer wins!");
                    break;
                }

                if (board.IsFull())
                {
                    board.Display();
                    Console.WriteLine("It's a draw!");
                    break;
                }
               
            }
        }
        public void Run()
        {
            Console.WriteLine("How many people do you want to play with 1 or 2");
            Console.WriteLine("In the case of 1 person, press 1");
            Console.WriteLine("In the case of 2 person, press 2");
            if (int.TryParse(Console.ReadLine(), out int m))
            {
                if (m == 2)
                {
                    Console.WriteLine("We start the game with 2 people ");
                    Play();
                }
                else if (m == 1)
                {
                    Console.WriteLine("We start the game computer ");
                    PlayWithComputer();

                }
                else
                {
                    Console.WriteLine("Invalid choice. Please restart the game.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input press 1 or 2");
            }
            

        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter grid size");
            int gridSize = int.Parse(Console.ReadLine());
            Game game = new Game(gridSize);
           game.Run();
        }
    } 
}
