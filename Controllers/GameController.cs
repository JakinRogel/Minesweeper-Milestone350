using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minesweeper_Milestone350.Models;
using Minesweeper_Milestone350.Services;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Minesweeper_Milestone350.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GameService _gameService;
        private Board _board;

        // Constructor to initialize the GameController with HTTP context accessor and game service
        public GameController(IHttpContextAccessor httpContextAccessor, GameService gameService)
        {
            _httpContextAccessor = httpContextAccessor;
            _gameService = gameService;
            InitializeBoard();
        }

        // Private method to initialize the game board using the game service
        private void InitializeBoard()
        {
            _board = _gameService.InitializeBoard();
        }

        // GET: /Game
        // Method to render the main game view
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Game/StartGame
        // Method to start the game and return the game grid partial view
        [HttpPost]
        public IActionResult StartGame()
        {
            return PartialView("Grid", _board.Grid);
        }

        // POST: /Game/HandleButtonClick
        // Method to handle a button click on the game board
        [HttpPost]
        public IActionResult HandleButtonClick(int row, int col)
        {
            _gameService.HandleButtonClick(_board, row, col);

            string status;

            // Check if the game is won or lost
            if (!_gameService.CheckEndGame(_board))
            {
                // All non-mine cells have been visited, game won
                status = "gameWon";
            }
            else if (_gameService.GameLost(_board, row, col))
            {
                // Player clicked on a bomb, game over
                status = "gameOver";
            }
            else
            {
                status = "continue";
            }

            return Content(status);
        }

        // GET: /Game/GameOver
        // Method to handle the game over scenario, resetting the board
        public IActionResult GameOver()
        {
            // Reset the game by clearing the session and initializing a new board
            _gameService.clearBoard();
            InitializeBoard();

            return PartialView("GameOver");
        }

        // GET: /Game/GameWon
        // Method to handle the game won scenario, resetting the board
        public IActionResult GameWon()
        {
            // Reset the game by clearing the session and initializing a new board
            _gameService.clearBoard();
            InitializeBoard();

            return PartialView("GameWon");
        }

        // GET: /Game/UpdatedButton
        // Method to update a button on the game board
        public IActionResult UpdatedButton(int row, int col, string mine)
        {
            _board = _gameService.updateBoard(_board, row, col, mine);
            return PartialView("UpdatedButton", _board.Grid[row, col]);
        }

        // GET: /Game/SaveGame
        // Method to save the current game state
        public IActionResult SaveGame()
        {
            _gameService.SaveGame();
            return Content("Save Game Successful");
        }

        // GET: /Game/DeleteGame
        // Method to delete a saved game by its ID
        public IActionResult DeleteGame(int id)
        {
            _gameService.deleteGame(id);
            return Content("Game Delete Successful");
        }

        // GET: /Game/LoadGame
        // Method to load saved games and return the load game view
        public IActionResult LoadGame()
        {
            List<string> savedGames = _gameService.LoadGame();
            return View("LoadGame", savedGames);
        }

        // GET: /Game/LoadSelectedGame
        // Method to load a selected saved game state
        public IActionResult LoadSelectedGame(string savedGame)
        {
            _gameService.LoadSelectedGame(savedGame);
            return RedirectToAction("Index");
        }
    }
}

