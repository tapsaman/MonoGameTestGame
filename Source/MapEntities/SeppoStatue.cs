using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Enums;
using ZA6.Models;
using TapsasEngine.Sprites;
using System;
using TapsasEngine;

namespace ZA6
{
    public class SeppoStatue : Character
    {       
        public override MapLevel Level { get => MapLevel.Character; }

        private bool _talkingTo;
        private static int _talkIndex;

        public SeppoStatue()
        {
            var texture = Img.NPCSprites;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "IdleDown", new SAnimation(texture, 2, 1, 20, 30) }
            };

            AnimatedSprite = new AnimatedSprite(animations, "IdleDown");
            Hitbox.Load(16, 16);
            SpriteOffset = new Vector2(-1, -12);
            Interactable = true;
            Facing = Direction.Down;
            Trigger += TalkTo;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (_talkingTo)
            {
                Shaders.SpriteNoise.Parameters["seed"].SetValue(Shaders.SpriteNoise.Parameters["seed"].GetValueSingle() + 1);
                Static.Renderer.ChangeToEffect(Shaders.SpriteNoise);
                base.Draw(spriteBatch, offset);
                Static.Renderer.ChangeToDefault();
            }
            else
            {
                base.Draw(spriteBatch, offset);
            }
        }

        private void TalkTo(Character _)
        {
            _talkingTo = true;

            Static.EventSystem.Load(new Event[]
            {
                new TextEvent(new Dialog(IndexToText(++_talkIndex)), this),
                new RemoveEvent(this)
            });
        }

        private static string IndexToText(int index)
        {
            if (index < 1)
                throw new Exception("Unexpected SeppoStatue index " + index);
            if (index == 1)
                return "SYSTEM ERROR";
            if (index == 2)
                return "FAILURE";
            
            return "DEATH";
        }
    }
}
