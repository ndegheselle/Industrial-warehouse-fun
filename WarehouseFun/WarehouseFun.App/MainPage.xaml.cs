using WarehouseFun.App.Base;
using WarehouseFun.App.Pages;
using WarehouseFun.Shared;

namespace WarehouseFun.App
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
                _client.CurrentActorId = id;
                await Navigation.PushAsync(new Game());
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Game());
        }
    }

}
