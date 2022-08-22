using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;
using ZA6.UI;
using ZA6.Managers;

namespace ZA6
{
    public class GameMenu : Menu
    {
        private bool _startMenu;

        public GameMenu(EventHandler startGame, bool startMenu = false)
        {
            _startMenu = startMenu;

            if (!_startMenu) {
                OverlayColor =  new Color(50, 50, 50, 100);
            }
            else
            {
                Margin.Top = 50;
            }
            
            var fontSmall = Static.FontSmall;

            Button startButton = new Button(fontSmall)
            {
                Text = _startMenu 
                    ? (Static.LoadedGame == null ? "START" : "CONTINUE")
                    : "RESUME",
                Height = 20
            };
            startButton.Click += startGame;

            CheckBoxButton gamePadCheckBox = new CheckBoxButton(fontSmall)
            {
                Text = "USE GAMEPAD",
                Height = 20,
                IsChecked = Static.GamePadEnabled
            };
            gamePadCheckBox.Click += ToggleGamePad;

            Button settingsButton = new Button(fontSmall)
            {
                Text = "CONFIG",
                Height = 20
            };
            settingsButton.Click += GoToSettings;

            Button quitButton = new Button(fontSmall)
            {
                Text = "QUIT",
                Height = 20
            };
            quitButton.Click += QuitGame;

            Components = new UIComponent[]
            {
                startButton,
                settingsButton,
                gamePadCheckBox,
                quitButton
            };
        }

        private void GoToSettings(object sender, EventArgs e)
        {
            UIManager.Add(new SettingsMenu(_startMenu));
        }

        private void QuitGame(object sender, EventArgs e)
        {
            Static.Game.Exit();
        }

        private void ToggleGamePad(object sender, EventArgs e)
        {
            if (!Static.GamePadEnabled)
            {
                try
                {
                    Input.EnableGamePadController();
                }
                catch (System.Exception ex)
                {
                    Sys.LogError(ex.Message);
                    UIManager.CreateAlert(ex.Message);
                }
            }
            else
            {
                Input.DisableGamePadController();
            }
        }
    }
}