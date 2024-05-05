using System;
using System.Linq;

namespace Minesweeper_Milestone350.Models
{
    public class Board
    {
        public int Size { get; set; }
        public Cell[,] Grid { get; set; }
        public double Difficulty { get; set; }

        public Board(int size)
        {
            Size = size;
            Grid = new Cell[size, size];
            Difficulty = 0.1; // Default difficulty
            InitializeGrid();
        }

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
