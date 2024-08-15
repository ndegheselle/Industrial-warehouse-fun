using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WarehouseFun.Shared
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

        private int _scoreUpCounter = 0;
        public bool IsScoreUp { get; set; } = false;
        private int _score = 0;
        public int Score
        {
            get { return _score; }
            set {
                if (value > _score)
                {
                    _scoreUpCounter++;
                    IsScoreUp = true;
                    Task.Delay(GameParameters.ScoreDisplayDelayMs).ContinueWith(t =>
                    {
                        _scoreUpCounter--;
                        if (_scoreUpCounter == 0)
                            IsScoreUp = false;
                    });
                }
                _score = value;

                OnPropertyChanged();
            }
        }
    }

    public struct GameParameters
    {
        public GameParameters()
        { }
        public const int DeathDelayMs = 8000;
        public const int ScoreDisplayDelayMs = 2000;
    }
}
