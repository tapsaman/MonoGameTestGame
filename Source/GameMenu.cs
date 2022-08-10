using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Controls;
using ZA6.Managers;

namespace ZA6
{
    public class GameMenu : Menu
    {
        public GameMenu(EventHandler startGame)
        {
            OverlayColor = new Color(50, 50, 50);
            var buttonTexture = Static.Content.Load<Texture2D>("Button");
            var font = Static.Content.Load<SpriteFont>("Fonts/TestFont");
            var fontSmall = Static.Content.Load<SpriteFont>("Fonts/TestFontSmall");

            Button startButton = new Button(buttonTexture, font)
            {
                Text = !Static.GameStarted ? "START" : "RESUME"
            };
            startButton.Click += startGame;

            Button settingsButton = new Button(buttonTexture, font)
            {
                Text = "OPTIONS"
            };
            settingsButton.Click += GoToSettings;

            Button quitButton = new Button(buttonTexture, font)
            {
                Text = "QUIT"
            };
            quitButton.Click += QuitGame;

            Add(startButton);
            Add(settingsButton);
            Add(quitButton);

            CalculateSize();
        }

        private void GoToSettings(object sender, EventArgs e)
        {
            UI.Add(new SettingsMenu());
        }

        private void QuitGame(object sender, EventArgs e)
        {
            Static.Game.Exit();
        }
    }
}