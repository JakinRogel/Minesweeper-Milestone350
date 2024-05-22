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

        // Constructor to initialize the logger and SecurityDAO
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            securityDAO = new SecurityDAO();
        }

        // Action method to display the home page
        public IActionResult Index()
        {
            return View();
        }

        // Action method to display the privacy page
        public IActionResult Privacy()
        {
            return View();
        }

        // Action method to display the registration page
        [HttpGet]
        public IActionResult RegistrationPage()
        {
            return View();
        }

        // Action method to create a new user based on the registration form submission
        [HttpPost]
        public IActionResult CreateUser(UserRegistrationModel model)
        {
            // Check if the user already exists by username
            if (securityDAO.FindUserByUsername(model.userName))
            {
                ModelState.AddModelError(string.Empty, "User already exists.");
                return View("ExistingUser", model);
            }

            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Call the CreateUser method in SecurityDAO to save the user to the database
                securityDAO.CreateUser(model);

                // Return a view displaying the user details
                return View("ShowUserDetails", model);
            }
            else
            {
                // If the model state is not valid, return the registration page with validation errors
                return View("RegistrationPage", model);
            }
        }

        // Action method to handle errors
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
