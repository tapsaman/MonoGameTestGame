using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Controls;

namespace ZA6.Managers
{
    public static class UI
    {
        public static Menu CurrentMenu { get; private set; }
        private static UpdatableList<Menu> _menus = new UpdatableList<Menu>();
        private static UpdatableList<Alert> _alerts = new UpdatableList<Alert>();
        private static bool _disabledMenus;

        public static void CreateAlert(string message)
        {
            _alerts.Add(new Alert(message));

            DisableMenus();

            SFX.Error.Play();
        }

        public static void Add(Menu menu)
        {
            if (menu is Alert)
                return;
            
            _menus.Add(menu);

            menu.Disabled = _disabledMenus;
        }

        public static void SetToRemove(Menu menu)
        {
            _menus.SetToRemove(menu);
        }

        public static void SetToClear()
        {
            foreach (var menu in _menus)
            {
                _menus.SetToRemove(menu);
            }
            foreach (var alert in _alerts)
            {
                _alerts.SetToRemove(alert);
            }
            CurrentMenu = null;
        }

        public static void Update(GameTime gameTime)
        {
            _menus.Update();
            _alerts.Update();

            CurrentMenu = _menus.Count == 0 ? null : _menus[_menus.Count - 1];

            if (!_disabledMenus && _menus.Count != 0)
            {
                CurrentMenu.Update(gameTime);
            }
            foreach (var alert in _alerts)
            {
                alert.Update(gameTime);
            }
            
            if (_disabledMenus && _alerts.Count == 0)
            {
                // Last alert
                EnableMenus();
            }
        }

        private static void DisableMenus()
        {
            if (!_disabledMenus)
            {
                foreach (var menu in _menus)
                {
                    menu.Disabled = true;
                }

                _disabledMenus = true;
            }
        }

        private static void EnableMenus()
        {
            if (_disabledMenus)
            {
                foreach (var menu in _menus)
                {
                    menu.Disabled = false;
                }
                
                _disabledMenus = false;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentMenu != null)
            {
                CurrentMenu.Draw(spriteBatch);
            }
            foreach (var alert in _alerts)
            {
                alert.Draw(spriteBatch);
            }
        }

        private class Alert : Menu
        {
            public Alert(string message)
            {
                Padding = 16;
                var buttonTexture = Static.Content.Load<Texture2D>("Button");
                _bgTexture = buttonTexture;
                var font = Static.Content.Load<SpriteFont>("Fonts/TestFont");
                var fontSmall = Static.Content.Load<SpriteFont>("Fonts/TestFontSmall");

                var text = new TextComponent(fontSmall, message);

                var okButton = new Button(buttonTexture, fontSmall)
                {
                    Text = "OK I GUESS"
                };
                
                okButton.Click += OK;

                Add(text);
                Add(okButton);

                CalculateSize();
            }

            private void OK(object sender, EventArgs e)
            {
                UI._alerts.SetToRemove(this);
            }
        }
    }
}