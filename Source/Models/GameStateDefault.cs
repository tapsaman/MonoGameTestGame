using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6.Models
{
    public class GameStateDefault : RenderState
    {
        private ZeldaAdventure666 _game;

        public GameStateDefault(ZeldaAdventure666 game)
        {
            _game = game;
        }

        public override void Enter(StateArgs _)
        {
            Static.Player.StateMachine.TransitionTo("Idle");
            Static.Renderer.OnPostDraw -= DrawTapeTime;

            if (Static.GameData.GetString("scenario") == "tape")
            {
                Static.Renderer.OnPostDraw += DrawTapeTime;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Static.PlayTimeTimer.Update(gameTime);
            Static.EventSystem.Update(gameTime);
            Static.SceneManager.Update(gameTime);
            Music.Update(gameTime);
            _game.Hud.Update(gameTime);
            _game.TitleText.Update(gameTime);

            if (Input.P1.JustReleased(Input.P1.Start) || Input.P1.JustReleased(Input.P1.Select))
                StateMachine.TransitionTo("MainMenu");
            // To access main menu easily
            else if (Input.P1.JustReleasedMouseLeft())
                StateMachine.TransitionTo("MainMenu");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Static.Renderer.Start();

            //Static.Renderer.ChangeToEffect(Shaders.VertexShaderTest);
            Static.SceneManager.Draw(spriteBatch);
            _game.TitleText.Draw(spriteBatch);
            _game.Hud.Draw(spriteBatch);
            
            Static.Renderer.End();
        }

        public override void Exit() {}

        private void DrawTapeTime()
        {
            string text = Static.PlayTimeTimer.Seconds.ToString();
            text = text.Substring(0, Math.Min(6, text.Length));
            Vector2 size = Static.FontLarge.MeasureString(text);

            Static.SpriteBatch.DrawString(
                Static.FontLarge,
                text,
                new Vector2(Static.Renderer.Resolution.Size.X - size.X - 10, 10),
                Color.Black
            );
        }
    } 
}