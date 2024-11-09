using System;
using Microsoft.Maui.Controls;

namespace BoardGamesApp;

public partial class TicTacToePage : ContentPage
{
    private string currentPlayer = "X";
    private string[] board = new string[9];
    private bool gameEnded = false;

    public TicTacToePage()
    {
        InitializeComponent();
        ResetGame();
    }

    private void OnButtonClicked(object sender, EventArgs e)
    {
        if (gameEnded) return;

        var button = (Button)sender;
        int index = int.Parse(button.StyleId);

        if (board[index] == null)
        {
            board[index] = currentPlayer;
            button.Text = currentPlayer;

            if (CheckForWinner())
            {
                DisplayAlert("Game Over", $"{currentPlayer} wins!", "OK");
                gameEnded = true;
                return;
            }

            if (Array.TrueForAll(board, cell => cell != null))
            {
                DisplayAlert("Game Over", "It's a draw!", "OK");
                gameEnded = true;
                return;
            }

            currentPlayer = currentPlayer == "X" ? "O" : "X";
        }
    }

    private bool CheckForWinner()
    {
        int[,] winningCombinations = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Rows
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columns
            {0, 4, 8}, {2, 4, 6}             // Diagonals
        };

        for (int i = 0; i < winningCombinations.GetLength(0); i++)
        {
            int a = winningCombinations[i, 0];
            int b = winningCombinations[i, 1];
            int c = winningCombinations[i, 2];

            if (board[a] != null && board[a] == board[b] && board[a] == board[c])
            {
                return true;
            }
        }

        return false;
    }

    private void ResetGame()
    {
        currentPlayer = "X";
        board = new string[9];
        gameEnded = false;

        foreach (var button in this.Content.FindByName<Grid>("TicTacToeGrid").Children.OfType<Button>())
        {
            button.Text = string.Empty;
        }
    }
}
