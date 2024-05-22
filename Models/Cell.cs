namespace Minesweeper_Milestone350.Models
{
    public class Cell
    {
        // Properties to represent the cell's row and column in the board
        public int Row { get; set; }
        public int Column { get; set; }

        // Property to indicate if the cell has been visited
        public bool Visited { get; set; }

        // Property to indicate if the cell contains a mine
        public bool IsMine { get; set; }

        // Property to store the number of live (mine-containing) neighbors
        public int LiveNeighbors { get; set; }

        // Property to store a timestamp for when the cell was created or modified
        public string TimeStamp { get; set; } // New property for timestamp

        // Constructor to initialize a new cell with given row and column
        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
            Visited = false;       // Initialize as not visited
            IsMine = false;        // Initialize as not a mine
            LiveNeighbors = 0;     // Initialize with zero live neighbors
        }
    }
}

