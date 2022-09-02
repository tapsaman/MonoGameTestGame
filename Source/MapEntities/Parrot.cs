using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZA6.Models;
using TapsasEngine.Sprites;

namespace ZA6
{
    public enum ParrotColor : ushort
    {
        Blue,
        Pink,
        Yellow
    }

    public class Parrot : Character
    {
        public override MapLevel Level { get => MapLevel.Air; }
        public bool RunningOff;

        public Parrot(ParrotColor color = ParrotColor.Blue)
        {
            var texture = Img.NPCSprites;
            var textureY = 3 + (int)color;

            SAnimation.DefaultFrameWidth = 20;
            SAnimation.DefaultFrameHeight = 30;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "IdleLeft", new SAnimation(texture, 0.15f, Vector2.Zero,
                        new Rectangle[]
                        {
                            new Rectangle(0, textureY * 30, 20, 30),
                            new Rectangle(0, textureY * 30, 20, 30),
                            new Rectangle(0, textureY * 30, 20, 30),
                            new Rectangle(0, textureY * 30, 20, 30),
                            new Rectangle(20, textureY * 30, 20, 30),
                        },
                        isLooping: true
                    )
                },
                { "IdleRight",  new SAnimation(texture, 2, textureY, 20, 30) },
                { "WalkLeft",   new SAnimation(texture, 2, 0.2f, true, textureY, 3) },
                { "WalkRight",  new SAnimation(texture, 2, 0.2f, true, textureY, 5) },
                { "WalkUp",     new SAnimation(texture, 2, 0.2f, true, textureY, 5) }
            };

            AnimatedSprite = new AnimatedSprite(animations, "IdleLeft");
            Hitbox.Load(1, 1);
            Hitbox.Color = Color.Red;
            SpriteOffset = new Vector2(-12, -22);
            Moving = true;
            DrawingShadow = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (RunningOff)
            {
                Velocity = new Vector2(50, -20);
            }
            
            base.Update(gameTime);
        }
    }
}
