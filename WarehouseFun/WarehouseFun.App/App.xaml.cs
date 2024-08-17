using Plugin.Maui.Audio;
using WarehouseFun.App.Base;
using WarehouseFun.Shared;

namespace WarehouseFun.App
{
    public partial class App : Application, IAlerts
    {
        private readonly GameHubClient _client;
        private readonly IAudioManager _audioManager = AudioManager.Current;
        private List<IAudioPlayer> _players = [];

        public App()
        {
            _client = IPlatformApplication.Current!.Services.GetRequiredService<GameHubClient>();
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        public async void Sound(string soundName)
        {
            IAudioPlayer player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(soundName)); _players.Add(player);
            player.PlaybackEnded += (s, e) =>
            {
                _players.Remove(player);
            };
            player.Play();
        }
    }
}
