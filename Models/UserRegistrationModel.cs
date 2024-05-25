using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Minesweeper_Milestone350.Models
{
    // This class represents the user registration model containing all the necessary fields for user registration.
    public class UserRegistrationModel
    {
        // The first name of the user. This field is required.
        [Required]
        [DisplayName("Please enter your first name.")]
        public string firstName { get; set; }

        // The last name of the user. This field is required.
        [Required]
        [DisplayName("Please enter your last name.")]
        public string lastName { get; set; }

        // The gender of the user. This field is required.
        [Required]
        [DisplayName("Please enter your gender.")]
        public string gender { get; set; }

        // The age of the user. This field is required.
        [Required]
        [DisplayName("Please enter your age.")]
        public int userAge { get; set; }

        // The state where the user resides. This field is required.
        [Required]
        [DisplayName("Please enter your state. (example: Arizona)")]
        public string state { get; set; }

        // The email address of the user. This field is required and must be a valid email format.
        [Required]
        [EmailAddress]
        [DisplayName("Please enter a valid email address.")]
        public string emailAddress { get; set; }

        // The username chosen by the user. This field is required.
        [Required]
        [DisplayName("Please enter your username.")]
        public string userName { get; set; }

        // The password chosen by the user. This field is required.
        [Required]
        [DisplayName("Please enter a strong password.")]
        public string password { get; set; }

        // Default constructor
        public UserRegistrationModel()
        {
        }

        // Parameterized constructor to initialize the model with user-provided values.
        public UserRegistrationModel(string firstName, string lastName, string gender, int userAge, string state, string emailAddress, string userName, string password)
        {
            // Initializing fields with the provided values
            this.firstName = firstName;
            this.lastName = lastName;
            this.gender = gender;
            this.userAge = userAge;
            this.state = state;
            this.emailAddress = emailAddress;
            this.userName = userName;
            this.password = password;
        }
    }
}

