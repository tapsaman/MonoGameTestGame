using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Utilities;
using TapsasEngine;
using ZA6.Controls;
using ZA6.Managers;

namespace ZA6
{
    public class SettingsMenu : Menu
    {
        private CheckBoxButton _gamePadCheckBox;
        private Slider _musicVolSlider;
        private Slider _sfxVolSlider;
        private Select<RenderResolution> _resolutionSelect;

        public SettingsMenu(bool startMenu)
        {
            if (!startMenu)
                OverlayColor = new Color(50, 50, 50);
            
            var buttonTexture = Static.Content.Load<Texture2D>("Button");
            var font = Static.Content.Load<SpriteFont>("Fonts/TestFont");
            var fontSmall = Static.Content.Load<SpriteFont>("Fonts/TestFontSmall");

            Button backButton = new Button(buttonTexture, font)
            {
                Text = "< BACK"
            };
            backButton.Click += GoBack;

            _gamePadCheckBox = new CheckBoxButton(buttonTexture, fontSmall)
            {
                Text = "USE GAMEPAD",
                IsChecked = Static.GamePadEnabled
            };
            _gamePadCheckBox.Click += ToggleGamePad;

            _musicVolSlider = new Slider(buttonTexture, fontSmall, 0f, 1f, 0.1f)
            {
                Text = "MUSIC VOL",
                Value = Music.Volume
            };
            _musicVolSlider.OnChange += ChangeMusicVol;

            _sfxVolSlider = new Slider(buttonTexture, fontSmall, 0f, 1f, 0.1f)
            {
                Text = "SOUND VOL",
                Value = SFX.Volume
            };
            _sfxVolSlider.OnChange += ChangeSFXVol;

            _resolutionSelect = new Select<RenderResolution>(buttonTexture, fontSmall, Static.ResolutionOptions)
            {
                Text = "RESOLUTION",
                Value = Static.Renderer.Resolution
            };
            _resolutionSelect.OnChange += ChangeResolution;

            Add(_resolutionSelect);
            Add(_gamePadCheckBox);
            Add(_musicVolSlider);
            Add(_sfxVolSlider);
            Add(backButton);

            CalculateSize();
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
                    UI.CreateAlert(ex.Message);
                }
            }
            else
            {
                Input.DisableGamePadController();
            }
        }

        private void ChangeMusicVol(float value)
        {
            Sys.Log("new volume " + value);
            Music.Volume = value;
            _musicVolSlider.Value = Music.Volume;
        }

        private void ChangeSFXVol(float value)
        {
            Sys.Log("new volume " + value);
            SFX.Volume = value;
            _sfxVolSlider.Value = SFX.Volume;
        }

        private void ChangeResolution(RenderResolution res)
        {
            Static.Renderer.Resolution = res;
            _resolutionSelect.Value = Static.Renderer.Resolution;
        }

        private void GoBack(object sender, EventArgs e)
        {
            UI.SetToRemove(this);
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.P1.JustPressed(Input.P1.B) || Input.P1.JustPressed(Input.P1.Select))
            {
                GoBack(this, new EventArgs());
                return;
            }

            _gamePadCheckBox.IsChecked = Static.GamePadEnabled;
            base.Update(gameTime);
        }
    }
}