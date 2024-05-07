using Minesweeper_Milestone350.Models;
using RegisterAndLoginApp.Services;
using System;
using System.Linq;
using System.Text.Json;

namespace Minesweeper_Milestone350.Services
{
    public class GameService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private SecurityDAO _securityDAO;

        public GameService(IHttpContextAccessor httpContextAccessor, SecurityDAO securityDAO)
        {
            _httpContextAccessor = httpContextAccessor;
            _securityDAO = securityDAO;
        }

        public Board InitializeBoard()
        {
            var savedBoardJson = _httpContextAccessor.HttpContext.Session.GetString("boardState");
            Board board;
            if (!string.IsNullOrEmpty(savedBoardJson))
            {
                board = new Board(10); // Initialize an empty board
                board.Grid = DeserializeBoard(savedBoardJson); // Populate the board's Grid property
            }
            else
            {
                board = new Board(10);
                SetDifficulty(board, 1);
                CalculateLiveNeighbors(board);

                // Store the new board state in session
                _httpContextAccessor.HttpContext.Session.SetString("boardState", SerializeBoard(board.Grid));
            }
            return board;
        }

        public string SerializeBoard(Cell[,] board)
        {
            var cellList = new List<Cell>();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    cellList.Add(board[i, j]);
                }
            }

            return JsonSerializer.Serialize(cellList);
        }

        public Cell[,] DeserializeBoard(string json)
        {
            var cellList = JsonSerializer.Deserialize<List<Cell>>(json);

            var rows = (int)Math.Sqrt(cellList.Count);
            var cols = rows; // Assuming it's a square grid

            var board = new Cell[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    board[i, j] = cellList[i * rows + j];
                }
            }

            return board;
        }



        public void HandleButtonClick(Board board, int row, int col)
            {
                // Check if the clicked cell is a bomb
                if (board.Grid[row, col].IsMine)
                {
                    // Player clicked on a bomb, game over
                    return;
                }

                // Update the clicked cell
                if (!board.Grid[row, col].Visited)
                {
                    FloodFill(board, row, col); // Perform flood fill algorithm
                }

            // Store the updated board state in session
            _httpContextAccessor.HttpContext.Session.SetString("boardState", SerializeBoard(board.Grid));

        }

            public void CalculateLiveNeighbors(Board board)
        {
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.Grid[row, col].IsMine)
                    {
                        board.Grid[row, col].LiveNeighbors = 9;

                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                int neighborRow = row + i;
                                int neighborColumn = col + j;

                                if (IsValidNeighbor(neighborRow, neighborColumn, board.Size))
                                {
                                    if (!board.Grid[neighborRow, neighborColumn].IsMine)
                                    {
                                        board.Grid[neighborRow, neighborColumn].LiveNeighbors++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool IsValidNeighbor(int row, int col, int size)
        {
            return row >= 0 && row < size && col >= 0 && col < size;
        }

        public void FloodFill(Board board, int row, int col)
        {
            if (row < 0 || row >= board.Size || col < 0 || col >= board.Size || board.Grid[row, col].Visited)
            {
                return;
            }

            board.Grid[row, col].Visited = true;

            if (board.Grid[row, col].LiveNeighbors == 0)
            {
                FloodFill(board, row - 1, col);
                FloodFill(board, row + 1, col);
                FloodFill(board, row, col - 1);
                FloodFill(board, row, col + 1);
                FloodFill(board, row - 1, col - 1);
                FloodFill(board, row - 1, col + 1);
                FloodFill(board, row + 1, col - 1);
                FloodFill(board, row + 1, col + 1);
            }
        }

        public bool CheckEndGame(Board board)
        {
            int visitedCells = board.Grid.Cast<Cell>().Count(cell => cell.Visited && !cell.IsMine);
            int remainingInertCells = TotalInertCells(board);
            return visitedCells != remainingInertCells;
        }

        public int TotalInertCells(Board board)
        {
            return board.Grid.Cast<Cell>().Count(cell => !cell.IsMine);
        }

        public void SetDifficulty(Board board, double difficulty)
        {
            if (difficulty == 1)
            {
                difficulty = 0.1;
            }
            else if (difficulty == 2)
            {
                difficulty = 0.5;
            }
            else if (difficulty == 3)
            {
                difficulty = 0.7;
            }

            SetupLiveBombs(board, difficulty);

        }

        // Update the Board class to randomly place mines on the board
        public void SetupLiveBombs(Board board, double difficulty)
        {
            Random random = new Random();
            int bombsToPlace = (int)Math.Round(board.Size * board.Size * difficulty);
            int bombsPlaced = 0;

            while (bombsPlaced < bombsToPlace)
            {
                int row = random.Next(0, board.Size);
                int col = random.Next(0, board.Size);

                if (!board.Grid[row, col].IsMine)
                {
                    board.Grid[row, col].IsMine = true;
                    bombsPlaced++;
                }
            }
        }
        public bool GameLost(Board board, int clickedRow, int clickedCol)
        {
            // Check if the clicked cell is a bomb
            return board.Grid[clickedRow, clickedCol].IsMine;
        }

        internal Board updateBoard(Board board, int row, int col, string mine)
        {
            if (board.Grid[row, col].TimeStamp == null)
            {
                board.Grid[row, col].TimeStamp = DateTime.Now.ToString("hh:mm:ss tt");
            }

            if(mine == "true")
            {
                board.Grid[row,col].Visited = true;
            }

            // Store the new board state in session
            _httpContextAccessor.HttpContext.Session.SetString("boardState", SerializeBoard(board.Grid));

            return board;
        }

        internal void SaveGame()
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            // Get the current time
            var currentTime = DateTime.Now;
            _securityDAO.SaveSerializedBoard("Username: " + userName + ", " + "Time Saved: " + currentTime + ", " + _httpContextAccessor.HttpContext.Session.GetString("boardState"));
        }

        internal List<string> LoadGame()
        {
           List<string> savedGames = _securityDAO.GetSavedGames();


            return savedGames;
        }

        internal void clearBoard()
        {
            _httpContextAccessor.HttpContext.Session.Clear();

        }

        internal void LoadSelectedGame(string savedGame)
        {
            // Find the index of the opening square bracket
            int index = savedGame.IndexOf('[');

            // Extract the substring from the opening square bracket to the end of the string
            savedGame = savedGame.Substring(index);

            _httpContextAccessor.HttpContext.Session.SetString("boardState", savedGame);
        }
    }
}

