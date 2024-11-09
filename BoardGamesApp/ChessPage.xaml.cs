using ChessDotNet;
using ChessDotNet.Pieces;
using Microsoft.Maui.Controls;
using System;

namespace BoardGamesApp;

public partial class ChessPage : ContentPage
{
    private ChessGame chessGame;
    private string selectedSquare = null;

    public ChessPage()
    {
        InitializeComponent();
        chessGame = new ChessGame();
        CreateChessBoard();
        UpdateChessBoard();
    }

    private void CreateChessBoard()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Button button = new Button
                {
                    StyleId = $"{row},{col}",
                    BackgroundColor = (row + col) % 2 == 0 ? Colors.Bisque : Colors.SaddleBrown,
                    FontSize = 24,
                    TextColor = Colors.Black // Ensures text is visible
                };

                button.SetValue(Grid.RowProperty, row);
                button.SetValue(Grid.ColumnProperty, col);
                button.Clicked += OnSquareClicked;
                ChessBoardGrid.Add(button);
            }
        }
    }

    private void UpdateChessBoard()
    {
        try
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    string buttonName = $"{row},{col}";
                    var button = ChessBoardGrid.Children
                        .FirstOrDefault(c => c is Button b && b.StyleId == buttonName) as Button;

                    if (button == null) continue;

                    ChessDotNet.File file = col switch
                    {
                        0 => ChessDotNet.File.A,
                        1 => ChessDotNet.File.B,
                        2 => ChessDotNet.File.C,
                        3 => ChessDotNet.File.D,
                        4 => ChessDotNet.File.E,
                        5 => ChessDotNet.File.F,
                        6 => ChessDotNet.File.G,
                        7 => ChessDotNet.File.H,
                        _ => throw new ArgumentOutOfRangeException(nameof(col), "Column index is out of range")
                    };

                    int rank = 8 - row;
                    var position = new Position(file, rank);

                    Piece piece = chessGame.GetPieceAt(position);

                    button.Text = piece switch
                    {
                        Pawn p when p.Owner == Player.White => "♙",
                        Pawn p => "♟",
                        Rook r when r.Owner == Player.White => "♖",
                        Rook r => "♜",
                        Knight n when n.Owner == Player.White => "♘",
                        Knight n => "♞",
                        Bishop b when b.Owner == Player.White => "♗",
                        Bishop b => "♝",
                        Queen q when q.Owner == Player.White => "♕",
                        Queen q => "♛",
                        King k when k.Owner == Player.White => "♔",
                        King k => "♚",
                        _ => ""
                    };
                }
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"An error occurred while updating the board: {ex.Message}", "OK");
        }
    }

    private void OnSquareClicked(object sender, EventArgs e)
    {
        try
        {
            var button = (Button)sender;
            var position = button.StyleId.Split(',');

            if (position.Length != 2)
            {
                DisplayAlert("Error", "Invalid button position.", "OK");
                return;
            }

            int row = int.Parse(position[0]);
            int col = int.Parse(position[1]);

            if (row < 0 || row > 7 || col < 0 || col > 7)
            {
                DisplayAlert("Error", "Invalid board coordinates.", "OK");
                return;
            }

            ChessDotNet.File file = col switch
            {
                0 => ChessDotNet.File.A,
                1 => ChessDotNet.File.B,
                2 => ChessDotNet.File.C,
                3 => ChessDotNet.File.D,
                4 => ChessDotNet.File.E,
                5 => ChessDotNet.File.F,
                6 => ChessDotNet.File.G,
                7 => ChessDotNet.File.H,
                _ => throw new ArgumentOutOfRangeException(nameof(col), "Column index is out of range")
            };

            int rank = 8 - row;
            var square = new Position(file, rank);

            if (selectedSquare == null)
            {
                selectedSquare = $"{file}{rank}";
                button.BackgroundColor = Colors.Yellow;
            }
            else
            {
                var startFile = (ChessDotNet.File)(selectedSquare[0] - 'A');
                var startRank = int.Parse(selectedSquare[1].ToString());
                var startPosition = new Position(startFile, startRank);

                var move = new Move(startPosition, square, chessGame.WhoseTurn);

                if (chessGame.IsValidMove(move))
                {
                    chessGame.MakeMove(move, true);
                    UpdateChessBoard();
                }
                else
                {
                    DisplayAlert("Invalid Move", "This move is not valid.", "OK");
                }

                selectedSquare = null;
                ResetBoardColors();
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            selectedSquare = null;
            ResetBoardColors();
        }
    }

    private void ResetBoardColors()
    {
        foreach (var child in ChessBoardGrid.Children)
        {
            if (child is Button button)
            {
                var position = button.StyleId.Split(',');
                int row = int.Parse(position[0]);
                int col = int.Parse(position[1]);
                button.BackgroundColor = (row + col) % 2 == 0 ? Colors.Bisque : Colors.SaddleBrown;
            }
        }
    }


    protected override bool OnBackButtonPressed()
    {
        if (Navigation.NavigationStack.Count > 1)
        {
            Navigation.PopAsync();
        }
        return true;
    }
}

