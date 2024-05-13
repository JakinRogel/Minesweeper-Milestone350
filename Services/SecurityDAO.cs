using Microsoft.Data.SqlClient;
using Minesweeper_Milestone350.Models;
using System.Text;


namespace RegisterAndLoginApp.Services
{
    public class SecurityDAO
    {

        string connectionString = @"Server=tcp:milestoneserver.database.windows.net,1433;Initial Catalog=milestone;Persist Security Info=False;User ID=Jakin;Password=rootroot1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;";




        public bool FindUserByNameAndPassword(UserLoginModel user)
        {
            //assume nothing is found
            bool success = false;

            //uses prepared statements for security. @username @password are defined below
            string sqlStatement = "SELECT * FROM dbo.users WHERE username = @username and password = @password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                //define the values of the two placeholders in the sqlStatement string
                command.Parameters.Add("@USERNAME", System.Data.SqlDbType.VarChar, 50).Value = user.UserName;
                command.Parameters.Add("@PASSWORD", System.Data.SqlDbType.VarChar, 50).Value = user.Password;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        success = true;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                };
            }
            return success;
        }

        public void CreateUser(UserRegistrationModel user)
        {
            string sqlStatement = "INSERT INTO dbo.users (USERNAME, PASSWORD, FIRSTNAME, LASTNAME, GENDER, USERAGE, STATE, EMAILADDRESS) " +
                                 "VALUES (@USERNAME, @PASSWORD, @FIRSTNAME, @LASTNAME, @GENDER, @USERAGE, @STATE, @EMAILADDRESS)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

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
        // attempting to get search for exsisting user based off database search
        public bool FindUserByUsername(string username)
        {
            bool existingUser = false;

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

        // Method to save serialized game board to the database
        public void SaveSerializedBoard(string serializedBoard)
        {
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

        // Method to retrieve the serialized game board from the database
        public string GetSerializedBoard(int boardId)
        {
            string serializedBoard = null;

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

        public void DeleteGameBoard(int gameId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
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
