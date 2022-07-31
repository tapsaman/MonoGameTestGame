using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Controls;

namespace MonoGameTestGame
{
    public class GameMenu
    {
        public List<UIComponent> Components;
        public int Padding = 5;
        private CheckBoxButton _gamePadCheckBox;
        private Vector2 _firstComponentPosition;

        public GameMenu(EventHandler startGame)
        {
            Components = new List<UIComponent>();

            var buttonTexture = StaticData.Content.Load<Texture2D>("Button");
            var font = StaticData.Content.Load<SpriteFont>("Fonts/TestFont");
            var fontSmall = StaticData.Content.Load<SpriteFont>("Fonts/TestFontSmall");

            Button startButton = new Button(buttonTexture, font)
            { 
                Text = !StaticData.GameStarted ? "START" : "CONTINUE"
            };
            startButton.Click += startGame;

            _gamePadCheckBox = new CheckBoxButton(buttonTexture, fontSmall)
            {
                Text = "USE GAMEPAD",
                IsChecked = StaticData.GamePadEnabled
            };
            _gamePadCheckBox.Click += ToggleGamePad;

            Components.Add(startButton);
            Components.Add(_gamePadCheckBox);

            CalculateFirstComponentPosition();
        }

        private void ToggleGamePad(object sender, EventArgs e)
        {
            if (!StaticData.GamePadEnabled)
            {
                Input.EnableGamePadController();
            }
            else
            {
                Input.DisableGamePadController();
            }
        }

        protected void CalculateFirstComponentPosition()
        {
            int greatestWidth = 0;
            int combinedHeight = 0;

            foreach (var item in Components)
            {
                if (greatestWidth < item.Width)
                    greatestWidth = item.Width;
                
                combinedHeight += item.Height;
            }

            float x = StaticData.NativeWidth / 2 - greatestWidth / 2;
            float y = StaticData.NativeHeight / 2 - (combinedHeight + Padding * Components.Count - 1) / 2;

            _firstComponentPosition = new Vector2(x, y);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var item in Components)
            {
                item.Update(gameTime);
            }

            _gamePadCheckBox.IsChecked = StaticData.GamePadEnabled;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float y = _firstComponentPosition.Y;

            foreach (var item in Components)
            {
                item.Position = new Vector2(_firstComponentPosition.X, y);
                item.Draw(spriteBatch);
                y += item.Height + Padding;
            }
        }
    }
}