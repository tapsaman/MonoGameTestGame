using System;
using Microsoft.Xna.Framework;
using ZA6;

namespace TapsasEngine.Utilities
{
    public class Timer
    {
        public float Seconds { get; set; }

        public string HourMinuteSecondString
        {
            get
            {
                return new TimeSpan(0, 0, (int)Math.Floor(Seconds)).ToString();
            }
        }

        public void Update(GameTime gameTime)
        {
            Seconds += gameTime.GetSeconds();
        }
    }
}