using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TapsasEngine.Utilities;
using ZA6.Animations;

namespace ZA6.Models
{
    public class GameStateIntro : RenderState
    {
        private ZeldaAdventure666 _game;
        private Song _song;
        private const string _TEXT1 = "A long time in past in world\nswalloved by darkness...";
        private const string _TEXT2 = "a hero is exists.";
        private Animation _introAnimation;
        private float _elapsedTime;
        private Args _args;

        public class Args : StateArgs
        {
            public string Text1 = null;
            public string Text2 = null;
        }

        public GameStateIntro(ZeldaAdventure666 game)
        {
            CanReEnter = true;
            _game = game;
            _song = Songs.Intro;
        }

        public override void Enter(StateArgs args = null)
        {
            _elapsedTime = 0f;
            _introAnimation = null;

            _args = args != null && args is Args a ? a : new Args();
            //Music.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            Music.Update(gameTime);

            if (_introAnimation == null)
            {
                _elapsedTime += gameTime.GetSeconds();

                if (_elapsedTime > 1f)
                {
                    Music.PlayOnce(_song);
                    _introAnimation = new Animations.Intro(
                        _args.Text1 ?? _TEXT1,
                        _args.Text2 ?? _TEXT2
                    );
                    _introAnimation.Enter();
                }

                return;
            }

            _introAnimation.Update(gameTime);

            if (_introAnimation.ElapsedTime > 13f)
            {
                Static.Game.StateMachine.TransitionTo("StartOver");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Static.Renderer.Start();

            Utility.DrawOverlay(spriteBatch, Color.Black);
            _introAnimation?.Draw(spriteBatch);

            Static.Renderer.End();
        }

        public override void Exit()
        {
            _introAnimation = null;
        }
    } 
}