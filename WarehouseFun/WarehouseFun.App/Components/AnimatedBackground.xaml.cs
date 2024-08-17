namespace WarehouseFun.App.Components;

public partial class AnimatedBackground : ContentView
{
    private Animation? _animationProgression = null;
    public AnimatedBackground()
	{
		InitializeComponent();
	}

    public void Start(uint duration)
    {
        // Create an animation that gradually increases the width of the Rectangle
        _animationProgression = new Animation(v => AbsoluteContainer.SetLayoutBounds(RectangleProgression, new Rect() { Height = v, Width = 1 }), 0, 1);
        _animationProgression.Commit(
            RectangleProgression,
            "TrackListSideBarAnimation",
            16,
            duration,
            Easing.Linear);
    }

    public void Pause()
    {
        if (_animationProgression != null)
            _animationProgression.Pause();
    }

    public void Stop()
    {
        if (_animationProgression != null)
        {
            _animationProgression.Dispose();
            _animationProgression = null;
            RectangleProgression.AbortAnimation("TrackListSideBarAnimation");
        }
    }

    public void Reset()
    {
        AbsoluteContainer.SetLayoutBounds(RectangleProgression, new Rect() { Height = 0, Width = 1 });
    }
}