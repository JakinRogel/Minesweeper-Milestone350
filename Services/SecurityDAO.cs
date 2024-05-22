using Microsoft.Data.SqlClient;
using Minesweeper_Milestone350.Models;
using System.Text;

namespace RegisterAndLoginApp.Services
{
    public class SecurityDAO
    {
        // Connection string for connecting to the SQL database
        string connectionString = @"Server=tcp:milestoneserver.database.windows.net,1433;Initial Catalog=milestone;Persist Security Info=False;User ID=Jakin;Password=rootroot1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;";

        // Method to find a user by username and password
        public bool FindUserByNameAndPassword(UserLoginModel user)
        {
            // Assume no user is found
            bool success = false;

            // SQL statement to find the user by username and password
            string sqlStatement = "SELECT * FROM dbo.users WHERE username = @username and password = @password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                // Define the values of the placeholders in the SQL statement
                command.Parameters.Add("@USERNAME", System.Data.SqlDbType.VarChar, 50).Value = user.UserName;
                command.Parameters.Add("@PASSWORD", System.Data.SqlDbType.VarChar, 50).Value = user.Password;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // If any rows are returned, the user was found
                    if (reader.HasRows)
                    {
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return success;
        }

        // Method to create a new user in the database
        public void CreateUser(UserRegistrationModel user)
        {
            // SQL statement to insert a new user
            string sqlStatement = "INSERT INTO dbo.users (USERNAME, PASSWORD, FIRSTNAME, LASTNAME, GENDER, USERAGE, STATE, EMAILADDRESS) " +
                                 "VALUES (@USERNAME, @PASSWORD, @FIRSTNAME, @LASTNAME, @GENDER, @USERAGE, @STATE, @EMAILADDRESS)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                // Define the values of the placeholders in the SQL statement
                command.Parameters.AddWithValue("@USERNAME", user.userName);
                command.Parameters.AddWithValue("@PASSWORD", user.password);
                command.Parameters.AddWithValue("@FIRSTNAME", user.firstName);
                command.Parameters.AddWithValue("@LASTNAME", user.lastName);
                command.Parameters.AddWithValue("@GENDER", user.gender);
                command.Parameters.AddWithValue("@USERAGE", user.userAge);
                command.Parameters.AddWithValue("@STATE", user.state);
                command.Parameters.AddWithValue("@EMAILADDRESS", user.emailAddress);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        // Method to check if a user exists by username
        public bool FindUserByUsername(string username)
        {
            bool existingUser = false;

            // SQL statement to count users with the specified username
            string sqlStatement = "SELECT COUNT(*) FROM dbo.users WHERE username = @username";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@username", username);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    existingUser = count > 0;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, e.g., log it
                Console.WriteLine("An error occurred while checking for user existence: " + ex.Message);
            }

            return existingUser;
        }

        // Method to save a serialized game board to the database
        public void SaveSerializedBoard(string serializedBoard)
        {
            // SQL statement to insert the serialized game board
            string sqlStatement = "INSERT INTO GameBoards (SerializedBoard) VALUES (@SerializedBoard)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.AddWithValue("@SerializedBoard", serializedBoard);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // Handle the exception appropriately, e.g., log it
                }
            }
        }

        // Method to retrieve a serialized game board from the database by board ID
        public string GetSerializedBoard(int boardId)
        {
            string serializedBoard = null;

            // SQL statement to select the serialized board by ID
            string sqlStatement = "SELECT SerializedBoard FROM GameBoards WHERE Id = @BoardId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.AddWithValue("@BoardId", boardId);

                try
                {
                    connection.Open();
                    serializedBoard = (string)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // Handle the exception appropriately, e.g., log it
                }
            }

            return serializedBoard;
        }

        // Method to return a list of strings, each representing a saved game data entry
        public List<string> GetSavedGames()
        {
            List<string> savedGamesList = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // SQL query to select all saved game entries
                    string sqlQuery = "SELECT * FROM GameBoards"; // Adjust the query according to your table structure

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            StringBuilder gameEntryBuilder = new StringBuilder();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                gameEntryBuilder.Append(reader[i].ToString());
                                gameEntryBuilder.Append(", "); // Add a separator between fields
                            }

                            // Add the constructed game entry string to the list
                            savedGamesList.Add(gameEntryBuilder.ToString().TrimEnd(',', ' '));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving saved games: " + ex.Message);
                // Handle the exception appropriately, e.g., log it
            }

            return savedGamesList;
        }

        // Method to delete a game board from the database by game ID
        public void DeleteGameBoard(int gameId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // SQL query to delete the game board by ID
                    string sqlQuery = "DELETE FROM GameBoards WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id", gameId);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} row(s) deleted.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the game board: " + ex.Message);
                // Handle the exception appropriately, e.g., log it
            }
        }
    }
}

