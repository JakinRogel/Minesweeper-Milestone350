using Minesweeper_Milestone350.Models;

namespace RegisterAndLoginApp.Services
{
    public class SecurityService
    {
        // Instance of SecurityDAO to interact with the data access layer
        SecurityDAO securityDAO = new SecurityDAO();

        // Method to validate if a user is valid based on their login information
        public bool IsValid(UserLoginModel user)
        {
            // Call the FindUserByNameAndPassword method in SecurityDAO to check user credentials
            return securityDAO.FindUserByNameAndPassword(user);
        }
    }
}

