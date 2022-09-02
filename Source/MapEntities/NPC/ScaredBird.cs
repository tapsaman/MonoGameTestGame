using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZA6.Models;
using TapsasEngine.Sprites;
using System;

namespace ZA6
{
    public class ScaredBird : Character
    {
        public override MapLevel Level { get => MapLevel.Air; }
        public bool FlyingOff;

        public override void Update(GameTime gameTime)
        {
            if (!FlyingOff)
            {
                var diff = Static.Player.Hitbox.Rectangle.Center - Hitbox.Rectangle.Center;

                if (Math.Pow(diff.X, 2) + Math.Pow(diff.Y, 2) < Math.Pow(50f, 2))
                {
                    FlyingOff = true;
                    Static.GameData.Save("bird scare count", Static.GameData.GetInt("bird scare count") + 1);
                    Static.SessionData.Save("bird scared", true);
                }
            }
            else
            {
                Velocity = new Vector2(50, -20);
            }
            
            base.Update(gameTime);
        }
    }
}
