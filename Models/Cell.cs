namespace Minesweeper_Milestone350.Models
{
    public class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool Visited { get; set; }
        public bool IsMine { get; set; }
        public int LiveNeighbors { get; set; }
        public bool flagged { get; set; }
        public string TimeStamp { get; set; } // New property for timestamp

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
            Visited = false;
            IsMine = false;
            LiveNeighbors = 0;
            flagged = false;
        }
    }
}
