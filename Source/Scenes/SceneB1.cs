using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TapsasEngine.Enums;
using ZA6.Animations;
using ZA6.Managers;
using ZA6.Models;

namespace ZA6
{
    public class SceneB1 : Scene
    {
        private Texture2D _overlay;
        private ScaredBird _bird;
        private Animations.RealOwl _realOwlAnimation;
        private Animations.SeppoScreamer _seppoScreamer;

        public SceneB1()
        {
            if (!Static.SessionData.Get("eaten mushroom"))
            {
                Theme = Static.Content.Load<Song>("linktothepast/forest");
            }
            else
            {
                Theme = Static.Content.Load<Song>("mushroom_song");
            }

            if (Static.GameData.GetString("scenario") != "noise")
                ExitTransitions[Direction.Left] = TransitionType.FadeToBlack;
            else
                ExitTransitions[Direction.Left] = TransitionType.GoToRitual;
            
            ExitTransitions[Direction.Down] = TransitionType.FadeToBlack;

        }

        protected override void Load()
        {
            _overlay = Static.Content.Load<Texture2D>("linktothepast/shadedwoodtransparency");

            if (Static.SessionData.Get("bird scared"))
            {
                return;
            }
            else if (!Static.GameStarted)
            {
                _bird = new ScaredBirdParrot() { Position = TileMap.ConvertTileXY(50, 21) };
                Add(_bird);
            }
            else if (Static.GameData.GetInt("bird scare count") == 0)
            {
                _bird = new ScaredBirdParrot() { Position = TileMap.ConvertTileXY(50, 21) };
                Add(_bird);
            }
            else if (Static.GameData.GetInt("bird scare count") == 1)
            {
                _bird = new ScaredBirdParrot() { Position = TileMap.ConvertTileXY(50, 21) };
                Add(_bird);
            }
            else if (Static.GameData.GetInt("bird scare count") == 2)
            {   
                _realOwlAnimation = new Animations.RealOwl(Player, TileMap.ConvertTileXY(50, 21));
            }
        }

        public override void Start()
        {
            if (!Static.GameStarted)
                // If game is not started, this scene is created for the title screen
                return;

            base.Start();

            if (Static.GameData.GetString("scenario")  == "crispy")
            {
                _seppoScreamer = new Animations.SeppoScreamer();
                _seppoScreamer.Enter();
                return;
            }
            
            if (_realOwlAnimation != null)
            {
                _realOwlAnimation.DrawOffset = DrawOffset;
                _realOwlAnimation.Enter();
            }

            if (!Static.SessionData.Get("title shown"))
            { 
                Static.Game.TitleText.Enter();
                Static.GameData.Save("title shown", true);
            }
            
            if (Static.SessionData.Get("eaten mushroom"))
            {
                Locked = true;

                Static.EventSystem.Load(
                    new Event[]
                    {
                        new AnimateEvent(new Animations.High()) { Wait = false },
                        new WaitEvent(30f),
                        new RunEvent(() => {
                            Static.GameData.Save("scenario", "noise");
                            Static.Game.StateMachine.TransitionTo(
                                "GameOver",
                                new GameStateGameOver.Args() { Question = "Haha drug joke fun" }
                            );
                        })
                    },
                    EventSystem.Settings.Parallel | EventSystem.Settings.SustainSceneChange
                );
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_seppoScreamer != null)
            {
                _seppoScreamer.Update(gameTime);
            }
            
            if (_realOwlAnimation != null)
            {
                _realOwlAnimation.DrawOffset = DrawOffset;
                _realOwlAnimation.OwlSprite.Update(gameTime);
                _realOwlAnimation.Update(gameTime);
            }
        }

        public override void Exit()
        {
            if (_realOwlAnimation != null)
            {
                _realOwlAnimation.Exit();
            }
        }

        // NOTE drawing tree shadow overlay on top of dialog looks cool 
        public override void DrawOverlay(SpriteBatch spriteBatch)
        {
            // Reduce native size for panning
            var overlayPosition = OverlayOffset + DrawOffset * 0.5f - Static.NativeSize;
            
            if (_seppoScreamer != null)
            {
                _seppoScreamer.Draw(spriteBatch);
            }

            spriteBatch.Draw(
                _overlay,
                overlayPosition,
                new Rectangle(0, 0, Width * 3, Height * 2),
                new Color(255, 255, 255, 0.5f)
            );
        }
    }
}