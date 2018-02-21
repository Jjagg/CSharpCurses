namespace CSharpCurses.Controls
{
    public class ScrollingLabel : Label
    {
        public int Ticks { get; set; }
        public bool MoveRight { get; set; }

        private int _currentTicks;

        public ScrollingLabel(int x, int y, string text, bool moveRight = false, int ticks = 30)
            : base(x, y, text)
        {
            MoveRight = moveRight;
            Ticks = ticks;
        }

        public override void Update()
        {
            _currentTicks++;
            if (_currentTicks > Ticks)
            {
                _currentTicks -= Ticks;
                if (MoveRight)
                {
                    Text = Text.MoveRight();
                }
                else
                {
                    Text = Text.MoveLeft();
                }
            }
            base.Update();
        }
    }
}
