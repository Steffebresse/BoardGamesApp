namespace BoardGamesApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnChessClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChessPage());
    }

    private async void OnLudoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LudoPage());
    }

    private async void OnTicTacToeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TicTacToePage());
    }
}
