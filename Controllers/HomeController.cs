using Microsoft.AspNetCore.Mvc;
using Minesweeper_Milestone350.Models;
using RegisterAndLoginApp.Services;
using System.Diagnostics;

namespace Minesweeper_Milestone350.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SecurityDAO securityDAO;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            securityDAO = new SecurityDAO();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet] 
        public IActionResult RegistrationPage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(UserRegistrationModel model)
        {
            // Check if the user already exists by username
            if(model.userName == "existingUser")
            //if (securityDAO.FindUserByUsername(model.userName))
            {
                ModelState.AddModelError(string.Empty, "User already exists.");
                return View("ExistingUser", model);
            }

            if (ModelState.IsValid)
            {
                // Here, you can process the registration logic
                // For example, save the user to a database
                // Call the CreateUser method in SecurityDAO to save the user to the database
                securityDAO.CreateUser(model);

                // For demonstration, let's just return a success message
                return View("ShowUserDetails", model);
            }
            else
            {
                // If the model state is not valid, return the registration page with validation errors
                return View("RegistrationPage", model);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
