using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Minesweeper_Milestone350.Models;
using RegisterAndLoginApp.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Minesweeper_Milestone350.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProcessLogin(UserLoginModel user)
        {
            SecurityService securityService = new SecurityService();

            if (securityService.IsValid(user))
            {
                // Create claims for the authenticated user
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            // Add more claims as needed
        };

                // Create claims identity
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Create authentication properties
                var authProperties = new AuthenticationProperties
                {
                    // You can customize authentication properties here
                };

                // Sign in the user
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirect to the game page
                return RedirectToAction("Index", "Game");
            }
            else
            {
                return View("LoginFailure", user);
            }
        }

    }
}
