using Minesweeper_Milestone350.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper_Milestone350.Services
{
    public class GameService
    {
        private static List<ButtonModel> buttons = new List<ButtonModel>();
        private Random random = new Random();
        private const int GRID_SIZE = 25;
        private const int ROW_SIZE = 5; // Assuming a 5x5 grid
        private string[] imageNames = { "bomb_image.jpg", "cream background.jpg", "flag_image.png", "light blue background.jpg" };

        public List<ButtonModel> InitializeButtons()
        {
            buttons = new List<ButtonModel>();
            for (int i = 0; i < GRID_SIZE; i++)
            {
                // Initialize all buttons with a hidden state
                buttons.Add(new ButtonModel(i, Array.IndexOf(imageNames, "cream background.jpg")));
            }
            return buttons;
        }

        public void HandleButtonClick(List<ButtonModel> buttons, int buttonIndex)
        {
            ButtonModel button = buttons.ElementAt(buttonIndex);

            // Place bombs after the first button click to ensure bombs start hidden
            if (buttons.All(btn => btn.ButtonState == Array.IndexOf(imageNames, "cream background.jpg")))
            {
                PlaceBombs(buttonIndex);
                RecursiveReveal(buttonIndex);
            }
            else
            {
                // If the button is a bomb, end the game
                if (button.ButtonState == Array.IndexOf(imageNames, "bomb_image.jpg"))
                {
                    // Game over, reveal all bombs
                    foreach (var btn in buttons)
                    {
                        if (btn.ButtonState == Array.IndexOf(imageNames, "bomb_image.jpg"))
                        {
                            // Reveal the bomb
                            btn.ButtonState = Array.IndexOf(imageNames, "bomb_image.jpg");
                        }
                    }
                    // No need to use ViewData, we'll handle this in the controller
                }
                else
                {
                    // Recursive reveal
                    RecursiveReveal(buttonIndex);
                }
            }
        }

        void RecursiveReveal(int index)
        {
            // Base case: Stop if the index is out of bounds or if the button is already revealed
            if (index < 0 || index >= GRID_SIZE || buttons[index].ButtonState != Array.IndexOf(imageNames, "cream background.jpg"))
                return;

            // Get the row and column of the index
            int row = index / ROW_SIZE;
            int col = index % ROW_SIZE;

            // Count adjacent bombs
            int adjacentBombs = CountAdjacentBombs(row, col);

            // Update button state
            buttons[index].ButtonState = adjacentBombs;

            // If no adjacent bombs, recursively reveal neighbors
            if (adjacentBombs == 0)
            {
                RecursiveReveal(index - 1);         // Left
                RecursiveReveal(index + 1);         // Right
                RecursiveReveal(index - ROW_SIZE);  // Up
                RecursiveReveal(index + ROW_SIZE);  // Down
            }
        }


        private int CountAdjacentBombs(int row, int col)
        {
            int count = 0;
            for (int i = Math.Max(0, row - 1); i <= Math.Min(row + 1, ROW_SIZE - 1); i++)
            {
                for (int j = Math.Max(0, col - 1); j <= Math.Min(col + 1, ROW_SIZE - 1); j++)
                {
                    if (buttons[i * ROW_SIZE + j].ButtonState == Array.IndexOf(imageNames, "bomb_image.jpg"))
                        count++;
                }
            }
            return count;
        }

        private void PlaceBombs(int firstClickIndex)
        {
            List<int> safeIndices = GetSafeIndices(firstClickIndex);
            int bombsToPlace = 5; // Change as needed
            while (bombsToPlace > 0)
            {
                int randomIndex = safeIndices[random.Next(safeIndices.Count)];
                if (buttons[randomIndex].ButtonState != Array.IndexOf(imageNames, "bomb_image.jpg"))
                {
                    buttons[randomIndex].ButtonState = Array.IndexOf(imageNames, "bomb_image.jpg");
                    bombsToPlace--;
                }
            }
        }

        private List<int> GetSafeIndices(int firstClickIndex)
        {
            List<int> safeIndices = new List<int>();
            for (int i = 0; i < GRID_SIZE; i++)
            {
                if (i != firstClickIndex)
                {
                    // Exclude the clicked button and its adjacent buttons
                    int row1 = i / ROW_SIZE;
                    int col1 = i % ROW_SIZE;
                    int row2 = firstClickIndex / ROW_SIZE;
                    int col2 = firstClickIndex % ROW_SIZE;
                    if (Math.Abs(row1 - row2) > 1 || Math.Abs(col1 - col2) > 1)
                    {
                        safeIndices.Add(i);
                    }
                }
            }
            return safeIndices;
        }
    }
}
