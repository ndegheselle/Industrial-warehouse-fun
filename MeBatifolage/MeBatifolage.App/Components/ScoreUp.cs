namespace MeBatifolage.App.Components
{
    public class ScoreUp : Label
    {
        private const uint ANIMATION_DELAY = 500;
        public void Animate()
        {
            this.Opacity = 1;
            this.TranslationY = 0;
            this.TranslateTo(0, -30, ANIMATION_DELAY, easing: Easing.CubicInOut);
            this.FadeTo(0, ANIMATION_DELAY, easing: Easing.CubicInOut);
        }
    }
}
