using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TapsasEngine.Sprites;
using TapsasEngine.Utilities;
using ZA6.Animations;
using ZA6.Models;

namespace ZA6
{
    public class RitualScene : Scene
    {
        Sprite DeadErkki = new Sprite(Img.NPCSprites, new Rectangle(0, 180, 40, 30));
        float _elapsedTime = 0f;
        float _doneTime = 0f;
        Animation[] _animations;

        public RitualScene()
        {
            Theme = Static.Content.Load<Song>("Songs/colin_stetson_reborn");
        }

        protected override void Load()
        {
            var bunny1 = new Bunny() { Position = TileMap.ConvertTileXY(39, 10) };
            var bunny2 = new Bunny() { Position = TileMap.ConvertTileXY(26, 18) };
            var bunny3 = new Bunny() { Position = TileMap.ConvertTileXY(31, 15) };
            var parrot1 = new Parrot() { Position = TileMap.ConvertTileXY(29, 14) };
            var parrot2 = new Parrot(ParrotColor.Pink) { Position = TileMap.ConvertTileXY(31, 25) };
            var parrot3 = new Parrot(ParrotColor.Yellow) { Position = TileMap.ConvertTileXY(42, 2) };
            var crab1 = new Crab() { Position = TileMap.ConvertTileXY(21, 16) };
            var crab2 = new Crab() { Position = TileMap.ConvertTileXY(40, 23) };
            var bigBird = new BigBird() { Position = TileMap.ConvertTileXY(40, 19) };
            var deadLink = new DeadLink() { Position = TileMap.ConvertTileXY(31, 4) };
            var linksHat = new DeadLinkHat() { Position = TileMap.ConvertTileXY(25, -2) };
            
            Add(bunny1);
            Add(bunny2);
            Add(bunny3);
            Add(crab1);
            Add(crab2);
            Add(parrot1);
            Add(parrot2);
            Add(parrot3);
            Add(bigBird);
            Add(deadLink);
            Add(linksHat);

            _animations = new Animation[]
            {
                new Animations.Ritual(deadLink, linksHat),
                new Animations.DanceAround(bunny1),
                new Animations.DanceAround(bunny2),
                new Animations.DanceAround(parrot1),
                new Animations.DanceAround(parrot2),
                new Animations.DanceAround(parrot3),
                new Animations.ShiftAround(crab1, 0.6f),
                new Animations.ShiftAround(crab2, 0.7f)
            };

            foreach (var item in _animations)
            {
                item.Enter();
            }
        }

        public override void Start()
        {
            base.Start();

            Static.Game.Hud.Drawing = false;
            Static.Renderer.ApplyPostEffect(null);
            Static.Game.StateMachine.TransitionTo("Cutscene");
            Static.Player.Sprite.Color = Color.Transparent;
            Static.Player.DrawingShadow = false;
            Static.Player.StateMachine.TransitionTo("Stopped");
        }

        public override void Update(GameTime gameTime)
        {
            //Static.Player.StateMachine.TransitionTo("Stopped");

            base.Update(gameTime);

            _elapsedTime += gameTime.GetSeconds();

            

            if (!_animations[0].IsDone)
            {
                foreach (var item in _animations)
                {
                    item.Update(gameTime);
                }
            }
            else if (_doneTime == 0)
            {
                _doneTime = _elapsedTime;
                Music.Stop();
                SFX.RitualEndSound.Play();
            }
            else if (_elapsedTime > _doneTime + 4f)
            {
                Static.GameData.Save("scenario", "end");
                Static.Game.StateMachine.TransitionTo(
                    "Intro",
                    new GameStateIntro.Args()
                    {
                        Text1 = "juuh",
                        Text2 = "elikkäs",
                    }
                );
            }
        }

        public override void DrawOverlay(SpriteBatch spriteBatch)
        {
            DeadErkki.Draw(spriteBatch, TileMap.ConvertTileXY(16, 19) + DrawOffset);

            // Sepia overlay
            Utility.DrawOverlay(spriteBatch, new Color(146, 99, 45, 60));

            if (!_animations[0].IsDone)
            {
                _animations[0].Draw(spriteBatch);
                var v = (float)Math.Min(1f, _elapsedTime / 6f);
                Utility.DrawOverlay(spriteBatch, new Color(0, 0, 0, 1f - v));
            }
            else if (_doneTime != 0)
            {
                Utility.DrawOverlay(spriteBatch, new Color(0, 0, 0));
            }
        }
    }
}