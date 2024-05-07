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


        public GameController(IHttpContextAccessor httpContextAccessor, GameService gameService)
        {
            _httpContextAccessor = httpContextAccessor;
            _gameService = gameService;
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            _board = _gameService.InitializeBoard();
        }

        // GET: /Game
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StartGame()
        {
            return PartialView("Grid", _board.Grid);
        }


        // Handle button click
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

        // Game over action
        public IActionResult GameOver()
        {
            // Reset the game by clearing the session and initializing a new board
            _gameService.clearBoard();
            InitializeBoard();

            return PartialView("GameOver");
        }

        // Game won action
        public IActionResult GameWon()
        {
            // Reset the game by clearing the session and initializing a new board
            _gameService.clearBoard();
            InitializeBoard();

            return PartialView("GameWon");
        }

        public IActionResult UpdatedButton(int row, int col, string mine)
        {
            
            _board = _gameService.updateBoard(_board, row, col, mine);



            return PartialView("UpdatedButton", _board.Grid[row, col]);
        }

        public IActionResult SaveGame()
        {
            _gameService.SaveGame();

            return Content("Save Game Successful");
        }

        public IActionResult LoadGame()
        {
            List<string> savedGames = _gameService.LoadGame();

            return View("LoadGame", savedGames);
        }

        public IActionResult LoadSelectedGame(string savedGame)
        {

            _gameService.LoadSelectedGame(savedGame);

            return RedirectToAction("Index");
        }
    }
}
