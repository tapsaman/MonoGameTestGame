
using System;
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
            Shaders.Highlight,
            Shaders.VHS
        };
        public string[] GameStateKeyList =
        {
            "Default",
            "MainMenu",
            "Intro",
            "Dialog",
            "Cutscene",
            "GameOver",
            "StartMenu",
            "Cartoon"
        };
        public string[] ScenarioList =
        {
            null,
            "hole",
            "crispy",
            "mushroom",
            "noise",
            "arrows",
            "scrambled",
            "tape",
            "end"
        };
        private bool _askingNumber;
        private Func<int, string> _onNumberAnswer;

        public DevUtils()
        {
            Actions = new DevToolAction[]
            {
                new DevToolAction(Keys.F2, "Toggle hitboxes", ToggleHitboxes),
                new DevToolAction(Keys.F3, "Toggle dev info", ToggleDevInfo),
                new DevToolAction(Keys.F4, "Toggle collision map", ToggleCollisionMap),
                new DevToolAction(Keys.F5, "Toggle no clip mode", ToggleNoClip),
                new DevToolAction(Keys.F6, "Go to map", StartGoToMap),
                new DevToolAction(Keys.F7, "Go to game state", StartGoToGameState),
                new DevToolAction(Keys.F8, "Go to scenario", StartGoToScenario),
                new DevToolAction(Keys.F9, "Apply shader", StartApplyShader),
                new DevToolAction(Keys.F10, "Check game data", CheckGameData),
                new DevToolAction(Keys.F11, "Give player health", StartGiveHealth),
                new DevToolAction(Keys.F12, "Damage player", StartDamagePlayer)
            };
        }

        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;

            base.Update(gameTime);

            if (_askingNumber)
            {
                int? number = Input.P1.AnyNumberKeyJustPressed();

                if (number != null)
                {
                    Reset();
                    SetMessage(_onNumberAnswer((int)number));
                }
            }
        }

        public override void Reset()
        {
            _askingNumber = false;
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

        private string CheckGameData()
        {
            return "\n" + Static.GameData.ToString();
        }

        private string StartGiveHealth()
        {
            _askingNumber = true;
            _onNumberAnswer = GiveHealth;
            return "Input number to replenish health";
        }

        private string StartDamagePlayer()
        {
            _askingNumber = true;
            _onNumberAnswer = DamagePlayer;
            return "Input number to do damage";
        }

        private string StartGoToMap()
        {
            _askingNumber = true;
            _onNumberAnswer = GoToMap;
            return "Select map" + ArrayToListString(Static.SceneManager.World.Maps);
        }

        private string GoToMap(int index)
        {
            if (index >= Static.SceneManager.World.Maps.Length)
            {
                return "Error: No map by index " + index;
            }

            var map = Static.SceneManager.World.Maps[index];

            Static.Game.StateMachine.TransitionTo(
                "StartOver",
                new GameStateStartOver.Args() { MapName = map.Name }
            );

            return "Started over with map name " + map.Name;
        }

        private string StartApplyShader()
        {
            _askingNumber = true;
            _onNumberAnswer = ApplyShader;
            return "Select shader" + ArrayToListString(ShaderList);
        }

        private string ApplyShader(int index)
        {
            if (index >= ShaderList.Length)
            {
                return "Error: No shader by index " + index;
            }

            var shader = ShaderList[index];

            Static.Renderer.ApplyPostEffect(shader);

            if (shader == null)
                return "Removed post effects";
            else
                return "Applied post effect " + shader.Name;
        }

        private string StartGoToGameState()
        {
            _askingNumber = true;
            _onNumberAnswer = GoToGameState;
            return "Select game state" + ArrayToListString(GameStateKeyList);
        }

        private string GoToGameState(int index)
        {
            if (index >= GameStateKeyList.Length)
            {
                return "Error: No game state by index " + index;
            }

            var gameState = GameStateKeyList[index];

            Static.Game.StateMachine.TransitionTo(gameState);

            return "Transitioned to game state " + gameState;
        }

        private string StartGoToScenario()
        {
            _askingNumber = true;
            _onNumberAnswer = GoToScenario;
            return "Select scenario" + ArrayToListString(ScenarioList);;
        }

        private string GoToScenario(int index)
        {
            if (index >= ScenarioList.Length)
            {
                return "Error: No scenario by index " + index;
            }

            var scenario = ScenarioList[index];

            Static.GameData.Save("scenario", scenario);

            return "Set scenario to '" + scenario + "'";
        }

        private void DrawDevInfo(SpriteBatch spriteBatch)
        {
            Static.SpriteBatch.DrawString(
                Static.Font,
                "Game state: " + Static.Game.StateMachine.CurrentStateKey
                    + "\nMap: " + Static.Scene.TileMap.Name
                    + "\nDraw offset: " + Static.Scene.Camera.Offset
                    + "\nPaused: " + Static.Scene.Paused,
                Vector2.One,
                TextColor
            );

            DrawCharacterData(spriteBatch);
        }

        private static string ArrayToListString(object[] array)
        {
            string txt = "";

            for (int i = 0; i < array.Length; i++)
            {
                txt += "\n(" + i + ") " + (array[i] == null ? "None" : array[i]);
            }

            return txt;
        }

        private void DrawMapGrid(SpriteBatch spriteBatch)
        {
            var scene = Static.Scene;
            var sizeMultiplier = Static.Renderer.NativeSizeMultiplier;

            var color = new Color(0, 0, 0, 120);
            var x = Static.Renderer.ScreenRectangle.X + Static.Scene.Camera.Offset.X * sizeMultiplier.X + 1;
            var y = Static.Renderer.ScreenRectangle.Y +Static. Scene.Camera.Offset.Y * sizeMultiplier.Y + 1;
            
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
                var pos = (new Vector2(c.Position.X, c.Hitbox.Rectangle.Bottom) + Static.Scene.Camera.Offset) * Static.Renderer.NativeSizeMultiplier + Static.Renderer.ScreenPosition;

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