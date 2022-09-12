using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;
using TapsasEngine.Sprites;
using TapsasEngine.Utilities;
using ZA6.Models;

namespace ZA6
{
    public class GrowingHole : MapObject
    {
        public override MapLevel Level { get => MapLevel.Ground; }

        private int _width = 16;
        private int _height = 16;
        private float _elapsedTime;
        private float _growthTime = 2f;
        private int _growthCount = 0;
        private Random _random;
        //private TouchEventTrigger _innerHoleEventTrigger;
        
        public GrowingHole(Vector2 position)
        {
            // Always use same seed so growth intervals stay the same 
            _random = new Random(0);

            Position = position;
            
            Sprite = new Sprite(Img.ObjectTexture, new Rectangle(48, 0, 16, 16));
            Hitbox.Load(16, 16);
            TriggeredOnTouch = true;
            Trigger += PullToHole;
            
            /*_innerHoleEventTrigger = new TouchEventTrigger(Position + new Vector2(5, 5), _width - 10, _height - 10);
            _innerHoleEventTrigger.Trigger += PullToHole;

            Static.Scene.Add(_innerHoleEventTrigger);*/
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _elapsedTime += gameTime.GetSeconds();

            if (_elapsedTime > _growthTime)
            {
                if (_growthCount == 10 && Static.Game.StateMachine.CurrentStateKey == "Default")
                {
                    Static.EventSystem.Load(
                        new TextEvent(new Dialog("I don't think you have any\noptions, kupo."), this)
                    );
                }
                else
                {
                    GrowInSize();
                }

                _elapsedTime = 0f;
                _growthTime = 0.8f + (float)_random.NextDouble() * 2.2f;
                _growthCount++;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Sprite.Draw(spriteBatch, Position + SpriteOffset + offset, _width, _height);
        }

        private void GrowInSize()
        {
            Static.Scene.Camera.ShakeVelocity = new Vector2(10, 10);

            _width += 16;
            _height += 16;
            Hitbox.Load(_width, _height);
            Position += new Vector2(-8, -8);
            
            //_innerHoleEventTrigger.Hitbox.Load(_width - 10, _height - 10);
            //_innerHoleEventTrigger.Position = Position + new Vector2(5, 5);

            SFX.Ram.Play();
            SFX.Quake1.Play();
            SFX.MoveObject.Play();
        }

        private void PullToHole(Character character)
        {
            character.ElementalVelocity = Hitbox.Rectangle.Center - character.Hitbox.Rectangle.Center;
            character.ElementalVelocity.Normalize();
            character.ElementalVelocity *= 33f;

            if (Hitbox.Rectangle.Contains(character.Hitbox.Rectangle))
            {
                character.MakeFall();
            }
        }
    }
}