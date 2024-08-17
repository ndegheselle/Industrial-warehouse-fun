using WarehouseFun.App.Base;
using WarehouseFun.Shared;

namespace WarehouseFun.App.Pages;

public partial class Game : ContentPage
{
    private readonly GameHubClient _client;
    private readonly IHardwareHandling _hardware;
    private readonly IAlerts _alerts;

    public bool HaveScored { get; set; } = false;

    public List<Actor> Actors => _client.Actors;
    public Actor? CurrentActor => _client.CurrentActor;

    public Game()
    {
        _client = IPlatformApplication.Current!.Services.GetRequiredService<GameHubClient>();
        _hardware = IPlatformApplication.Current!.Services.GetRequiredService<IHardwareHandling>();
        _alerts = IPlatformApplication.Current!.Services.GetRequiredService<IAlerts>();

        _hardware.TriggerPressed += _hardware_TriggerPressed;
        _hardware.DataScanned += _hardware_DataScanned;

        InitializeComponent();
    }

    private async void _hardware_DataScanned(string obj)
    {
        if (Guid.TryParse(obj, out var id))
        {
            await _client.ShootActorAsync(id);
        }
    }

    private void _hardware_TriggerPressed()
    {
        _alerts.Sound("pewpew.mp3");
    }

    private async void Refresh_Tapped(object sender, TappedEventArgs e)
    {
        await _client.ResetScoreAsync();
    }

    private async void Exit_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await _client.ShootActorAsync(CurrentActor.Id);
    }
}