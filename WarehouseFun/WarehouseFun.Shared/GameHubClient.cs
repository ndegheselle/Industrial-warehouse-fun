using Microsoft.AspNetCore.SignalR.Client;

namespace WarehouseFun.Shared
{
    public class GameHubClient
    {
        private HubConnection _connection;

        public List<Actor> Actors { get; private set; } = new List<Actor>();
        public Actor? CurrentActor => Actors.FirstOrDefault(x => x.Id == CurrentActorId);
        public Guid? CurrentActorId { get; set; }

        public GameHubClient(string SignalRUrl)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(new Uri(SignalRUrl))
                .WithAutomaticReconnect()
                .Build();

            _connection.On<Guid>("CurrentActorId", (id) =>
            {
                CurrentActorId = id;
            });

            _connection.On<Actor>("ActorRegistered", (actor) =>
            {
                Actors.Add(actor);
            });

            _connection.On<Guid>("ActorDisconnected", (id) =>
            {
                var actor = Actors.FirstOrDefault(a => a.Id == id);
                if (actor != null)
                {
                    Actors.Remove(actor);
                }
            });

            _connection.On<Actor>("ActorUpdated", (updatedActor) =>
            {
                Actor? actor = Actors.FirstOrDefault(a => a.Id == updatedActor.Id);
                if (actor != null)
                {
                    actor.IsScoreUp = false;
                    actor.State = updatedActor.State;
                    actor.Score = updatedActor.Score;
                }
            });
        }

        public async Task StartConnectionAsync()
        {
            await _connection.StartAsync();
        }

        public async Task RegisterAsync(string username, Guid id)
        {
            await _connection.InvokeAsync("Register", username, id);
            Actors = await GetActorsAsync();
        }

        public async Task ResetScoreAsync()
        {
            await _connection.InvokeAsync("ResetScore");
        }

        public async Task ShootActorAsync(Guid actorId)
        {
            await _connection.InvokeAsync("ShootActor", actorId);
        }

        private async Task<List<Actor>> GetActorsAsync()
        {
            return await _connection.InvokeAsync<List<Actor>>("GetActors");
        }
    }
}
