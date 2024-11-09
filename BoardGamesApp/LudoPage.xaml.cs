using System;
using Microsoft.Maui.Controls;

namespace BoardGamesApp;

public partial class LudoPage : ContentPage
{
    private int currentPlayer = 1;
    private int diceResult = 0;
    private int player1Position = 0;
    private int player2Position = 0;
    private const int TotalHexes = 9;

    public LudoPage()
    {
        InitializeComponent();
        UpdateBoard();
    }

    private void OnRollDiceClicked(object sender, EventArgs e)
    {
        // Roll a dice (1-6)
        Random random = new Random();
        diceResult = random.Next(1, 7);
        DiceResultLabel.Text = $"Dice Result: {diceResult}";

        MovePlayer();
        UpdateBoard();
    }

    private void MovePlayer()
    {
        if (currentPlayer == 1)
        {
            player1Position = (player1Position + diceResult) % TotalHexes;
            PlayerTurnLabel.Text = "Player 2's Turn";
            currentPlayer = 2;
        }
        else
        {
            player2Position = (player2Position + diceResult) % TotalHexes;
            PlayerTurnLabel.Text = "Player 1's Turn";
            currentPlayer = 1;
        }
    }

    private void UpdateBoard()
    {
        // Reset all hexes
        for (int i = 0; i < TotalHexes; i++)
        {
            var button = this.FindByName<Button>($"Hex{i}");
            button.Text = i.ToString();
            button.BackgroundColor = Colors.LightGray;
        }

        // Mark player positions
        this.FindByName<Button>($"Hex{player1Position}").Text = "P1";
        this.FindByName<Button>($"Hex{player1Position}").BackgroundColor = Colors.Blue;

        this.FindByName<Button>($"Hex{player2Position}").Text = "P2";
        this.FindByName<Button>($"Hex{player2Position}").BackgroundColor = Colors.Red;

        // Check for win condition
        if (player1Position == TotalHexes - 1)
        {
            GameStatusLabel.Text = "Player 1 Wins!";
            EndGame();
        }
        else if (player2Position == TotalHexes - 1)
        {
            GameStatusLabel.Text = "Player 2 Wins!";
            EndGame();
        }
    }

    private void EndGame()
    {
        RollDiceButton.IsEnabled = false;
    }
}
