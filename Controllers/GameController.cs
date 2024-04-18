using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minesweeper_Milestone350.Models;
using System.Text.Json;

namespace Minesweeper_Milestone350.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Board board;

        public GameController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Check if the board state exists in session
            var savedBoardJson = _httpContextAccessor.HttpContext.Session.GetString("boardState");
            if (!string.IsNullOrEmpty(savedBoardJson))
            {
                // If board state exists, load it
                board = new Board(10); // Initialize an empty board
                board.Grid = DeserializeBoard(savedBoardJson); // Populate the board's Grid property
            }
            else
            {
                // Otherwise, create a new board
                board = new Board(10);
                board.SetDifficulty(1);
                board.CalculateLiveNeighbors();

                // Store the new board state in session
                _httpContextAccessor.HttpContext.Session.SetString("boardState", SerializeBoard(board.Grid));
            }
        }

        private string SerializeBoard(Cell[,] board)
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

        private Cell[,] DeserializeBoard(string json)
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


        // GET: /Game
        public IActionResult Index()
        {
            return View(board.Grid);
        }

        // Handle button click
        [HttpPost]
        public IActionResult HandleButtonClick(int row, int col)
        {

            // Check if the clicked cell is a bomb
            if (board.Grid[row, col].IsMine)
            {
                // Player clicked on a bomb, game over
                return RedirectToAction("GameOver");
            }

            // Update the clicked cell
            if (!board.Grid[row, col].Visited)
            {
                board.FloodFill(row, col); // Perform flood fill algorithm
            }

            // Store the updated board state in session
            _httpContextAccessor.HttpContext.Session.SetString("boardState", SerializeBoard(board.Grid));

            // Check if the game is won
            if (!board.CheckEndGame())
            {
                // All non-mine cells have been visited, game won
                return RedirectToAction("GameWon");
            }

            // Redirect to the Index action after handling the button click
            return RedirectToAction("Index");
        }

        // Game over action
        public IActionResult GameOver(TimeSpan totalTime)
        {

            // Reset the game by clearing the session and initializing a new board
            _httpContextAccessor.HttpContext.Session.Clear();
            InitializeBoard();
            // You can add additional logic here, such as displaying a message or score
            return View();
        }

        // Game won action
        public IActionResult GameWon(TimeSpan totalTime)
        {

            // Reset the game by clearing the session and initializing a new board
            _httpContextAccessor.HttpContext.Session.Clear();
            InitializeBoard();
            // You can add additional logic here, such as displaying a message or score
            return View();
        }


    }
}
