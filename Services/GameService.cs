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

        // Constructor to initialize the GameService with HTTP context accessor and security DAO
        public GameService(IHttpContextAccessor httpContextAccessor, SecurityDAO securityDAO)
        {
            _httpContextAccessor = httpContextAccessor;
            _securityDAO = securityDAO;
        }

        // Method to initialize the game board
        public Board InitializeBoard()
        {
            // Check if there is a saved board state in the session
            var savedBoardJson = _httpContextAccessor.HttpContext.Session.GetString("boardState");
            Board board;
            if (!string.IsNullOrEmpty(savedBoardJson))
            {
                // Deserialize the saved board state
                board = new Board(10); // Initialize an empty board
                board.Grid = DeserializeBoard(savedBoardJson); // Populate the board's Grid property
            }
            else
            {
                // Create a new board and set its difficulty
                board = new Board(10);
                SetDifficulty(board, 1);
                CalculateLiveNeighbors(board);

                // Store the new board state in session
                _httpContextAccessor.HttpContext.Session.SetString("boardState", SerializeBoard(board.Grid));
            }
            return board;
        }

        // Method to serialize the board's grid into a JSON string
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

        // Method to deserialize a JSON string back into a board's grid
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

        // Method to handle a button click on the board
        public void HandleButtonClick(Board board, int row, int col)
        {
            // Check if the clicked cell is a bomb
            if (board.Grid[row, col].IsMine)
            {
                // Player clicked on a bomb, game over
                return;
            }

            // Update the clicked cell if it has not been visited
            if (!board.Grid[row, col].Visited)
            {
                FloodFill(board, row, col); // Perform flood fill algorithm
            }

            // Store the updated board state in session
            _httpContextAccessor.HttpContext.Session.SetString("boardState", SerializeBoard(board.Grid));
        }

        // Method to calculate the number of live neighbors for each cell on the board
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

        // Method to check if a neighbor cell is within the bounds of the board
        private bool IsValidNeighbor(int row, int col, int size)
        {
            return row >= 0 && row < size && col >= 0 && col < size;
        }

        // Method to perform a flood fill algorithm starting from a given cell
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

        // Method to check if the game has ended
        public bool CheckEndGame(Board board)
        {
            int visitedCells = board.Grid.Cast<Cell>().Count(cell => cell.Visited && !cell.IsMine);
            int remainingInertCells = TotalInertCells(board);
            return visitedCells != remainingInertCells;
        }

        // Method to count the total number of inert (non-mine) cells on the board
        public int TotalInertCells(Board board)
        {
            return board.Grid.Cast<Cell>().Count(cell => !cell.IsMine);
        }

        // Method to set the difficulty level of the board
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

        // Method to randomly place mines on the board based on the difficulty level
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

        // Method to check if the game is lost by clicking on a mine
        public bool GameLost(Board board, int clickedRow, int clickedCol)
        {
            return board.Grid[clickedRow, clickedCol].IsMine;
        }

        // Method to update the board state after a cell is clicked
        internal Board updateBoard(Board board, int row, int col, string mine)
        {
            if (board.Grid[row, col].TimeStamp == null)
            {
                board.Grid[row, col].TimeStamp = DateTime.Now.ToString("hh:mm:ss tt");
            }

            if (mine == "true")
            {
                board.Grid[row, col].Visited = true;
            }

            // Store the new board state in session
            _httpContextAccessor.HttpContext.Session.SetString("boardState", SerializeBoard(board.Grid));

            return board;
        }

        // Method to save the current game state
        internal void SaveGame()
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            // Get the current time
            var currentTime = DateTime.Now;
            _securityDAO.SaveSerializedBoard("Username: " + userName + ", " + "Time Saved: " + currentTime + ", " + _httpContextAccessor.HttpContext.Session.GetString("boardState"));
        }

        // Method to delete a saved game by its ID
        internal void deleteGame(int id)
        {
            _securityDAO.DeleteGameBoard(id);
        }

        // Method to load saved games
        internal List<string> LoadGame()
        {
            List<string> savedGames = _securityDAO.GetSavedGames();
            return savedGames;
        }

        // Method to clear the current board state from the session
        internal void clearBoard()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

        // Method to load a selected saved game state into the session
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

