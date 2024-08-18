using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using WarehouseFun.Shared;

namespace WarehouseFun.Api.Hubs
{
    public class GameHub : Hub
    {
        private static ConcurrentDictionary<string, Actor> Actors = new ConcurrentDictionary<string, Actor>();
        private readonly IHubContext<GameHub> _hubContext;

        public GameHub(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (Actors.TryRemove(Context.ConnectionId, out var actor))
            {
                Clients.All.SendAsync("ActorDisconnected", actor.Id);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public void Register(string username, Guid id)
        {
            var actor = new Actor()
            {
                Id = id,
                Name = username,
                State = EnumActorState.Active,
            };
            Actors.TryAdd(Context.ConnectionId, actor);
            Clients.Caller.SendAsync("Connected", actor);
            Clients.Others.SendAsync("ActorRegistered", actor);
        }

        public IEnumerable<Actor> GetActors()
        {
            return Actors.Values;
        }

        public async void ShootActor(Guid actorId)
        {
            Actor? actor = Actors.Values.FirstOrDefault(a => a.Id == actorId);

            if (actor == null || actor.State != EnumActorState.Active)
                return;

            // Update the caller score
            if (Actors.TryGetValue(Context.ConnectionId, out var callerActor))
            {
                if (callerActor.State != EnumActorState.Active)
                    return;

                callerActor.Score++;
                await Clients.Caller.SendAsync("Scored", actor);
                await Clients.All.SendAsync("ActorUpdated", callerActor);
            }
            else
            {
                return;
            }

            // Update the state of the target
            actor.State = EnumActorState.Down;
            await Clients.All.SendAsync("ActorUpdated", actor);
            Revive(actor);
        }

        private async void Revive(Actor actor)
        {
            // Revive after a delay
            await Task.Delay(GameParameters.DeathDelayMs);
            actor.State = EnumActorState.Active;
            await _hubContext.Clients.All.SendAsync("ActorUpdated", actor);
        }

        public async void ResetScore()
        {
            // Update the caller score
            if (Actors.TryGetValue(Context.ConnectionId, out var callerActor))
            {
                callerActor.Score = 0;
                await Clients.All.SendAsync("ActorUpdated", callerActor);
            }
        }
    }
}
