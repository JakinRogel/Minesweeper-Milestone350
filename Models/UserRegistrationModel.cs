using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Minesweeper_Milestone350.Models
{
    public class UserRegistrationModel
    {
        [Required]
        [DisplayName("Please enter your first name.")]
        public string firstName { get; set; }
        [Required]
        [DisplayName("Please enter your first name.")]
        public string lastName { get; set; }
        [Required]
        [DisplayName("Please enter your gender.")]
        public string gender { get; set; }
        [Required]
        [DisplayName("Please enter your age.")]
        public int userAge {get; set; }
        [Required]
        [DisplayName("Please enter your state. (example: Arizona)")]
        public string state { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("Please enter a valid email address.")]
        public string emailAddress { get; set; }
        [Required]
        [DisplayName("Please enter your username.")]
        public string userName { get; set; }
        [Required]
        [DisplayName("Please enter a strong password.")]
        public string password { get; set; }

        public UserRegistrationModel()
        {
        }

        public UserRegistrationModel (string firstName, string lastName, string gender, int userAge, string state, string emailAddress, string userName, string password)
        {
            
            this.firstName=firstName;
            this.lastName=lastName;
            this.gender=gender;
            this.userAge=userAge;
            this.state=state;
            this.emailAddress=emailAddress;
            this.userName=userName;
            this.password=password;
        }
    }
}
