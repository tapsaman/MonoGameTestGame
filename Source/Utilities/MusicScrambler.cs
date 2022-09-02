using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using TapsasEngine;
using TapsasEngine.Utilities;

namespace ZA6.Utilities
{
    public abstract class MusicScrambler : IUpdate
    {
        public bool IsDone { get; protected set; }

        //public abstract void Enter();
        public abstract void Update(GameTime gameTime);
    }

    public class StartMenuScrambler : MusicScrambler
    {
        private double _LOOP_TIME = 0.18;
        private double _elapsedTime;
        private double _scrambleTime;
        private TimeSpan _scrambleStart;

        public void Enter()
        {
            _elapsedTime = 0;
            _scrambleTime = 0;
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime < 1.4)
            {
                return;
            }
            else if (_scrambleTime == 0)
            {
                _scrambleTime = _elapsedTime + _LOOP_TIME;
                _scrambleStart = MediaPlayer.PlayPosition;
            }
            else if (_elapsedTime > _scrambleTime)
            {
                if (_elapsedTime > 2.6)
                {
                    IsDone = true;
                    Music.Stop();
                    return;
                }

                _scrambleTime = _elapsedTime + _LOOP_TIME;
                MediaPlayer.Play(MediaPlayer.Queue.ActiveSong, _scrambleStart);
            }
        }
    }
}