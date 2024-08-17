using Microsoft.AspNetCore.SignalR.Client;
using System.ComponentModel;

namespace WarehouseFun.Shared
{
    public class GameHubClient
    {
        public event Action? Connected;

        private HubConnection _connection;

        public List<Actor> Actors { get; private set; } = new List<Actor>();
        public Actor? CurrentActor { get; set; }

        public GameHubClient(string SignalRUrl)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(SignalRUrl)
                .WithAutomaticReconnect()
                .Build();

            _connection.On<Actor>("Connected", (actor) =>
            {
                // XXX : Why the references are not the same there ???
                CurrentActor = actor;
                Actors.Add(CurrentActor);
                Connected?.Invoke();
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
                    actor.State = updatedActor.State;
                    actor.Score = updatedActor.Score;
                }

                if (CurrentActor?.Id == updatedActor.Id)
                {
                    CurrentActor.State = updatedActor.State;
                    CurrentActor.Score = updatedActor.Score;
                }
            });
        }

        public async Task StartConnectionAsync()
        {
            await _connection.StartAsync();
        }

        public async Task DisconnectAsync()
        {
            await _connection.StopAsync();
            Actors.Clear();
            CurrentActor = null;
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
