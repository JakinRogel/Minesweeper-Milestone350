using System;
using System.Linq;

namespace Minesweeper_Milestone350.Models
{
    public class Board
    {
        // Properties for the board size, grid, and difficulty level
        public int Size { get; set; }
        public Cell[,] Grid { get; set; }
        public double Difficulty { get; set; }

        // Constructor to initialize the board with a given size
        public Board(int size)
        {
            Size = size;
            Grid = new Cell[size, size];
            Difficulty = 0.1; // Default difficulty
            InitializeGrid(); // Initialize the grid with cells
        }

        // Private method to initialize the grid with Cell objects
        private void InitializeGrid()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid[i, j] = new Cell(i, j);
                }
            }
        }
    }
}

