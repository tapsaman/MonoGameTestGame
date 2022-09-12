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
                OverlayColor =  new Color(40, 40, 100, 215);
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
                IsChecked = Input.GamePadEnabled
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
            quitButton.Click += AskToQuitGame;

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
            UIManager.Replace(new SettingsMenu(_startMenu));
        }

        private void AskToQuitGame(object sender, EventArgs e)
        {
            if (_startMenu)
            {
                Static.Game.Exit();
            }
            else
            {
                UIManager.CreateConfirm(
                    "Are you sure to quit\nthe game?\nUnsaved progress will\nbe lost.",
                    (object sender, EventArgs e) =>
                    {
                        Static.Game.StateMachine.TransitionTo("StartMenu");
                    }
                );
            }
        }

        private void ToggleGamePad(object sender, EventArgs e)
        {
            if (!Input.GamePadEnabled)
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