using WarehouseFun.App.Base;
using WarehouseFun.Shared;

namespace WarehouseFun.App.Pages;

public partial class Game : ContentPage
{
    private readonly GameHubClient _client;
    private readonly IHardwareHandling _hardware;

    public bool HaveScored { get; set; } = false;

    public List<Actor> Actors { get; set; } = new List<Actor>()
    {
        new Actor() {Name = "Test", Score = 1},
        new Actor() {Name = "Test", Score = 10},
        new Actor() {Name = "Test", Score = 30, State = EnumActorState.Down},
    };
    public Actor CurrentActor { get; set; } = new Actor()
    {
        Name = "azfglakl",
        Score = 100
    };

    public Game()
    {
        _client = IPlatformApplication.Current!.Services.GetRequiredService<GameHubClient>();
        _hardware = IPlatformApplication.Current!.Services.GetRequiredService<IHardwareHandling>();
        InitializeComponent();
    }

    private async void Refresh_Tapped(object sender, TappedEventArgs e)
    {
        await _client.ResetScoreAsync();
    }

    private async void Exit_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Actors.First().Score++;
        CurrentActor.Score++;
    }
}