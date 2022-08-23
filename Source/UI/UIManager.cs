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
        private static List<Menu> _menus = new List<Menu>();
        private static List<Menu> _alerts = new List<Menu>();
        private static bool _disabledMenus;

        public static bool IsDisplaying(Menu menu)
        {
            foreach (var item in _menus)
            {
                if (item == menu)
                    return true;
            }
            foreach (var item in _alerts)
            {
                if (item == menu)
                    return true;
            }

            return false;
        }

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
            var confirm = new Confirm(message, onYes);

            _alerts.Add(confirm);

            DisableMenus();

            CurrentMenu = confirm;
        }

        public static void Add(Menu menu)
        {
            _menus.Add(menu);

            if (_disabledMenus)
                menu.Disabled = _disabledMenus;
            else
                CurrentMenu = menu;
        }

        public static void Replace(Menu menu)
        {
            if (CurrentMenu != null)
            {
                menu.Replacing = CurrentMenu;
                _menus.Remove(CurrentMenu);
            }

            Add(menu);
        }

        public static void SetToRemove(Menu menu)
        {
            _menus.Remove(menu);

            if (menu.Replacing != null)
            {
                Add(menu.Replacing);
            }
            else if (CurrentMenu == menu)
            {
                CurrentMenu = _menus.Count == 0 ? null : _menus[_menus.Count - 1];
            }
        }

        public static void SetToClear()
        {
            _menus.Clear();
            _alerts.Clear();
            CurrentMenu = null;
        }

        public static void Update(GameTime gameTime)
        {
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
            foreach (var menu in _menus)
            {
                menu.Draw(spriteBatch);
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
                UIManager._alerts.Remove(this);
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
                UIManager._alerts.Remove(this);
            }

            private void Cancel(object sender, EventArgs e)
            {
                UIManager._alerts.Remove(this);
            }
        }
    }
}