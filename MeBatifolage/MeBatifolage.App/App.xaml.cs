using Plugin.Maui.Audio;
using MeBatifolage.App.Base;
using MeBatifolage.Shared;

namespace MeBatifolage.App
{
    public partial class App : Application, IAlerts
    {
        private readonly GameHubClient _client;
        private readonly IAudioManager _audioManager = AudioManager.Current;

        public App()
        {
            _client = IPlatformApplication.Current!.Services.GetRequiredService<GameHubClient>();
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        public async void Sound(string soundName)
        {
            IAudioPlayer player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(soundName));
            player.PlaybackEnded += (s, e) =>
            {
                player.Dispose();
            };
            player.Play();
        }
    }
}
