namespace Minesweeper_Milestone350.Models
{
    public class ButtonModel
    {
        // Property to represent the unique identifier of the button
        public int Id { get; set; }

        // Property to represent the state of the button (e.g., pressed, unpressed, etc.)
        public int ButtonState { get; set; }

        // Default constructor
        public ButtonModel()
        {
        }

        // Parameterized constructor to initialize a new button model with specified id and state
        public ButtonModel(int id, int buttonState)
        {
            Id = id;
            ButtonState = buttonState;
        }
    }
}

