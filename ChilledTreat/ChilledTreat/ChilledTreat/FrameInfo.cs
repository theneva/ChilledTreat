using Microsoft.Xna.Framework;

namespace ChilledTreat
{
    class FrameInfo
    {
        readonly public static FrameInfo Instance = new FrameInfo();

        private FrameInfo() { }

        GameTime _GameTime = new GameTime();
        public GameTime GameTime
        {
            get
            {
                return _GameTime;
            }

            set
            {
                _GameTime = value;
                DeltaTime = (float)GameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        float _DeltaTime = 0f;
        public float DeltaTime
        {
            get
            {
                return _DeltaTime;
            }

            protected set
            {
                _DeltaTime = value;
            }
        }
    }
}
