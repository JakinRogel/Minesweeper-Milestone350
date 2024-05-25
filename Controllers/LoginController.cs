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
        // Action method to display the login page
        public IActionResult Index()
        {
            return View();
        }

        // Action method to process login information asynchronously
        public async Task<IActionResult> ProcessLogin(UserLoginModel user)
        {
            // Instantiate the security service
            SecurityService securityService = new SecurityService();

            // Check if the provided user credentials are valid
            if (securityService.IsValid(user))
            {
                // Create claims for the authenticated user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    // Add more claims as needed
                };

                // Create claims identity with the authentication scheme
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Create authentication properties
                var authProperties = new AuthenticationProperties
                {
                    // You can customize authentication properties here
                };

                // Sign in the user using the created claims and authentication properties
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirect to the game page upon successful login
                return RedirectToAction("Index", "Game");
            }
            else
            {
                // If login fails, return the login failure view with the user data
                return View("LoginFailure", user);
            }
        }
    }
}

