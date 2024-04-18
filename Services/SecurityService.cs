using Minesweeper_Milestone350.Models;

namespace RegisterAndLoginApp.Services
{
    public class SecurityService
    {
        SecurityDAO securityDAO = new SecurityDAO();

        public bool IsValid(UserLoginModel user)
        {
            return securityDAO.FindUserByNameAndPassword(user);
        }
    }
}
