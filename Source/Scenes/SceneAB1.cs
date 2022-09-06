using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TapsasEngine.Enums;
using ZA6.Animations;

namespace ZA6
{
    public class SceneAB1 : Scene
    {
        private Bunny _bunny;
        private Animation _crispyAnimation;

        public SceneAB1()
        {
            Theme = Songs.DarkWorld;
            ExitTransitions[Direction.Right] = TransitionType.FadeToBlack;

            if (Static.GameData.GetString("scenario") == "noise")
            {
                UseAlternativeLayers = new string[] { "Noise" };
            }
        }

        protected override void Load()
        {
            if (Static.GameData.GetString("scenario") == "mushroom")
            {
                Add(new Items.Mushroom() { Position = TileMap.ConvertTileXY(30, 15) });
            }
            else if (Static.GameData.GetString("scenario") == "noise")
            {
                Add(new Text() { Position = TileMap.ConvertTileXY(57, 15), Message = "FUCK YOU" });
                Add(new Doorway(new Vector2(31 * 8, 27 * 8 + 4), "Void"));
            }
            else
            {
                _bunny = new Bunny() { Position = TileMap.ConvertTileXY(31, 25) };
                Add(_bunny);
            }
        }

        public override void Start()
        {
            base.Start();

            if (Static.GameData.GetString("scenario") == "crispy")
            {
                _crispyAnimation = new Animations.Crispy();
                _crispyAnimation.Enter();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_crispyAnimation != null)
            {
                _crispyAnimation.Update(gameTime);
            }

            if (_bunny != null && !_bunny.RunningOff)
            {
                var diff = Static.Player.Hitbox.Rectangle.Center - _bunny.Hitbox.Rectangle.Center;

                if (Math.Pow(diff.X, 2) + Math.Pow(diff.Y, 2) < Math.Pow(50f, 2))
                {
                    _bunny.NoClip = true;
                    _bunny.RunningOff = true;
                }
            }
        }
    }
}