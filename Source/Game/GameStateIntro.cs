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
        private const float _END_TIME = 14f;
        private Song _song;
        private string _text1 = "A long time in past in world\nswalloved by darkness...";
        private string _text2 = "a hero is exists.";
        private Animation _introAnimation;

        public GameStateIntro(ZeldaAdventure666 game)
        {
            CanReEnter = true;
            _game = game;
            _song = Static.Content.Load<Song>("loz_intro");
        }

        public override void Enter(StateArgs _)
        {
            Music.PlayOnce(_song);
            _introAnimation = new Animations.Intro(_text1, _text2);
            _introAnimation.Enter();
        }

        public override void Update(GameTime gameTime)
        {
            Music.Update(gameTime);

            _introAnimation.Update(gameTime);

            if (_introAnimation.ElapsedTime > _END_TIME)
            {
                Static.Game.StateMachine.TransitionTo(
                    "StartOver",
                    new GameStateStartOver.Args()
                    {
                        SaveData = Static.LoadedGame
                    }
                );
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Static.Renderer.Start();

            Utility.DrawOverlay(spriteBatch, Color.Black);
            _introAnimation.Draw(spriteBatch);

            Static.Renderer.End();
        }

        public override void Exit() {}
    } 
}