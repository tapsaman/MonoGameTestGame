using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Utilities;
using TapsasEngine;
using ZA6.UI;

namespace ZA6
{
    public class SettingsMenu : Menu
    {
        private bool _startMenu;
        private Slider _musicVolSlider;
        private Slider _sfxVolSlider;
        private Dropdown<RenderResolution> _resolutionDropdown;
        private Button _clearSaveButton;

        public SettingsMenu(bool startMenu)
        {
            _startMenu = startMenu;
            
            if (!_startMenu)
                OverlayColor = new Color(50, 50, 50, 100);
            
            var font = Static.Content.Load<SpriteFont>("Fonts/TestFont");
            var fontSmall = Static.FontSmall;
            
            Row controlRow = new Row()
            {
                Width = 200,
                Height = 20,
                Components = new UIComponent[]
                {
                    new Button(fontSmall, Save) { Text = "Save" },
                    new Button(fontSmall, Cancel) { Text = "Cancel" },
                }
            };

            _clearSaveButton = new Button(fontSmall)
            {
                Text = "CLEAR SAVE"
            };
            _clearSaveButton.Click += ClearSave;

            _musicVolSlider = new Slider(fontSmall, 0f, 1f, 0.1f)
            {
                Text = "MUSIC VOL",
                Value = Music.Volume
            };
            _musicVolSlider.OnChange += ChangeMusicVol;

            _sfxVolSlider = new Slider(fontSmall, 0f, 1f, 0.1f)
            {
                Text = "SOUND VOL",
                Value = SFX.Volume
            };
            _sfxVolSlider.OnChange += ChangeSFXVol;

            _resolutionDropdown = new Dropdown<RenderResolution>(fontSmall, Static.ResolutionOptions)
            {
                Text = "RESOLUTION",
                Value = Static.Renderer.Resolution
            };
            _resolutionDropdown.OnChange += ChangeResolution;

            Components = new UIComponent[]
            {
                _resolutionDropdown,
                _musicVolSlider,
                _sfxVolSlider,
                _clearSaveButton,
                new EmptySpace(0, 10),
                controlRow
            };
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
            _resolutionDropdown.Value = Static.Renderer.Resolution;
        }

        private void Save(object sender, EventArgs e)
        {
            SavedConfig.CreateAndSave();
            UIManager.SetToRemove(this);
        }

        private void Cancel(object sender, EventArgs e)
        {
            SavedConfig.LoadAndApply();
            UIManager.SetToRemove(this);
        }

        private void ClearSave(object sender, EventArgs e)
        {
            UIManager.CreateConfirm(
                "Are you sure to clear\nyour progress and\nstart over?",
                (object sender, EventArgs e) => {
                    SaveData.Clear();
                    
                    if (!_startMenu)
                    {
                        Static.Game.StateMachine.TransitionTo("StartMenu");
                    }
                }
            );
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.P1.JustPressed(Input.P1.B) || Input.P1.JustPressed(Input.P1.Select))
            {
                Cancel(this, new EventArgs());
                return;
            }

            _clearSaveButton.Disabled = Static.LoadedGame == null;
            base.Update(gameTime);
        }
    }
}