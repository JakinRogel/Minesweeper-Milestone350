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

        public void CalculateLiveNeighbors()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (Grid[row, col].IsMine)
                    {
                        Grid[row, col].LiveNeighbors = 9;

                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                int neighborRow = row + i;
                                int neighborColumn = col + j;

                                if (IsValidNeighbor(neighborRow, neighborColumn))
                                {
                                    if (!Grid[neighborRow, neighborColumn].IsMine)
                                    {
                                        Grid[neighborRow, neighborColumn].LiveNeighbors++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool IsValidNeighbor(int row, int col)
        {
            return row >= 0 && row < Size && col >= 0 && col < Size;
        }

        public void FloodFill(int row, int col)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size || Grid[row, col].Visited)
            {
                return;
            }

            Grid[row, col].Visited = true;

            if (Grid[row, col].LiveNeighbors == 0)
            {
                FloodFill(row - 1, col);
                FloodFill(row + 1, col);
                FloodFill(row, col - 1);
                FloodFill(row, col + 1);
                FloodFill(row - 1, col - 1);
                FloodFill(row - 1, col + 1);
                FloodFill(row + 1, col - 1);
                FloodFill(row + 1, col + 1);
            }
        }

        public bool CheckEndGame()
        {
            int visitedCells = Grid.Cast<Cell>().Count(cell => cell.Visited && !cell.IsMine);
            int remainingInertCells = TotalInertCells();
            return visitedCells != remainingInertCells;
        }

        public int TotalInertCells()
        {
            return Grid.Cast<Cell>().Count(cell => !cell.IsMine);
        }

        public void SetDifficulty(double difficulty)
        {
            if (difficulty == 1)
            {
                difficulty = 0.3;
            }
            else if (difficulty == 2)
            {
                difficulty = 0.5;
            }
            else if (difficulty == 3)
            {
                difficulty = 0.7;
            }

                SetupLiveBombs();

        }

        // Update the Board class to randomly place mines on the board
        public void SetupLiveBombs()
        {
            Random random = new Random();
            int bombsToPlace = (int)Math.Round(Size * Size * Difficulty);
            int bombsPlaced = 0;

            while (bombsPlaced < bombsToPlace)
            {
                int row = random.Next(0, Size);
                int col = random.Next(0, Size);

                if (!Grid[row, col].IsMine)
                {
                    Grid[row, col].IsMine = true;
                    bombsPlaced++;
                }
            }
        }

    }
}
