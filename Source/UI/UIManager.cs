using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.UI;

namespace ZA6.UI
{
    public static class UIManager
    {
        public static Menu CurrentMenu { get; private set; }
        private static UpdatableList<Menu> _menus = new UpdatableList<Menu>();
        private static UpdatableList<Menu> _alerts = new UpdatableList<Menu>();
        private static bool _disabledMenus;

        public static void CreateAlert(string message)
        {
            SFX.Error.Play();

            var alert = new Alert(message);

            _alerts.Add(alert);

            DisableMenus();

            CurrentMenu = alert;
        }

        public static void CreateConfirm(string message, EventHandler onYes)
        {
            SFX.Error.Play();

            var confirm = new Confirm(message, onYes);

            _alerts.Add(confirm);

            DisableMenus();

            CurrentMenu = confirm;
        }

        public static void Add(Menu menu)
        {
            if (menu is Menu == false)
                return;
            
            _menus.Add(menu);

            if (_disabledMenus)
                menu.Disabled = _disabledMenus;
            else
                CurrentMenu = menu;
        }

        public static void SetToRemove(Menu menu)
        {
            _menus.SetToRemove(menu);

            if (CurrentMenu == menu)
            {
                CurrentMenu = _menus.Count < 2 ? null : _menus[_menus.Count - 2];
            }
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

            //CurrentMenu = _menus.Count == 0 ? null : _menus[_menus.Count - 1];

            if (CurrentMenu != null)
            {
                if (!CurrentMenu.IsFocused)
                {
                    CurrentMenu.IsFocused = Input.P1.IsAnyKeyPressed();
                }

                CurrentMenu.Update(gameTime);
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

                CurrentMenu = _menus.Count == 0 ? null : _menus[_menus.Count - 1];
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentMenu != null)
            {
                CurrentMenu.Draw(spriteBatch);
            }
            /*foreach (var alert in _alerts)
            {
                alert.Draw(spriteBatch);
            }*/
        }

        private class Alert : Menu
        {
            public Alert(string message)
            {
                Padding = 10;
                Background = DefaultBackground;
                var fontSmall = Static.FontSmall;

                Components = new UIComponent[]
                {
                    new TextComponent(fontSmall, message),
                    new Button(fontSmall, OK) { Text = "OK I GUESS" }
                };
            }

            private void OK(object sender, EventArgs e)
            {
                UIManager._alerts.SetToRemove(this);
            }
        }

        private class Confirm : Menu
        {
            private EventHandler _onYes;
            public Confirm(string message, EventHandler onYes)
            {
                _onYes = onYes;
                Padding = 10;
                Background = DefaultBackground;
                var fontSmall = Static.FontSmall;

                Components = new UIComponent[]
                {
                    new TextComponent(fontSmall, message),
                    new Row()
                    {
                        Width = 150,
                        Height = 20,
                        Components = new UIComponent[]
                        {
                            new Button(fontSmall, OK) { Text = "YE YE" },
                            new Button(fontSmall, Cancel) { Text = "PLS NO" }
                        }
                    }
                };
            }


            private void OK(object sender, EventArgs e)
            {
                _onYes.Invoke(sender, e);
                UIManager._alerts.SetToRemove(this);
            }

            private void Cancel(object sender, EventArgs e)
            {
                UIManager._alerts.SetToRemove(this);
            }
        }
    }
}