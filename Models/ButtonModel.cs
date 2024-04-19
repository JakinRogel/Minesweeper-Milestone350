namespace Minesweeper_Milestone350.Models
{
    public class ButtonModel
    {
        public int Id { get; set; }

        public int ButtonState { get; set; }

        public ButtonModel()
        {
        }

        public ButtonModel(int id, int buttonState)
        {
            Id=id;
            ButtonState=buttonState;
        }
       
    }
}
