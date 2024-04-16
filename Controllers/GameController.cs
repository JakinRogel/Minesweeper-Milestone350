using Microsoft.AspNetCore.Mvc;
using Minesweeper_Milestone350.Models;
using Minesweeper_Milestone350.Services;
using System.Collections.Generic;

namespace Minesweeper_Milestone350.Controllers
{
    public class GameController : Controller
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        public IActionResult Index()
        {
            List<ButtonModel> buttons = _gameService.InitializeButtons();
            return View("Index", buttons);
        }

        public IActionResult HandleButtonClick(string buttonNumber)
        {
            int bN = int.Parse(buttonNumber);
            List<ButtonModel> buttons = _gameService.InitializeButtons();
            _gameService.HandleButtonClick(buttons, bN);

            return View("Index", buttons);
        }
    }
}

