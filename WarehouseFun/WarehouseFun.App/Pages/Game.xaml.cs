using System.Collections.ObjectModel;
using WarehouseFun.App.Base;
using WarehouseFun.App.Components;
using WarehouseFun.Shared;

namespace WarehouseFun.App.Pages;

public partial class Game : ContentPage
{
    private readonly GameHubClient _client;
    private readonly IHardwareHandling _hardware;
    private readonly IAlerts _alerts;

    public bool HaveScored { get; set; } = false;

    public ObservableCollection<Actor> Actors => _client.Actors;
    public Actor? CurrentActor => _client.CurrentActor;

    public Game()
    {
        _client = IPlatformApplication.Current!.Services.GetRequiredService<GameHubClient>();
        _hardware = IPlatformApplication.Current!.Services.GetRequiredService<IHardwareHandling>();
        _alerts = IPlatformApplication.Current!.Services.GetRequiredService<IAlerts>();

        _hardware.TriggerPressed += _hardware_TriggerPressed;
        _hardware.DataScanned += _hardware_DataScanned;
        _client.Scored += _client_Scored;

        if (CurrentActor == null)
            throw new ArgumentNullException(nameof(CurrentActor));

        InitializeComponent();

        CurrentActor.PropertyChanged += CurrentActor_PropertyChanged;
    }

    protected override void OnDisappearing()
    {
        _hardware.TriggerPressed -= _hardware_TriggerPressed;
        _hardware.DataScanned -= _hardware_DataScanned;
        _client.Scored -= _client_Scored;
    }

    private void CurrentActor_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Actor.State))
        {
            if (_client.CurrentActor?.State == EnumActorState.Down)
            {
                AnimatedBackground.Start(GameParameters.DeathDelayMs);
            }
        }
    }

    private void _client_Scored(Actor obj)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            AnimatedScore.Animate();
        });
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
        if (_client.CurrentActor?.State != EnumActorState.Down)
        {
            _alerts.Sound("pewpew.mp3");
        }
    }

    private async void Refresh_Tapped(object sender, TappedEventArgs e)
    {
        await _client.ResetScoreAsync();
    }

    private async void Exit_Tapped(object sender, TappedEventArgs e)
    {
        await _client.DisconnectAsync();
        await Navigation.PushAsync(new MainPage());
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        HardwareHandling.Instance.OnDataScanned(CurrentActor!.Id.ToString());
    }
}