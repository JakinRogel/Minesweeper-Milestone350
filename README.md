# Minesweeper - Milestone CST-350
The Minesweeper Milestone Project is a web app developed with Visual Studio Code 2022 and ASP.NET Core MVC. It offers a Minesweeper game with user registration, login, CSS-styled game board, and advanced features like AJAX updates, flag planting, and game saving. Built for smooth gameplay and code cleanliness.



# Minesweeper Milestone Project Readme

## 1. Application Requirements

### Registration Form:
- Build a registration form with the following fields:
  - First Name
  - Last Name
  - Sex
  - Age
  - State
  - Email Address
  - Username
  - Password
- Utilize HTTP POST to send form data to a controller.
- Implement server-side form validation.
- Save form data in a SQL Server relational database.
- Redirect form submission results to a success or error page.

### Login Form:
- Develop a login form with the following fields:
  - Username
  - Password
- Authenticate users against the SQL Server database.
- Redirect login results to a success or error page.

## 2. Additional Features

- Integrate login and registration functionality into the Main Menu.
- Display the game board upon successful login.
- Apply CSS and images to the game board for visual appeal.
- Implement button clicks to update game state.
- Utilize backend controllers and services for game logic.

## 3. Minesweeper Game Board Redesign

- Redesign the game board using AJAX and partial page updates.
- Display a timestamp to demonstrate AJAX updates.
- Encapsulate game logic into a backend service class.
- Implement right-click functionality with jQuery.
- Show game over message for success and failure cases.

## 4. New Application Features

- Implement "Save Game" and "Show Saved Games" functionality.
- Serialize game state and user information into the database.
- Create REST endpoints to display, delete, and load saved games.
- No security features are implemented for the API.

## 5. Application Details

- Refactor code based on feedback and reviews.
- Utilize IoC dependency injection for flexibility.
- Implement HTTP Request filters for page security.

## Project Structure

- **/Controllers**: Contains controller classes.
- **/Models**: Stores model classes for data representation.
- **/Views**: Contains view files for user interface rendering.
- **/Scripts**: Stores JavaScript files for frontend logic.
- **/Styles**: Contains CSS files for styling.
- **/Data**: Stores database-related files.
- **/Utilities**: Contains utility classes and functions.
- **/Tests**: Stores unit tests for the application.

## Setup Instructions

1. Clone the repository.
2. Open the project in Visual Studio Code 2022.
3. Set up the SQL Server database using Microsoft SQL Server Data Tools.
4. Configure connection settings in the application.
5. Run the application using Microsoft Web Developer Tools.
6. Access the application via the provided URL.

## Technologies Used

- Frontend: HTML, CSS, JavaScript (jQuery)
- Backend: ASP.NET Core MVC
- Database: SQL Server
- Other: AJAX, IoC Dependency Injection

## Contributors

- Jakin Rogel (@JakinRogel)
- Tony Brown (@)
- Miguel Gallegos (@)

## License

This project is licensed under the MIT License - see the LICENSE file for details.
