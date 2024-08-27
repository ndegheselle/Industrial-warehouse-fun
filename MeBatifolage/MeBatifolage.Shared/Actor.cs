using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MeBatifolage.Shared
{
    public enum EnumActorState
    {
        None,
        Active,
        Down,
    }

    public class Actor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public EnumActorState State { get; set; }
        public int Score { get; set; }
    }

    public struct GameParameters
    {
        public GameParameters()
        { }
        public const int DeathDelayMs = 5000;
        public const int ScoreDisplayDelayMs = 2000;
    }
}
