using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TapsasEngine.Utilities;
using ZA6.Animations;
using ZA6.Managers;

namespace ZA6.Models
{
    public class GameStateCartoon : RenderState
    {
        private ZeldaAdventure666 _game;
        private Animation _animation;
        private Texture2D _border;
        private DialogManager _dialogManager;

        public GameStateCartoon(ZeldaAdventure666 game)
        {
            CanReEnter = false;
            _game = game;
        }

        public override void Enter(StateArgs args = null)
        {
            _dialogManager = new DialogManager() { Borderless = true };
            _animation = new Animations.Cartoon(_dialogManager);
            _animation.Enter();
            _border = Utility.CreateColorTexture(Static.NativeWidth, 57, Color.Black);
            Music.Play(Songs.MorningSunlight);
        }

        public override void Update(GameTime gameTime)
        {
            Music.Update(gameTime);
            _dialogManager.Update(Static.GameTime);
            _animation.Update(gameTime);

            if (_animation.IsDone)
            {
                Static.GameData.Save("scenario", "noise");
                Static.Game.StateMachine.TransitionTo(
                    "StartOver",
                    new GameStateStartOver.Args() { NoSpotlight = true }
                );
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Static.Renderer.Start();

            _animation.Draw(spriteBatch);
            spriteBatch.Draw(_border, Vector2.Zero, Color.White);
            spriteBatch.Draw(_border, new Vector2(0, Static.NativeHeight - 57), Color.White);
            _dialogManager.Draw(spriteBatch);

            Static.Renderer.End();
        }

        public override void Exit()
        {
            _animation = null;
            _border.Dispose();
            _border = null;
            _dialogManager = null;
        }
    } 
}