﻿using Microsoft.Data.SqlClient;
using Minesweeper_Milestone350.Models;


namespace RegisterAndLoginApp.Services
{
    public class SecurityDAO
    {

        string connectionString = @"Server=tcp:milestoneserver.database.windows.net,1433;Initial Catalog=milestone;Persist Security Info=False;User ID=Jakin;Password=rootroot1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;";




        public bool FindUserByNameAndPassword(UserRegistrationModel user)
        {
            //assume nothing is found
            bool success = false;

            //uses prepared statements for security. @username @password are defined below
            string sqlStatement = "SELECT * FROM dbo.users WHERE username = @username and password = @password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                //define the values of the two placeholders in the sqlStatement string
                command.Parameters.Add("@USERNAME", System.Data.SqlDbType.VarChar, 50).Value = user.userName;
                command.Parameters.Add("@PASSWORD", System.Data.SqlDbType.VarChar, 50).Value = user.password;

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

    }
}
