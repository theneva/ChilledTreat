using Microsoft.Xna.Framework;

namespace ChilledTreat
{
    class FrameInfo
    {
        readonly public static FrameInfo Instance = new FrameInfo();

        private FrameInfo();

        GameTime _gameTime = new GameTime();
        public GameTime GameTime
        {
            get
            {
                return _gameTime;
            }

            set
            {
                _gameTime = value;
                DeltaTime = (float)GameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public float DeltaTime { get; protected set; }
    }
}
