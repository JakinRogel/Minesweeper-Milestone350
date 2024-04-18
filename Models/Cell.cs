// Modify the Cell class to include a property indicating whether it's a mine or not
namespace Minesweeper_Milestone350.Models
{
    public class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool Visited { get; set; }
        public bool IsMine { get; set; } // New property to indicate whether it's a mine
        public int LiveNeighbors { get; set; }

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
            Visited = false;
            IsMine = false; // Initialize as not a mine
            LiveNeighbors = 0;
        }
    }
}
