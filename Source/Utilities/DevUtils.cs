
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TapsasEngine.Utilities;
using ZA6.Models;

namespace ZA6
{
    public class DevUtils : DevTool
    {
        public bool RenderDevInfo;
        public Effect[] ShaderList =
        {
            null,
            Shaders.EnemyDamage,
            Shaders.HighContrast,
            Shaders.Rainbow,
            Shaders.InvertedColor,
            Shaders.Wavy,
            Shaders.Noise,
            Shaders.MildNoise,
            Shaders.Highlight
        };
        private bool _askingMapIndex;
        private bool _askingShaderIndex;
        private bool _askingHealthAmount;
        private bool _askingDamageAmount;

        public DevUtils()
        {
            Actions = new DevToolAction[]
            {
                new DevToolAction(Keys.F2, "Toggle hitboxes", ToggleHitboxes),
                new DevToolAction(Keys.F3, "Toggle dev info", ToggleDevInfo),
                new DevToolAction(Keys.F4, "Toggle collision map", ToggleCollisionMap),
                new DevToolAction(Keys.F5, "Toggle no clip mode", ToggleNoClip),
                new DevToolAction(Keys.F6, "Select map", StartGoToMap),
                new DevToolAction(Keys.F7, "Select shader", StartApplyShader),
                new DevToolAction(Keys.F8, "Check game data", CheckGameData),
                new DevToolAction(Keys.F9, "Give player health", StartGiveHealth),
                new DevToolAction(Keys.F10, "Damage player", StartDamagePlayer),
                new DevToolAction(Keys.F11, "Force game over", GameOver),
                new DevToolAction(Keys.F12, "Start menu", StartMenu)
            };
        }

        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;

            base.Update(gameTime);

            if (_askingMapIndex)
            {
                int? number = Input.P1.AnyNumberKeyJustPressed();

                if (number != null)
                {
                    Reset();
                    SetMessage(GoToMap((int)number));
                }
            }
            else if (_askingShaderIndex)
            {
                int? number = Input.P1.AnyNumberKeyJustPressed();

                if (number != null)
                {
                    Reset();
                    SetMessage(ApplyShader((int)number));
                }
            }
            else if (_askingHealthAmount)
            {
                int? number = Input.P1.AnyNumberKeyJustPressed();

                if (number != null)
                {
                    Reset();
                    SetMessage(GiveHealth((int)number));
                }
            }
            else if (_askingDamageAmount)
            {
                int? number = Input.P1.AnyNumberKeyJustPressed();

                if (number != null)
                {
                    Reset();
                    SetMessage(DamagePlayer((int)number));
                }
            }
        }

        public override void Reset()
        {
            _askingShaderIndex = false;
            _askingMapIndex = false;
            _askingHealthAmount = false;
            _askingDamageAmount = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (RenderDevInfo)
            {
                DrawDevInfo(spriteBatch);
            }
            if (Static.RenderCollisionMap)
            {
                DrawMapGrid(spriteBatch);
            }
        }

        private string ToggleHitboxes()
        {
            Static.RenderHitboxes = !Static.RenderHitboxes;
            return "Render hitboxes: " + Static.RenderHitboxes;
        }

        private string ToggleNoClip()
        {
            Static.Player.NoClip = !Static.Player.NoClip;
            return "No clip mode: " + Static.Player.NoClip;
        }

        private string ToggleCollisionMap()
        {
            Static.RenderCollisionMap = !Static.RenderCollisionMap;
            return "Render collision map " + Static.RenderCollisionMap;
        }

        private string ToggleDevInfo()
        {
            RenderDevInfo = !RenderDevInfo;
            return "Render dev info: " + RenderDevInfo;
        }

        private string GiveHealth(int amount)
        {
            Static.Player.ReplenishHealth(amount);
            return "Player health + " + amount;
        }

        private string DamagePlayer(int amount)
        {
            if (!Static.GameStarted)
            {
                return "Can't do this dev tool action until game has started";
            }
            Static.Player.TakeDamage(Vector2.Zero, amount);
            return "Damaged player from " + Vector2.Zero + " by " + amount;
        }

        private string GameOver()
        {
            if (!Static.GameStarted)
            {
                return "Can't do this dev tool action until game has started";
            }
            Static.Game.StateMachine.TransitionTo("GameOver");
            return "Game over";
        }

        private string CheckGameData()
        {
            return "\n" + Static.GameData.ToString();
        }

        private string StartMenu()
        {
            Static.Game.StateMachine.TransitionTo("StartMenu");
            return "Start menu";
        }

        private string StartGiveHealth()
        {
            _askingHealthAmount = true;
            return "Input number to replenish health";
        }

        private string StartDamagePlayer()
        {
            _askingDamageAmount = true;
            return "Input number to do damage";
        }

        private string StartGoToMap()
        {
            var maps = Static.SceneManager.World.Maps;
            string txt = "Select map";
            
            for (int i = 0; i < maps.Length; i++)
            {
                txt += "\n(" + i + ") " + maps[i].Name;
            }

            _askingMapIndex = true;
            return txt;
        }

        private string GoToMap(int mapIndex)
        {
            var maps = Static.SceneManager.World.Maps;

            if (mapIndex >= maps.Length)
            {
                return "Error: No map by index " + mapIndex;
            }

            var map = maps[mapIndex];

            //Static.SceneManager.Init(map.Name);
            Static.Game.StateMachine.TransitionTo(
                "StartOver",
                new GameStateStartOver.Args() { MapName = map.Name }
            );

            return "Started over with map name " + map.Name;
        }

        private string StartApplyShader()
        {
            var shaders = ShaderList;
            string txt = "Select shader";
            
            for (int i = 0; i < shaders.Length; i++)
            {
                var shader = shaders[i];
                txt += "\n(" + i + ") ";

                if (shader == null)
                    txt += "None";
                else
                    txt += shader.Name;
            }

            _askingShaderIndex = true;
            return txt;
        }

        private string ApplyShader(int shaderIndex)
        {
            var shaders = ShaderList;

            if (shaderIndex >= shaders.Length)
            {
                return "Error: No shader by index " + shaderIndex;
            }

            var shader = shaders[shaderIndex];

            Static.Renderer.ApplyPostEffect(shader);

            if (shader == null)
                return "Removed post effects";
            else
                return "Applied post effect " + shader.Name;
        }

        private void DrawDevInfo(SpriteBatch spriteBatch)
        {
            Static.SpriteBatch.DrawString(
                Static.Font,
                "Game state: " + Static.Game.StateMachine.CurrentStateKey
                    + "\nMap: " + Static.Scene.TileMap.Name,
                Vector2.One,
                TextColor
            );

            DrawCharacterData(spriteBatch);
        }

        private void DrawMapGrid(SpriteBatch spriteBatch)
        {
            var scene = Static.Scene;
            var sizeMultiplier = Static.Renderer.NativeSizeMultiplier;

            var color = new Color(0, 0, 0, 120);
            var x = Static.Renderer.ScreenRectangle.X + scene.DrawOffset.X * sizeMultiplier.X + 1;
            var y = Static.Renderer.ScreenRectangle.Y + scene.DrawOffset.Y * sizeMultiplier.Y + 1;
            
            while (x < Static.Renderer.ScreenRectangle.Right)
            {    
                spriteBatch.DrawLine(
                    new Vector2(x, 0),
                    new Vector2(x, Static.NativeHeight * sizeMultiplier.Y),
                    color
                );
                x += scene.TileMap.TileWidth * sizeMultiplier.X;
            }
            for (int i = 0; i < scene.TileMap.Height; i++)
            {
                spriteBatch.DrawLine(
                    new Vector2(0, y),
                    new Vector2(Static.NativeWidth * sizeMultiplier.X, y),
                    color
                );
                y += scene.TileMap.TileHeight * sizeMultiplier.Y;
            }
        }

        private void DrawCharacterData(SpriteBatch spriteBatch)
        {
            var characters = Static.Scene.Characters;

            foreach (var c in characters)
            {
                var txt = CharacterToDataString(c);
                var pos = (new Vector2(c.Position.X, c.Hitbox.Rectangle.Bottom) + Static.Scene.DrawOffset) * Static.Renderer.NativeSizeMultiplier + Static.Renderer.ScreenPosition;

                Static.SpriteBatch.DrawString(
                    Static.Font,
                    txt,
                    pos,
                    TextColor
                );
            }
        }

        private static string CharacterToDataString(Character c)
        {
            string txt = "";

            if (c.StateMachine != null)
            {
                txt += c.StateMachine.CurrentStateKey + "\n";
            }
            
            txt += "XY: " + c.Position.X
                + "/" + c.Position.Y + "\n";
            
            if (c.Level == MapLevel.Character)
            {
                txt += "CollX: " + c.CollisionX
                    + "\nCollY: " + c.CollisionY;
            }

            return txt;
        }
    }
}