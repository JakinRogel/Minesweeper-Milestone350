using Microsoft.AspNetCore.Mvc;
using Minesweeper_Milestone350.Models;

namespace Minesweeper_Milestone350.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessLogin(UserLoginModel user) 
        {
            bool success = true;
            if(success)
            {
                return View("LoginSuccess", user);
            } else
            {
                return View("LoginFialure", user);
            }
        }
    }
}
