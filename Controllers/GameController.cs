using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minesweeper_Milestone350.Models;
using Minesweeper_Milestone350.Services;
using System.Text.Json;

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
            return View(_board.Grid);
        }

        // Handle button click
        [HttpPost]
        public IActionResult HandleButtonClick(int row, int col)
        {
            _gameService.HandleButtonClick(_board, row, col);

            // Check if the game is won
            if (!_gameService.CheckEndGame(_board))
            {
                // All non-mine cells have been visited, game won
                return RedirectToAction("GameWon");
            }
            else if (_gameService.GameLost(_board, row, col))
            {
                // Player clicked on a bomb, game over
                return RedirectToAction("GameOver");
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
