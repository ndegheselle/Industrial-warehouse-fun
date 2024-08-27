using MeBatifolage.App.Base;
using MeBatifolage.App.Pages;
using MeBatifolage.Shared;

namespace MeBatifolage.App
{
    public partial class MainPage : ContentPage
    {
        private readonly GameHubClient _client;
        private readonly IHardwareHandling _hardware;

        public MainPage()
        {
            _client = IPlatformApplication.Current!.Services.GetRequiredService<GameHubClient>();
            _hardware = IPlatformApplication.Current!.Services.GetRequiredService<IHardwareHandling>();

            InitializeComponent();
            _hardware.DataScanned += Hardware_DataScanned;
            _client.Connected += _client_Connected;
        }

        protected override void OnDisappearing()
        {
            _hardware.DataScanned -= Hardware_DataScanned;
            _client.Connected -= _client_Connected;
        }

        private void _client_Connected()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new Game());
            });
        }

        protected override async void OnAppearing()
        {
            await _client.StartConnectionAsync();
        }

        private async void Hardware_DataScanned(string data)
        {
            if (Guid.TryParse(data, out var id))
            {
                await _client.RegisterAsync(UsernameEntry.Text, id);
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            HardwareHandling.Instance.OnDataScanned(Guid.NewGuid().ToString());
        }
    }

}
