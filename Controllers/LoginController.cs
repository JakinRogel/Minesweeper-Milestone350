using Microsoft.AspNetCore.Mvc;
using Minesweeper_Milestone350.Models;
using RegisterAndLoginApp.Services;

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
            SecurityService securityService = new SecurityService();

            if(securityService.IsValid(user))
            {
                return View("LoginSuccess", user);
            } else
            {
                return View("LoginFailure", user);
            }
        }
    }
}
