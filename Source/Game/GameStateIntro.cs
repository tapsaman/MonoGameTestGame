using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TapsasEngine.Utilities;
using ZA6.Managers;
using ZA6.Manangers;
using ZA6.UI;

namespace ZA6.Models
{
    public class GameStateIntro : RenderState
    {
        private ZeldaAdventure666 _game;
        private static Vector2 _text1Position = new Vector2(40, 80);
        private static Vector2 _text2Position = new Vector2(40, 120);
        private Color _text1Color;
        private Color _text2Color;
        private float _elapsedTime;
        private const float _FADE_TIME = 6f;
        private const float _END_TIME = 13f;
        private Song _song;
        private string _text1 = "A long time in past...";
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
            _elapsedTime = 0;
            _text1Color = Color.Transparent;
            _text2Color = Color.Transparent;
            //_introAnimation = new Animations.Intro();
        }

        public override void Update(GameTime gameTime)
        {
            Music.Update(gameTime);
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            //_introAnimation.Update(gameTime);

            if (_elapsedTime < _FADE_TIME)
            {
                float v = RoundTo(_elapsedTime / _FADE_TIME, 0.2f);
                _text1Color = new Color(v, v, v, v);
            }
            else if (_elapsedTime < _FADE_TIME * 2)
            {
                float v = RoundTo((_elapsedTime - _FADE_TIME) / _FADE_TIME, 0.2f);
                _text2Color = new Color(v, v, v, v);
            }
            else if (_elapsedTime > _END_TIME)
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

        private float RoundTo(float value, float divider)
        {
            return (float)Math.Round(value / divider) * divider;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Static.Renderer.Start();

            Utility.DrawOverlay(spriteBatch, Color.Black);
            BitmapFontRenderer.DrawString(spriteBatch, _text1, _text1Position, color: _text1Color);
            BitmapFontRenderer.DrawString(spriteBatch, _text2, _text2Position, color: _text2Color);

            Static.Renderer.End();
        }

        public override void Exit() {}
    } 
}