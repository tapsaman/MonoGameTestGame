using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;
using ZA6.Models;
using TapsasEngine.Sprites;
using TapsasEngine;
using TapsasEngine.Utilities;

namespace ZA6
{
    public class Rattler : Character
    {
        private float _elapsedTime = 0f;
        private bool _drummedLeaves;

        public Rattler()
        {
            Moving = false;
            
            var texture = Img.NPCLargeSprites;
            SAnimation.DefaultFrameWidth = 30;
            SAnimation.DefaultFrameHeight = 40;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "Shake", new SAnimation(texture, 3, 0.06f, true, 1, 0) },
                { "ReadyToDrum", new SAnimation(texture, 3, 1) },
                { "Drum", new SAnimation(texture, 2, 0.15f, false, 1, 2) }
            };

            AnimatedSprite = new AnimatedSprite(animations, "Shake");
            Hitbox.Load(16, 16);
            SpriteOffset = new Vector2(-7, -20);
            DrawingShadow = false;
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.GetSeconds();
            
            if (_elapsedTime < 1f)
            {
                AnimatedSprite.SetAnimation("Shake");
            }
            else if (_elapsedTime < 1.5f)
            {
                AnimatedSprite.SetAnimation("ReadyToDrum");
            }
            else if (_elapsedTime < 2f)
            {
                AnimatedSprite.SetAnimation("Drum");

                if (!_drummedLeaves)
                {
                    _drummedLeaves = true;
                    Static.Scene.Add(new Animations.RainbowBushSlash(Position));
                }
            }
            else
            {
                _elapsedTime = 0f;
                _drummedLeaves = false;
            }

            AnimatedSprite.Update(gameTime);
        }
    }
}
